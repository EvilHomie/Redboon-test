using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.TryGetComponent(out InventoryItem item); // Проверка объекта является ли он предметом инвентаря

        if (item == null)
        {
            return;
        }
        else if (item.owner != InventoryItem.Owner.Merchant)
        {
            Destroy(item.gameObject);
        } 

    }
}
