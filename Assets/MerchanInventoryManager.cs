using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MerchanInventoryManager : MonoBehaviour, IDropHandler
{
    [SerializeField] Transform slotsConteiner;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] List<GameObject> inventorySlots = new();

    int defAmountSlots;
    int collumsAmout;

    private void Start()
    {
        FillListAvailableSlots();
    }
    // Заполнение листа со слотами и получение дефолтного количества слотов
    void FillListAvailableSlots()
    {
        foreach (Transform child in slotsConteiner.transform)
        {
            inventorySlots.Add(child.gameObject);
        }

        defAmountSlots = inventorySlots.Count;
        collumsAmout = slotsConteiner.GetComponent<GridLayoutGroup>().constraintCount;
    }

    // Событие при "Броске" объекта в границах объекта
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out DragAndDropItem item); // Проверка предмета является ли он предметом инвентаря

        if (item != null)
        {
            GameObject freeSlot = FindFreeSlot(); // Поиск свободного слота

            // Действия если найден свободный слот
            if (freeSlot != null) 
            {
                TransferItemInFreeSlot(item, freeSlot); // Перенос объекта в свободный слот
            }
            // Действия если свободный слот не найден
            else
            {
                AddNewSlots(); // Создание нового ряда слотов
                GameObject newFreeSlot = FindFreeSlot(); // Поиск свободного слота
                TransferItemInFreeSlot(item, newFreeSlot); // Перенос объекта в новый совбодный слот
            }
        }        
    }

    void AddNewSlots()
    {
        for (int i = 0; i < collumsAmout; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsConteiner.transform);
            inventorySlots.Add(newSlot);
        }        
    }

    GameObject FindFreeSlot()
    {
        GameObject freeSlot = inventorySlots.Find(slot => slot.GetComponentInChildren<DragAndDropItem>() == null);
        return freeSlot;
    }

    void TransferItemInFreeSlot(DragAndDropItem item, GameObject slot)
    {
        item.ChangeParent(slot.transform);
    }

    void DeleteFreeSlot()
    {
        if (inventorySlots.Count > defAmountSlots)
        {
            GameObject freeSlot = FindFreeSlot();
            Destroy(freeSlot);
        }
    }
}
