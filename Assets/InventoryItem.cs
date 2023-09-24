using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public BasicItem basicItem;
    TextMeshProUGUI countText;
    TextMeshProUGUI priceText;

    public int count = 1;

    void Start()
    {
        countText = transform.Find("Count Text").GetComponent<TextMeshProUGUI>();
        priceText = transform.Find("Price Text").GetComponent <TextMeshProUGUI>();
        InitialiseItem(basicItem);
        RefreshCount();
    }

    void InitialiseItem(BasicItem newItem)
    {
        GetComponent<Image>().sprite = newItem.image;
        priceText.text = $"Price : {newItem.startPrice} G";
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool countTextActive = count > 1;
        countText.gameObject.SetActive(countTextActive);
    }
}


