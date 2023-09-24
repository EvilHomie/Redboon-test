using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager123 : MonoBehaviour
{
    [SerializeField] Transform slotsConteiner;
    [SerializeField] GameObject inventoryItemPrefab;
    List<SlotManager> inventorySlots = new();

    private void Start()
    {
        FillList();
    }
    void FillList()
    {
        foreach (Transform child in slotsConteiner.transform)
        {
            inventorySlots.Add(child.GetComponent<SlotManager>());
        }
    }

    void AddItem(BasicItem item)
    {        
        foreach (SlotManager slot in inventorySlots)
        {
            slot.GetComponentInChildren<InventoryItem>();
            if (slot == null)
            {
                SpawnNewItem(item, slot);
                Debug.Log("Slot Founded");
                return;                
            }
        }
    }

    void SpawnNewItem (BasicItem item, SlotManager slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        //inventoryItem.InitialiseItem(item);
    }
}
