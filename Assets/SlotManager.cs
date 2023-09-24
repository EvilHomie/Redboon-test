using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour, IDropHandler
{
    public SlotType slotType;
    public static Action<PointerEventData> OnSlotBusy;
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out ItemDragAndDropLogic item); // Проверка объекта является ли он предметом инвентаря

        // Если является предметом инвентаря и слот свободет
        if (item != null & transform.childCount == 0) 
        {
            item.deffParent = transform; // Помещает предмет в слот
        }
        // Если является предметом инвентаря и слот занят
        else if (item != null & transform.childCount != 0)
        { 
            OnSlotBusy.Invoke(eventData); // Событие для инвентаря - попытаться положить в другой свободный слот 
        }
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
