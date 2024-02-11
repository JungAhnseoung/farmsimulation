using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Sprite allowedSprite = null;
    [SerializeField] private Sprite notAllowedSprite = null;
    [SerializeField] private RectTransform rectTransform = null;
    [SerializeField] private Image image = null;
    [SerializeField] private TileIndicator tileIndicator = null;
    [SerializeField] private Player player;
    private Canvas canvas;
    private Camera camera;

    private ItemType selectedItem;
    public ItemType SelectedItem { get => selectedItem; set => selectedItem = value; }

    private bool indicatorEnabled = false;
    public bool IndicatorEnabled { get => indicatorEnabled; set => indicatorEnabled = value; }

    private float radius = 0;
    public float Radius { get => radius; set => radius = value; }

    private bool indicatorAllowed = false;
    public bool IndicatorAllowed { get => indicatorAllowed; set => indicatorAllowed = value; }

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        camera = Camera.main;
    }

    private void Update()
    {
        if (IndicatorEnabled)
        {
            ShowIndicator();
        }
    }

    private void ShowIndicator()
    {
        Vector3 indicatorLocation = GetLocationIndicator();
        Vector3 playerLocation = player.GetCenter();

        indicatorAllowed = true;
        image.sprite = allowedSprite;
        image.color = new Color(1f, 1f, 1f, 1f);
        tileIndicator.DisableIndicator();

        if (indicatorLocation.x >= (playerLocation.x + radius / 3f) && indicatorLocation.y >= (playerLocation.y + radius / 3f) ||
            indicatorLocation.x <= (playerLocation.x - radius / 3f) && indicatorLocation.y >= (playerLocation.y + radius / 3f) ||
            indicatorLocation.x <= (playerLocation.x - radius / 3f) && indicatorLocation.y <= (playerLocation.y - radius / 3f) ||
            indicatorLocation.x >= (playerLocation.x + radius / 3f) && indicatorLocation.y <= (playerLocation.y - radius / 3f))
        {
            indicatorAllowed = false;
            image.color = new Color(0f, 0f, 0f, 0f);
            tileIndicator.EnableIndicator();
        }
        else
        {
            if(Mathf.Abs(indicatorLocation.x - playerLocation.x) >= radius || Mathf.Abs(indicatorLocation.y - playerLocation.y) >= radius)
            {
                indicatorAllowed = false;
                image.color = new Color(0f, 0f, 0f, 0f);
                tileIndicator.EnableIndicator();
            }
            else
            {
                ItemInfo selectedItemInfo = InventoryManager.GetSelectedItemInfo(InventoryType.Player);
                if(selectedItemInfo == null)
                {
                    indicatorAllowed = false;
                    image.color = new Color(0f, 0f, 0f, 0f); 
                    tileIndicator.EnableIndicator();
                }
                else
                {
                    switch(selectedItemInfo.itemType)
                    {
                        case ItemType.Tool:
                            if(!IsUsableTool(indicatorLocation, playerLocation, selectedItemInfo))
                            {
                                indicatorAllowed = false;
                                image.color = new Color(0f, 0f, 0f, 0f); 
                                tileIndicator.EnableIndicator();
                            }
                            break;
                        default:
                            //indicatorAllowed = false;
                            //image.color = new Color(0f, 0f, 0f, 0f);
                            //tileIndicator.EnableIndicator();
                            break;
                    }
                }
            }
        }

        Vector2 screenLocation = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        rectTransform.position = RectTransformUtility.PixelAdjustPoint(screenLocation, rectTransform, canvas);
    }

    public Vector3 GetLocationIndicator()
    {
        return camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
    }

    private bool IsUsableTool(Vector3 indicatorLocation, Vector3 playerLocation, ItemInfo itemInfo)
    {
        switch(itemInfo.itemName)
        {
            case "Scythe":
                return IsUsableScythe(indicatorLocation, playerLocation, itemInfo);
            default:
                return false;
        }
    }

    private bool IsUsableScythe(Vector3 indicatorLocation, Vector3 playerLocation, ItemInfo itemInfo)
    {
        List<Item> items = new List<Item>();

        if(Others.GetObjectInPosition<Item>(out items, indicatorLocation))
        {
            if(items.Count != 0)
            {
                foreach(Item item in items)
                {
                    if (InventoryManager.GetItemInfo(item.ItemNo).itemType == ItemType.Farmable) return true;
                }
            }
        }

        return false;
    }

    public void DisableIndicator()
    {
        image.color = new Color(1f, 1f, 1f, 0f);
        IndicatorEnabled = false;
    }

    public void EnableIndicator()
    {
        image.color = new Color(1f, 1f, 1f, 1f);
        IndicatorEnabled = true;
    }
}
