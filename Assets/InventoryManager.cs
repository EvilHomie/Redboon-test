using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IDropHandler
{
    [SerializeField] protected GameObject inventorySlotPrefab;
    [SerializeField] int defAmountOfSlots;
    [SerializeField] int maxAmountOfSlots;
    [SerializeField] int maxStackSize;

    protected Transform slotsConteiner;
    protected List<GameObject> inventorySlots = new();
    int collumsAmout;

    private void Start()
    {
        slotsConteiner = transform.GetComponentInChildren<GridLayoutGroup>().transform;
        collumsAmout = slotsConteiner.GetComponent<GridLayoutGroup>().constraintCount;

        AddNewSlots(defAmountOfSlots);
    }

    // Событие при "Броске" объекта в границах объекта
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out InventoryItem item); // Проверка предмета является ли он предметом инвентаря

        if (item != null)
        {     
            if (item.basicItem.stackable && FindSimilarItemInSlots(item))
            {                
                return;
            }
            
            GameObject freeSlot = FindFreeSlot(); // Поиск свободного слота

            // Действия если найден свободный слот
            if (freeSlot != null)
            {
                ChangeItemParent(item, freeSlot); // Перенос объекта в свободный слот
            }
            // Действия если свободный слот не найден
            else if (freeSlot == null)
            {
                if (inventorySlots.Count < maxAmountOfSlots) // Проверка не достигнуто ли максимальное кол-во слотов
                {
                    AddNewSlots(collumsAmout); // Создание нового ряда слотов
                    GameObject newFreeSlot = FindFreeSlot(); // Поиск свободного слота
                    ChangeItemParent(item, newFreeSlot); // Перенос объекта в новый свободный слот
                }
                else
                {
                    Debug.Log("Max Slots Count Reached"); // Событие если достигнуто максимальное кол-во слотов
                }
            }
        }
    }

    protected virtual void AddNewSlots(int AmoutOfNewSlots)
    {
        for (int i = 0; i < AmoutOfNewSlots; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, slotsConteiner.transform);
            inventorySlots.Add(newSlot);
        }
    }

    bool FindSimilarItemInSlots(InventoryItem item)
    {
        List  <GameObject> busySlotsList = inventorySlots.FindAll (slot => slot.GetComponentInChildren<InventoryItem>() != null);
        GameObject slotWithSimilarItem = busySlotsList.Find(slot => slot.GetComponentInChildren<InventoryItem>().basicItem == item.basicItem);
        
        if (slotWithSimilarItem != null & item.count < maxStackSize)
        {
            InventoryItem similarItem = slotWithSimilarItem.GetComponentInChildren<InventoryItem>();
            similarItem.count += item.count;
            Destroy(item.gameObject);
            similarItem.RefreshCount();
            return true;
        }
        else { return false; }    
    }

    GameObject FindFreeSlot()
    {
        GameObject freeSlot = inventorySlots.Find(slot => slot.GetComponentInChildren<InventoryItem>() == null);
        return freeSlot;
    }

    void ChangeItemParent(InventoryItem item, GameObject slot)
    {
        ItemDragAndDropLogic script = item.GetComponent<ItemDragAndDropLogic>();
        script.deffParent = slot.transform;
    }

    void DeleteFreeSlot()
    {
        if (inventorySlots.Count > defAmountOfSlots)
        {
            GameObject freeSlot = FindFreeSlot();
            Destroy(freeSlot);
        }
    }
}
