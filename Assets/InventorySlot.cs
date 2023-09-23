using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //if (transform.childCount == 0)
        //{
        //    ItemDragAndDropLogic item = eventData.pointerDrag.GetComponent<ItemDragAndDropLogic>();
        //    item.deffParent = transform;
        //}
    }
}
