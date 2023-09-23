using UnityEngine;
using UnityEngine.UI;

public class InventoryItem123 : MonoBehaviour
{
    [SerializeField] BasicItem item;

    private void Start()
    {
        InitialiseItem(item);
    }

    public void InitialiseItem(BasicItem newItem)
    {
        GetComponent<Image>().sprite = newItem.image;
    }
}


