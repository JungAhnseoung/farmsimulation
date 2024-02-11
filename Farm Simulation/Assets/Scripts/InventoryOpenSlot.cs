using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryOpenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private InventoryOpen inventoryOpen = null;
    [SerializeField] private GameObject itemTextObject = null;
    [SerializeField] public int slotIndex;

    public ItemInfo itemInfo;
    public int itemAmount;

    public GameObject itemDragged;
    private Canvas canvasGroup;

    public Image inventoryOpenImage;
    public TextMeshProUGUI text;

    private void Awake()
    {
        canvasGroup = GetComponentInParent<Canvas>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemAmount != 0)
        {
            inventoryOpen.itemTextObject = Instantiate(itemTextObject, transform.position, Quaternion.identity);
            inventoryOpen.itemTextObject.transform.SetParent(canvasGroup.transform, false);

            InventoryItemDescription inventoryItemDescription = inventoryOpen.itemTextObject.GetComponent<InventoryItemDescription>();
            inventoryItemDescription.SetDescriptionText(itemInfo.itemName, itemInfo.itemType.ToString(), itemInfo.itemDescription);
            
            if(slotIndex > 11)
            {
                inventoryOpen.itemTextObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryOpen.itemTextObject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else
            {
                inventoryOpen.itemTextObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryOpen.itemTextObject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryOpen.RemoveItemTextObject();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemAmount != 0)
        {
            itemDragged = Instantiate(inventoryOpen.itemDraggedObject, inventoryOpen.transform);
            Image itemDraggedImage = itemDragged.GetComponentInChildren<Image>();
            itemDraggedImage.sprite = inventoryOpenImage.sprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemDragged != null) itemDragged.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(itemDragged != null)
        {
            Destroy(itemDragged);
            //if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryOpenSlot>() != null)
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryOpenSlot>() != null) 
                {
                    InventoryManager.SwapItemInInventory(InventoryType.Player, slotIndex, eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryOpenSlot>().slotIndex);
                }
                else if (eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null)
                {
                    InventoryManager.SwapItemInInventory(InventoryType.Player, slotIndex, eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>().slotIndex);
                }
                inventoryOpen.RemoveItemTextObject();
            }
        }
    }
}
