using TMPro;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    public static int playerGold = 1000;
    TextMeshProUGUI playerGoldText;

    private void Awake()
    {
        playerGoldText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        RefreshGoldText();
    }
    private void OnEnable()
    {
        MerchantInventory.OnItemSold += ChangePlayerGoldOnSold;
        PlayerInventory.OnItemBuy += ChangePlayerGoldOnBuy;
    }

    private void OnDisable()
    {
        MerchantInventory.OnItemSold -= ChangePlayerGoldOnSold;
        PlayerInventory.OnItemBuy -= ChangePlayerGoldOnBuy;
    }

    private bool ChangePlayerGoldOnBuy(int itemPrice)
    {
        if (playerGold - itemPrice >= 0)
        {
            playerGold -= itemPrice;
            RefreshGoldText();
            Debug.Log("Item Purchased");
            return true;
            
        }
        else return false;
    }

    private void ChangePlayerGoldOnSold(int itemPrice)
    {
        playerGold += itemPrice;
        RefreshGoldText();
        Debug.Log("Item Sold");
    }

    void RefreshGoldText()
    {
        playerGoldText.text = playerGold.ToString();   
    }
}
