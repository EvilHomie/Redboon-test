using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;
    Transform deffParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = transform.root.GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        deffParent = transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        
        SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        transform.SetParent(deffParent);
    }

    void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void ChangeParent(Transform parent)
    {
        deffParent = parent;
    }
}
