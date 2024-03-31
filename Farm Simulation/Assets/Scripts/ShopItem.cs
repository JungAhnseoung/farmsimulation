using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [ItemNoName]
    [SerializeField]
    private int itemNo;
    [SerializeField] private ShopUI shop = null;
    [SerializeField] private Image itemImage = null;
    [SerializeField] private TextMeshProUGUI itemName = null;
    [SerializeField] private TextMeshProUGUI itemPrice = null;
    [SerializeField] private GameObject itemDescription = null;
    
    private Canvas canvasGroup;
    public ItemInfo itemInfo;

    public int ItemNo
    {
        get { return itemNo; }
        set { itemNo = value; }
    }
    
    public void Initialize(int itemNo)
    {
        if (itemNo != 0)
        {
            this.itemNo = itemNo;
            itemInfo = InventoryManager.GetItemInfo(itemNo);
            itemImage.sprite = itemInfo.itemIcon;
            itemName.text = itemInfo.itemName;
            itemPrice.text = (itemInfo.sellPrice * 2).ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Player.coin >= itemInfo.sellPrice * 2)
        {
            Player.coin -= itemInfo.sellPrice * 2;
            InventoryManager.AddItemInInventory(InventoryType.Player, itemNo);
        }
        else
        {
            EventHandler.CallNotEnoughCoin();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shop.itemDescription = Instantiate(itemDescription, transform.position, Quaternion.identity);
        shop.itemDescription.transform.SetParent(canvasGroup.transform, false);
        shop.itemDescription.GetComponent<InventoryItemDescription>().SetDescriptionText(itemInfo.itemName, itemInfo.itemType.ToString(), itemInfo.itemDescription);
        shop.itemDescription.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
        shop.itemDescription.transform.position = new Vector3(transform.position.x - 150f, transform.position.y + 30f, transform.position.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shop.RemoveItemDescription();
    }

    private void Awake()
    {
        canvasGroup = GetComponentInParent<Canvas>();
    }
    void Start()
    {
        if (itemNo != 0) Initialize(itemNo);
    }

}
