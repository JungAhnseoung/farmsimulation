using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public TextMeshProUGUI TextMeshProUGUI;
    public Image inventorySlotSelected;
    public Image inventorySlotIcon;

    private Camera camera;
    private Transform itemGroup;
    public GameObject itemDragged;

    private Canvas canvasGroup;
    public ItemInfo itemInfo;
    public int itemCount;
    private TileIndicator tileIndicator;
    private Indicator indicator;

    [SerializeField] private InventoryBar inventoryBar = null;
    [SerializeField] private InventoryOpen inventoryOpen = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] public int slotIndex;
    [SerializeField] private GameObject inventoryItemDescription = null;

    [SerializeField] private GameObject eatCanvas = null;

    public bool isItemSelected;

    private void OnEnable()
    {
        EventHandler.AfterLoad += Loaded;
        EventHandler.DropEvent += DropItem;
        EventHandler.RemoveEvent += RemoveItem;
    }
    private void OnDisable()
    {
        EventHandler.AfterLoad -= Loaded;
        EventHandler.DropEvent -= DropItem;
        EventHandler.RemoveEvent -= RemoveItem;
    }

    private void Start()
    {
        camera = Camera.main;
        canvasGroup = GetComponentInParent<Canvas>();
        tileIndicator = FindObjectOfType<TileIndicator>();
        indicator = FindObjectOfType<Indicator>();
    }

    public void Loaded()
    {
        itemGroup = GameObject.FindGameObjectWithTag("ItemsGroup").transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemCount != 0)
        {
            inventoryBar.inventoryItemDescription = Instantiate(inventoryItemDescription, transform.position, Quaternion.identity);
            inventoryBar.inventoryItemDescription.transform.SetParent(canvasGroup.transform, false);
            inventoryBar.inventoryItemDescription.GetComponent<InventoryItemDescription>().SetDescriptionText(itemInfo.itemName, itemInfo.itemType.ToString(), itemInfo.itemDescription);
            inventoryBar.inventoryItemDescription.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
            inventoryBar.inventoryItemDescription.transform.position = new Vector3(transform.position.x, transform.position.y + 15f, transform.position.z);
            
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isItemSelected == true)
            {
                if(itemInfo.isEdible)
                {
                    eatCanvas.SetActive(true);
                    EatUI.eatText.text = ("Do you want to eat " + itemInfo.itemName + "?");
                    InventoryManager.RemoveItemFromInventory(InventoryType.Player, itemInfo.itemNo);
                }
                RemoveSelectedItem();
            }
            else
            {
                if (itemCount > 0)
                {
                    inventoryBar.RemoveSelectedSlot();
                    inventoryOpen.RemoveSelectedSlot();
                    isItemSelected = true;
                    inventoryBar.SetSelectedSlot();

                    tileIndicator.Radius = itemInfo.distance;
                    indicator.Radius = 1f;

                    if (itemInfo.distance > 0f) tileIndicator.EnableIndicator();
                    else tileIndicator.DisableIndicator();
                    if (itemInfo.nontiledistance > 0f) indicator.EnableIndicator();
                    else indicator.DisableIndicator();
                    tileIndicator.SelectedItem = itemInfo.itemType;
                    indicator.SelectedItem = itemInfo.itemType;
                    InventoryManager.SetSelectedItem(InventoryType.Player, itemInfo.itemNo);

                    if (itemInfo.isCarriable)
                    {
                        Player.HoldingItem(itemInfo.itemNo);
                    }
                    else
                    {
                        Player.NotHoldingItem();
                    }
                }
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemInfo != null && itemInfo.itemType != ItemType.Tool && Chest.ChestOpen)
            {
                int itemNo = itemInfo.itemNo;

                InventoryManager.RemoveItemFromInventory(InventoryType.Player, itemNo);
                if (InventoryManager.SearchForItem(InventoryType.Player, itemNo) == -1) RemoveSelectedItem();
                InventoryManager.AddItemInInventory(InventoryType.Chest, itemNo);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryBar.inventoryItemDescription != null) Destroy(inventoryBar.inventoryItemDescription);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemInfo != null)
        {
            Player.DisableInput();
            itemDragged = Instantiate(inventoryBar.itemDragged, inventoryBar.transform);
            itemDragged.transform.SetParent(canvasGroup.transform, false);
            Image imageDragged = itemDragged.GetComponentInChildren<Image>();
            imageDragged.sprite = inventorySlotIcon.sprite;
            inventoryBar.RemoveSelectedSlot();
            isItemSelected = true;
            inventoryBar.SetSelectedSlot();

            tileIndicator.Radius = itemInfo.distance;
            indicator.Radius = 1f;
            
            if (itemInfo.distance > 0f) tileIndicator.EnableIndicator();
            else tileIndicator.DisableIndicator();
            
            if(itemInfo.nontiledistance > 0f) indicator.EnableIndicator();
            else indicator.DisableIndicator();
            tileIndicator.SelectedItem = itemInfo.itemType;
            indicator.SelectedItem = itemInfo.itemType;
            InventoryManager.SetSelectedItem(InventoryType.Player, itemInfo.itemNo);
            

            if (itemInfo.isCarriable)
            {
                Player.HoldingItem(itemInfo.itemNo);
            }
            else
            {
                Player.NotHoldingItem();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemDragged != null)
        {
            itemDragged.transform.position = Input.mousePosition;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemDragged != null)
        {
            Destroy(itemDragged);

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null)
                {
                    InventoryManager.SwapItemInInventory(InventoryType.Player, slotIndex, eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>().slotIndex);
                }
                else if(eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryOpenSlot>() != null)
                { 
                    InventoryManager.SwapItemInInventory(InventoryType.Player, slotIndex, eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryOpenSlot>().slotIndex);
                }
                if (inventoryBar.inventoryItemDescription != null) Destroy(inventoryBar.inventoryItemDescription);
                RemoveSelectedItem();
            }
            else
            {
                if (itemInfo.isDroppable)
                {
                    DropItem();
                }
                //tileIndicator.DisableIndicator();
            }

            Player.InputDisabled = false;
        }
    }

    private void DropItem()
    {
        if (itemInfo != null && isItemSelected)
        {        
            if (tileIndicator.IndicatorAllowed)
            {
                Vector3 point = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z));

                GameObject itemGameObject = Instantiate(itemPrefab, new Vector3(point.x, point.y, point.z), Quaternion.identity, itemGroup);
                Item item = itemGameObject.GetComponent<Item>();
                item.ItemNo = itemInfo.itemNo;

                if (inventoryBar.inventoryItemDescription != null) Destroy(inventoryBar.inventoryItemDescription);
                InventoryManager.RemoveItemFromInventory(InventoryType.Player, item.ItemNo);

                if (InventoryManager.SearchForItem(InventoryType.Player, item.ItemNo) == -1)
                {
                    RemoveSelectedItem();
                }
            }
        }
    }

    private void RemoveItem()
    {
        if(itemInfo != null && isItemSelected)
        {
            int itemNo = itemInfo.itemNo;
            
            InventoryManager.RemoveItemFromInventory(InventoryType.Player, itemNo);
            if (InventoryManager.SearchForItem(InventoryType.Player, itemNo) == -1) RemoveSelectedItem();
        }
    }

    public void RemoveSelectedItem()
    {
        tileIndicator.DisableIndicator();
        indicator.DisableIndicator();
        tileIndicator.SelectedItem = ItemType.None;
        indicator.SelectedItem = ItemType.None;

        inventoryBar.RemoveSelectedSlot();
        isItemSelected = false;
        InventoryManager.RemoveSelectedItem(InventoryType.Player);
        Player.NotHoldingItem();
    }
}

