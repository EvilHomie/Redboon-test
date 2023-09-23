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
    // ���������� ����� �� ������� � ��������� ���������� ���������� ������
    void FillListAvailableSlots()
    {
        foreach (Transform child in slotsConteiner.transform)
        {
            inventorySlots.Add(child.gameObject);
        }

        defAmountSlots = inventorySlots.Count;
        collumsAmout = slotsConteiner.GetComponent<GridLayoutGroup>().constraintCount;
    }

    // ������� ��� "������" ������� � �������� �������
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out DragAndDropItem item); // �������� �������� �������� �� �� ��������� ���������

        if (item != null)
        {
            GameObject freeSlot = FindFreeSlot(); // ����� ���������� �����

            // �������� ���� ������ ��������� ����
            if (freeSlot != null) 
            {
                TransferItemInFreeSlot(item, freeSlot); // ������� ������� � ��������� ����
            }
            // �������� ���� ��������� ���� �� ������
            else
            {
                AddNewSlots(); // �������� ������ ���� ������
                GameObject newFreeSlot = FindFreeSlot(); // ����� ���������� �����
                TransferItemInFreeSlot(item, newFreeSlot); // ������� ������� � ����� ��������� ����
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
