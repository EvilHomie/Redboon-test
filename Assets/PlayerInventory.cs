using UnityEngine;

public class PlayerInventory : InventoryManager
{
    private void OnEnable()
    {
        SlotManager.OnSlotBusy += OnDrop;
    }
    private void OnDisable()
    {
        SlotManager.OnSlotBusy -= OnDrop;
    }
    protected override void AddNewSlots(int AmoutOfNewSlots)
    {
        for (int i = 0; i < AmoutOfNewSlots; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, slotsConteiner.transform);
            newSlot.AddComponent<SlotManager>().slotType = SlotManager.SlotType.Universal; 
            inventorySlots.Add(newSlot);
        }
    }
}
