using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Transform slotsConteiner;
    [SerializeField] GameObject inventoryItemPrefab;
    List<InventorySlot> inventorySlots = new();

    private void Start()
    {
        FillList();
    }
    void FillList()
    {
        foreach (Transform child in slotsConteiner.transform)
        {
            inventorySlots.Add(child.GetComponent<InventorySlot>());
        }
    }

    void AddItem(BasicItem item)
    {        
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.GetComponentInChildren<InventoryItem123>();
            if (slot == null)
            {
                SpawnNewItem(item, slot);
                Debug.Log("Slot Founded");
                return;                
            }
        }
    }

    void SpawnNewItem (BasicItem item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem123 inventoryItem = newItemGo.GetComponent<InventoryItem123>();
        inventoryItem.InitialiseItem(item);
    }
}
