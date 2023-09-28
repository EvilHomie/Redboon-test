using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryLogic : MonoBehaviour, IDropHandler
{    
    [SerializeField] protected GameObject inventorySlotPrefab;
    [SerializeField] protected Transform slotsConteiner;
    [SerializeField] protected int changePriceMod;
    [SerializeField] int defAmountOfSlots;
    [SerializeField] int maxAmountOfSlots;    
    public int maxStackSize;     
    protected List<GameObject> inventorySlots = new();
    int collumsAmout;
    protected int mergeCount;

    protected virtual void Start()
    {
        collumsAmout = slotsConteiner.GetComponent<GridLayoutGroup>().constraintCount;
        AddNewSlots(defAmountOfSlots);        
    }

    // Событие при "Броске" объекта в границах объекта
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out InventoryItem item); // Проверка предмета является ли он предметом инвентаря

        if (item == null) return;  // Остановка метода если предмет не является предметом инвентаря
        TryPutItemInInventory(item);
    }

    public virtual void TryPutItemInInventory(InventoryItem item)
    {
        //mergeCount = item.count;
        //Debug.Log($"translateItemCount   {mergeCount}");
        Debug.Log("TryPutItemInInventory");
        if (item.basicItem.stackable) // Проверка является ли предмет штабелируемым
        {
            InventoryItem suitableItem = FindSimilarItemAndNotFullStackInSlots(item); // Поиск подходящего предмета среди предметов в слотах (того же типа и не имеющего максимальный размер стака)

            if (suitableItem != null) // Проверка найден ли подходящий предмет
            {
                Debug.Log("suitableItem foud");
                MergingItems(suitableItem, item); // Слияние нового предмета с подходящим

                if (item.count != 0)
                {
                    TryPutItemInInventory(item); // Проверка остатка после слияния и запуск нового цикла попытки запихнуть в инвентарь если остаток не равен нулю
                }

                return;
            }
            Debug.Log("suitableItem not foud");
        }        

        TryPutInFreeSlot(item);
    }

    void TryPutInFreeSlot(InventoryItem item)
    {
        Debug.Log("TryPutInFreeSlot");
        GameObject freeSlot = FindFreeSlot(); // Поиск свободного слота

        // Действия если найден свободный слот
        if (freeSlot != null)
        {
            Debug.Log("PutInFreeSlot");
            PutItemInSlot(item, freeSlot); // Перенос объекта в свободный слот
        }
        // Действия если свободный слот не найден
        else if (freeSlot == null)
        {
            Debug.Log("TryAddNewSlots");
            if (inventorySlots.Count < maxAmountOfSlots) // Проверка не достигнуто ли максимальное кол-во слотов
            {
                Debug.Log("AddNewSlots");
                AddNewSlots(collumsAmout); // Создание нового ряда слотов
                GameObject newFreeSlot = FindFreeSlot(); // Поиск свободного слота
                PutItemInSlot(item, newFreeSlot); // Перенос объекта в новый свободный слот
            }
            else // Событие если достигнуто максимальное кол-во слотов
            {
                Debug.Log("Max Slots Count Reached"); 
                item.transform.position = Vector3.zero;
            }
        }
    }

    public virtual void MergingItems(InventoryItem suitableItem, InventoryItem item)
    {
        Debug.Log("TryMerge");
        int freeSpaceInSlot = maxStackSize - suitableItem.count;
        if (freeSpaceInSlot < item.count)
        {
            mergeCount = freeSpaceInSlot;
            suitableItem.count += freeSpaceInSlot;
            item.count -= freeSpaceInSlot;
            Debug.Log("PartiallyMerge");
            suitableItem.RefreshCount();
            item.RefreshCount();
            
        }
        else
        {
            mergeCount = item.count;
            suitableItem.count += item.count;            
            item.count = 0;
            Destroy(item.gameObject);
            Debug.Log("FullMerge");
            suitableItem.RefreshCount();
            item.RefreshCount();            
        }
    }
    public InventoryItem FindSimilarItemAndNotFullStackInSlots(InventoryItem item)
    {
        List<GameObject> busySlotsList = inventorySlots.FindAll(slot => slot.GetComponentInChildren<InventoryItem>() != null);
        if (busySlotsList == null) { return null; }

        List<GameObject> slotsWithSimilarItem = busySlotsList.FindAll(slot => slot.GetComponentInChildren<InventoryItem>().basicItem == item.basicItem);
        if (slotsWithSimilarItem == null) { return null; }

        GameObject notFullSlot = slotsWithSimilarItem.Find(slot => slot.GetComponentInChildren<InventoryItem>().count < maxStackSize);
        if (notFullSlot == null) { return null; }

        InventoryItem suitableItem = notFullSlot.GetComponentInChildren<InventoryItem>();
        return suitableItem;
    }
    protected virtual void AddNewSlots(int AmoutOfNewSlots)
    {
        for (int i = 0; i < AmoutOfNewSlots; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, slotsConteiner.transform);
            inventorySlots.Add(newSlot);
        }
    }

    public GameObject FindFreeSlot()
    {
        GameObject freeSlot = inventorySlots.Find(slot => slot.GetComponentInChildren<InventoryItem>() == null);
        return freeSlot;
    }

    public virtual void PutItemInSlot(InventoryItem item, GameObject slot)
    {
        DragAndDropLogic script = item.GetComponent<DragAndDropLogic>();
        script.deffParent = slot.transform;
    }

    //public void DeleteFreeSlot(GameObject slot)
    //{
    //    Destroy(slot);
    //    if (inventorySlots.Count > defAmountOfSlots)
    //    {
    //        GameObject freeSlot = FindFreeSlot();
    //        Destroy(freeSlot);
    //    }
    //}
}
