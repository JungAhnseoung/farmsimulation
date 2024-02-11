using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileIndicator : MonoBehaviour
{
    [SerializeField] private Sprite allowedSprite = null;
    [SerializeField] private Sprite notAllowedSprite = null;
    [SerializeField] private RectTransform rectTransform = null;
    [SerializeField] private Image image = null;
    [SerializeField] private Player player;
    [SerializeField] private PlantList plantList = null;
    private Canvas canvas;
    private Camera camera;
    private Grid grid;
    public static Vector2 tileSize = Vector2.one;

    private ItemType selectedItem;
    public ItemType SelectedItem { get => selectedItem; set => selectedItem = value; }

    private bool indicatorEnabled = false;
    public bool IndicatorEnabled { get=> indicatorEnabled; set => indicatorEnabled = value; }

    private int radius = 0;
    public int Radius { get => radius; set => radius = value; }

    private bool indicatorAllowed = false;
    public bool IndicatorAllowed { get => indicatorAllowed; set => indicatorAllowed = value; }

    public void OnEnable()
    {
        EventHandler.AfterLoad += Load;
    }

    public void OnDisable()
    {
        EventHandler.AfterLoad -= Load;
    }

    private void Load()
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        camera = Camera.main;
    }

    void Update()
    {
        if(IndicatorEnabled)
        {
            ShowIndicator();
        }
    }

    private Vector3Int ShowIndicator()
    {
        if (grid != null)
        {
            Vector3Int indicatorLocation = GetLocationIndicator();
            Vector3Int playerLocation = GetLocationPlayer();


            indicatorAllowed = true;
            image.sprite = allowedSprite;

            if(Mathf.Abs(indicatorLocation.x - playerLocation.x) > radius || Mathf.Abs(indicatorLocation.y - playerLocation.y) > radius)
            {
                indicatorAllowed = false;
                image.sprite = notAllowedSprite;
            }
            else
            {
                ItemInfo selectedItemInfo = InventoryManager.GetSelectedItemInfo(InventoryType.Player);
                if (selectedItemInfo == null)
                {
                    indicatorAllowed = false;
                    image.sprite = notAllowedSprite;
                }
                else
                {
                    TileAttributeDetail tileAttributeDetail = TileManager.GetTileAttributeDetail(indicatorLocation.x, indicatorLocation.y);
                    if (tileAttributeDetail != null)
                    {
                        switch (selectedItemInfo.itemType)
                        {
                            case ItemType.Goods:
                                if (!tileAttributeDetail.isDroppable)
                                {
                                    indicatorAllowed = false;
                                    image.sprite = notAllowedSprite;
                                }
                                break;
                            case ItemType.Seed:
                                if (!tileAttributeDetail.isDroppable)
                                {
                                    indicatorAllowed = false;
                                    image.sprite = notAllowedSprite;
                                }
                                break;
                            case ItemType.Tool:
                                if(!IsTileTool(tileAttributeDetail, selectedItemInfo))
                                {
                                    indicatorAllowed = false;
                                    image.sprite = notAllowedSprite;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        indicatorAllowed = false;
                        image.sprite = notAllowedSprite;
                    }
                }
            }
            
            Vector3 worldLocation = grid.CellToWorld(indicatorLocation);
            Vector2 screenLocation = camera.WorldToScreenPoint(worldLocation);

           
            rectTransform.position = RectTransformUtility.PixelAdjustPoint(screenLocation, rectTransform, canvas);
            return indicatorLocation;
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    public Vector3Int GetLocationIndicator()
    {
        return grid.WorldToCell(camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z))); //grid position (indicator)
    }

    public Vector3Int GetLocationPlayer()
    {
        return grid.WorldToCell(player.transform.position); //grid position (player)
    }

    public void EnableIndicator()
    {
        image.color = new Color(1f, 1f, 1f, 0.5f);
        IndicatorEnabled = true;
    }

    public void DisableIndicator()
    {
        image.color = new Color(0f, 0f, 0f, 0f);
        indicatorEnabled = false;
    }

    private bool IsTileTool(TileAttributeDetail tileAttributeDetail, ItemInfo itemInfo)
    {
        switch(itemInfo.itemName)
        {
            case "Hoe":
                if(tileAttributeDetail.isDiggable && tileAttributeDetail.ageDig == -1)
                {
                    List<Item> items = new List<Item>();
                    Others.GetObjectInPositionBox<Item>(out items, new Vector3(TileWorldLocation().x + 0.5f, TileWorldLocation().y + 0.5f, 0f), tileSize, 0f);
                    bool isThere = false;
                    
                    foreach(Item item in items)
                    {
                       if(InventoryManager.GetItemInfo(item.ItemNo).itemType == ItemType.Farmable)
                        {
                            isThere = true;
                            break;
                        }
                    }

                    return isThere ? false : true;
                }
                else
                {
                    return false;
                }

            case "Watering Can":
                return (tileAttributeDetail.ageDig > -1 && tileAttributeDetail.ageWater == -1) ? true : false;

            case "Axe":
            case "Pickaxe":
            case "Basket":
                if(tileAttributeDetail.seedNo != -1) //if seed has been planted
                {
                    PlantInfo plantInfo = plantList.GetPlantInfo(tileAttributeDetail.seedNo);
                    if(plantInfo != null)
                    {
                        if (tileAttributeDetail.age >= plantInfo.age[plantInfo.age.Length - 1])
                        {
                            return (plantInfo.IsToolUsablePlant(itemInfo.itemNo)) ? true : false;
                        }
                        else return false;
                    }
                }
                return false;

            default:
                return false;
        }
    }

    public Vector3 TileWorldLocation()
    {
        return grid.CellToWorld(grid.WorldToCell(camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z))));
    }
}
