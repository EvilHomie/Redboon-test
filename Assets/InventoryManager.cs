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

    // ������� ��� "������" ������� � �������� �������
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out InventoryItem item); // �������� �������� �������� �� �� ��������� ���������

        if (item != null)
        {     
            if (item.basicItem.stackable && FindSimilarItemInSlots(item))
            {                
                return;
            }
            
            GameObject freeSlot = FindFreeSlot(); // ����� ���������� �����

            // �������� ���� ������ ��������� ����
            if (freeSlot != null)
            {
                ChangeItemParent(item, freeSlot); // ������� ������� � ��������� ����
            }
            // �������� ���� ��������� ���� �� ������
            else if (freeSlot == null)
            {
                if (inventorySlots.Count < maxAmountOfSlots) // �������� �� ���������� �� ������������ ���-�� ������
                {
                    AddNewSlots(collumsAmout); // �������� ������ ���� ������
                    GameObject newFreeSlot = FindFreeSlot(); // ����� ���������� �����
                    ChangeItemParent(item, newFreeSlot); // ������� ������� � ����� ��������� ����
                }
                else
                {
                    Debug.Log("Max Slots Count Reached"); // ������� ���� ���������� ������������ ���-�� ������
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
