using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour, IDropHandler
{
    public SlotType slotType;
    public static Action<PointerEventData> OnSlotBusy;
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out ItemDragAndDropLogic item); // �������� ������� �������� �� �� ��������� ���������

        // ���� �������� ��������� ��������� � ���� ��������
        if (item != null & transform.childCount == 0) 
        {
            item.deffParent = transform; // �������� ������� � ����
        }
        // ���� �������� ��������� ��������� � ���� �����
        else if (item != null & transform.childCount != 0)
        { 
            OnSlotBusy.Invoke(eventData); // ������� ��� ��������� - ���������� �������� � ������ ��������� ���� 
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
