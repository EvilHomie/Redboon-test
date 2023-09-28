using System;
using UnityEngine;

public class MerchantInventory : InventoryLogic
{
    public static Action<bool> OnOpenMerchantMenu;
    public static Action<int> OnItemSold;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] int preloadItemsAmount;

    BasicItem[] BasicItemsArray;

    private void OnEnable()
    {
        OnOpenMerchantMenu?.Invoke(true);
    }

    private void OnDisable()
    {
        OnOpenMerchantMenu?.Invoke(false);
    }

    protected override void Start()
    {
        base.Start();
        AddFewItems();
        gameObject.SetActive(false);
    }

    private void AddFewItems()
    {
        BasicItemsArray = Resources.LoadAll<BasicItem>("InventoryScene/Items/Basic Items");
        for (int i = 0; i < preloadItemsAmount; i++)
        {
            BasicItem randomBasicItem = BasicItemsArray[UnityEngine.Random.Range(0, BasicItemsArray.Length)];
            GameObject newInventoryItem = Instantiate(inventoryItemPrefab, FindFreeSlot().transform);
            InventoryItem newItemParameters = newInventoryItem.GetComponent<InventoryItem>();
            newItemParameters.basicItem = randomBasicItem;
            if (randomBasicItem.stackable) newItemParameters.count = UnityEngine.Random.Range(3, 10);
            newItemParameters.price = randomBasicItem.startPrice * changePriceMod;
            newItemParameters.owner = InventoryItem.Owner.Merchant;
        }
    }


    public override void TryPutItemInInventory(InventoryItem item)
    {
        if (item.owner != InventoryItem.Owner.Merchant)
        {
            base.TryPutItemInInventory(item);
            //OnItemSold?.Invoke(item.price * translateItemCount);
            //Debug.Log($"SoldEntirely   {translateItemCount}   {item.price}    {item.price * item.count}");
            //item.owner = InventoryItem.Owner.Merchant;
            //SetPrice(item);
        }
    }
    public override void PutItemInSlot(InventoryItem item, GameObject slot)
    {
        base.PutItemInSlot(item, slot);
        OnItemSold?.Invoke(item.price * item.count);
        Debug.Log($"SoldEntirely   {item.count}   {item.price}    {item.price * item.count}");
        item.owner = InventoryItem.Owner.Merchant;
        item.price = item.basicItem.startPrice * changePriceMod;
        item.RefreshPrice();
    }

    public override void MergingItems(InventoryItem suitableItem, InventoryItem item)
    {
        base.MergingItems(suitableItem, item);
        OnItemSold?.Invoke(item.price * mergeCount);
        Debug.Log($"SoldPartially    {mergeCount}   {item.price}    {item.price * mergeCount}");
    }


    //public override void PutItemInSlot(InventoryItem item, GameObject slot)
    //{
    //    if (item.owner != InventoryItem.Owner.Merchant)
    //    {
    //        base.PutItemInSlot(item, slot);
    //        OnItemSold?.Invoke(item.price * item.count);
    //        Debug.Log($"SoldEntirely   {item.count}   {item.price}    {item.price * item.count}");            
    //        item.owner = InventoryItem.Owner.Merchant;
    //        SetPrice(item);
    //    }
    //}

    //public override void MergingItems(InventoryItem suitableItem, InventoryItem item)
    //{        
    //    if (item.owner != InventoryItem.Owner.Merchant)
    //    {
    //        base.MergingItems(suitableItem, item);
    //        OnItemSold?.Invoke(item.price * mergeCount);
    //        Debug.Log($"SoldPartially    {mergeCount}   {item.price}    {item.price * mergeCount}");
    //    }
    //}
       
}
