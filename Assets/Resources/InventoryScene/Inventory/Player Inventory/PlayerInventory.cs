using System;
using UnityEngine;

public class PlayerInventory : InventoryLogic
{
    public static Func<int, bool> OnItemBuy;

    protected override void AddNewSlots(int AmoutOfNewSlots)
    {
        for (int i = 0; i < AmoutOfNewSlots; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, slotsConteiner.transform);
            newSlot.AddComponent<SlotManager>().slotType = SlotManager.SlotType.Universal;
            inventorySlots.Add(newSlot);
        }
    }


    public override void TryPutItemInInventory(InventoryItem item)
    {
        if (item.owner != InventoryItem.Owner.Merchant)
            base.TryPutItemInInventory(item);
        else 
        {
            if (PlayerGoldManager.playerGold >= item.price * item.count)
            {
                Debug.Log($"BuyEntirely  {item.count}    {item.price}    {item.price * item.count}");
                base.TryPutItemInInventory(item);
            }
            else Debug.Log("NOT ENOUGH MONEY!");
        }
    }

    public override void PutItemInSlot(InventoryItem item, GameObject slot)
    {
        if (item.owner != InventoryItem.Owner.Merchant)
        {
            base.PutItemInSlot(item, slot);
            SetOwnerPlayer(item);
            Debug.Log("Just Put");
        }
        else
        {
            if (OnItemBuy?.Invoke(item.price * item.count) == true)
            {
                Debug.Log($"BuyEntirely  {item.count}    {item.price}    {item.price * item.count}");
                base.PutItemInSlot(item, slot);
                SetOwnerPlayer(item);
            }
            else Debug.Log("NOT ENOUGH MONEY!");
        }
    }
    public override void MergingItems(InventoryItem suitableItem, InventoryItem item)
    {
        if (item.owner != InventoryItem.Owner.Merchant)
        {
            base.MergingItems(suitableItem, item);
            SetOwnerPlayer(item);
            Debug.Log("Just Put");
        }

        else
        {
            mergeCount = maxStackSize - suitableItem.count;
            if (mergeCount >= item.count)
            {
                mergeCount = item.count;
            }
            if (OnItemBuy?.Invoke(item.price * mergeCount) == true)
            {
                base.MergingItems(suitableItem, item);
                Debug.Log($"BuyPartially  {mergeCount}    {item.price}    {item.price * mergeCount}");

            }
            else Debug.Log("NOT ENOUGH MONEY!");
        }
    }

    void SetOwnerPlayer(InventoryItem item)
    {
        item.owner = InventoryItem.Owner.Player;
        item.price = item.basicItem.startPrice / changePriceMod;
        item.RefreshPrice();
    }

    //void EventOnBuy(InventoryItem item, GameObject slot, int itemCount)
    //{
    //    if (OnItemBuy?.Invoke(item.price * itemCount) == true)
    //    {
    //        Debug.Log($"BuyEntirely  {item.count}    {item.price}    {item.price * itemCount}");
    //        base.PutItemInSlot(item, slot);
    //        item.owner = InventoryItem.Owner.Player;
    //        item.price = item.basicItem.startPrice / changePriceMod;
    //        item.RefreshPrice();
    //    }
    //    else Debug.Log("NOT ENOUGH MONEY!");
    //}


    //public override void PutItemInSlot(InventoryItem item, GameObject slot)
    //{
    //    if (item.owner != InventoryItem.Owner.Merchant)
    //    {
    //        base.PutItemInSlot(item, slot);
    //        item.owner = InventoryItem.Owner.Player;
    //        item.price = item.basicItem.startPrice / changePriceMod;
    //        item.RefreshPrice();
    //    }
    //    else if (item.owner == InventoryItem.Owner.Merchant)
    //    {
    //        if (OnItemBuy?.Invoke(item.price * item.count) == true)
    //        {
    //            Debug.Log($"BuyEntirely  {item.count}    {item.price}    {item.price * item.count}");
    //            base.PutItemInSlot(item, slot);
    //            item.owner = InventoryItem.Owner.Player;
    //            item.price = item.basicItem.startPrice / changePriceMod;
    //            item.RefreshPrice();
    //        }
    //        else Debug.Log("NOT ENOUGH MONEY!");
    //    }
    //}
    //public override void MergingItems(InventoryItem suitableItem, InventoryItem item)
    //{
    //    if (item.owner != InventoryItem.Owner.Merchant)
    //    {
    //        base.MergingItems(suitableItem, item);
    //        item.owner = InventoryItem.Owner.Player;
    //        item.price = item.basicItem.startPrice / changePriceMod;
    //        item.RefreshPrice();
    //    }

    //    else if (item.owner == InventoryItem.Owner.Merchant)
    //    {
    //        mergeCount = maxStackSize - suitableItem.count;
    //        if (mergeCount >= item.count)
    //        {
    //            mergeCount = item.count;
    //        }
    //        Debug.Log(mergeCount);
    //        if (OnItemBuy?.Invoke(item.price * mergeCount) == true)
    //        {
    //            base.MergingItems(suitableItem, item);
    //            Debug.Log($"BuyPartially  {mergeCount}    {item.price}    {item.price * mergeCount}");

    //        }
    //        else Debug.Log("NOT ENOUGH MONEY!");
    //    }

    //}

    //void EventOnBuy(InventoryItem item, GameObject slot, int itemCount)
    //{
    //    if (OnItemBuy?.Invoke(item.price * itemCount) == true)
    //    {
    //        Debug.Log($"BuyEntirely  {item.count}    {item.price}    {item.price * itemCount}");
    //        base.PutItemInSlot(item, slot);
    //        item.owner = InventoryItem.Owner.Player;
    //        item.price = item.basicItem.startPrice / changePriceMod;
    //        item.RefreshPrice();
    //    }
    //    else Debug.Log("NOT ENOUGH MONEY!");
    //}
}
