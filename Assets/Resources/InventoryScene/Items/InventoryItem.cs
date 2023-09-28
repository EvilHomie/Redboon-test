using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public BasicItem basicItem;
    TextMeshProUGUI countText;
    TextMeshProUGUI priceText;

    public int count = 1;
    public int price;
    public Owner owner;

    public InventoryItem(BasicItem basicItem)
    {
        this.basicItem = basicItem;
    }

    void Awake()
    {        
        countText = transform.Find("Count Text").GetComponent<TextMeshProUGUI>();
        priceText = transform.Find("Price Text").GetComponent <TextMeshProUGUI>();
        owner = Owner.Nobody;
    }
    private void Start()
    {
        MerchantInventory.OnOpenMerchantMenu += ShowPrice;
        InitialiseItem(basicItem);
        RefreshPrice();
        RefreshCount();        
    }
    private void OnDestroy()
    {
        MerchantInventory.OnOpenMerchantMenu -= ShowPrice;
    }

    private void ShowPrice(bool status)
    {
        priceText.gameObject.SetActive(status);
    }

    void InitialiseItem(BasicItem newItem)
    {
        GetComponent<Image>().sprite = newItem.image;
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool countTextActive = count > 1;
        countText.gameObject.SetActive(countTextActive);
    }

    public void RefreshPrice()
    {
        priceText.text = $"Price : {price} G";
    }

    public enum Owner
    {
        Nobody,
        Player,
        Merchant
    }
}


