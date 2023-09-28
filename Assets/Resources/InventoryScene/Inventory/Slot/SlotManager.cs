using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour, IDropHandler
{
    [SerializeField] PlayerInventory playerInventory;

    public SlotType slotType;
    int maxSlotSize;

    private void Awake()
    {        
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        maxSlotSize = playerInventory.maxStackSize;
    }
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out InventoryItem item); // Проверка объекта является ли он предметом инвентаря

        if (item == null) return; // Остановка метода если предмет не является предметом инвентаря

        if (slotType == SlotType.Universal)
        {
            if (transform.childCount == 0) // Проверка не занят ли слот
            {
                playerInventory.PutItemInSlot(item, transform.gameObject); // Помещает предмет в слот
            }
            else if (transform.childCount != 0 & item.basicItem.stackable)
            {
                TryMegreItems(item); 
            }
            else playerInventory.TryPutItemInInventory(item);
        }
        else
        {
            if (transform.childCount == 0 && slotType.ToString() == item.basicItem.type.ToString())
            {
                playerInventory.PutItemInSlot(item, transform.gameObject);
            }
            else if (transform.childCount == 0 && slotType.ToString() != item.basicItem.type.ToString())
            {
                playerInventory.TryPutItemInInventory(item);
            }
            else if (transform.childCount != 0 && slotType.ToString() == item.basicItem.type.ToString())
            {
                TryMegreItems(item);
            }
        }
    }

    void TryMegreItems(InventoryItem item)
    {
        InventoryItem itemInslot = GetComponentInChildren<InventoryItem>();
        if (itemInslot.basicItem.type == item.basicItem.type && itemInslot.count < maxSlotSize)
        {
            playerInventory.MergingItems(itemInslot, item);
            if(item.count != 0) playerInventory.TryPutItemInInventory(item);
        }
        else playerInventory.TryPutItemInInventory(item);
    }    

    public enum SlotType
    {
        Universal,
        Helmet,
        Chest,
        Legs,
        Boots,
        LeftArm,
        RightArm,
        Projectile
    }
}
