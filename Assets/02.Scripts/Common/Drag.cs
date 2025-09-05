using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject draggingItem = null;
    private Transform itemTr;
    private CanvasGroup canvasGroup;
    [SerializeField] private Transform inventoryTr;
    [SerializeField] private Transform ItemListTr;
    void Start()
    {
        itemTr = GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingItem = this.gameObject;
        this.transform.SetParent(inventoryTr);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;

        if (this.transform.parent == inventoryTr)
        {
            this.transform.SetParent(ItemListTr);
            GameManager.instance.RemoveItem(GetComponent<ItemInfo>().itemData);
        }
    }

    

}
