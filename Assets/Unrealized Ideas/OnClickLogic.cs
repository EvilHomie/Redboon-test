using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickLogic : MonoBehaviour
{
    PlayerInventory playerInventory;
    MerchantInventory merchantInventory;
    InventoryItem thisItem;
    int countOnClick = 1;
    private void Awake()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        merchantInventory = FindFirstObjectByType<MerchantInventory>();
        thisItem = GetComponent<InventoryItem>();
    }


    void OnClickEvent()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (thisItem.owner == InventoryItem.Owner.Player)
            {
                InventoryItem suitableItem = merchantInventory.FindSimilarItemAndNotFullStackInSlots(thisItem);
                if (suitableItem != null)
                {
                    thisItem.count -= countOnClick;
                    suitableItem.count += countOnClick;
                    thisItem.RefreshCount();
                    suitableItem.RefreshCount();
                }
                else
                {
                    GameObject newFreeSlot = merchantInventory.FindFreeSlot();
                    if (newFreeSlot != null) 
                    {
                        InventoryItem newItem = Instantiate(thisItem, transform.parent);
                        newItem.transform.position = Vector3.zero;
                        merchantInventory.TryPutItemInInventory(newItem);
                    }
                }
                
            }
            else if (thisItem.owner == InventoryItem.Owner.Merchant)
            {
                //playerInventory.TryPutItemInInventory(thisItem);
            }
        }
    }
}
