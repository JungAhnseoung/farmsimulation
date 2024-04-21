using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public enum Direction
{
    up,
    down,
    left,
    right,
    none
}

public class Player : MonoBehaviour, Save
{
    private static float xInput;
    private static float yInput;
    private static bool isIdle;
    private static bool isWalking;
    private static bool isRunning;
    private static bool isHolding;
    private static bool idleRight;
    private static bool idleLeft;
    private static bool idleUp;
    private static bool idleDown;
    private static bool isToolRight;
    private static bool isToolLeft;
    private static bool isToolUp;
    private static bool isToolDown;
    private static bool isPullRight;
    private static bool isPullLeft;
    private static bool isPullUp;
    private static bool isPullDown;
    private static bool isWaterRight;
    private static bool isWaterLeft;
    private static bool isWaterUp;
    private static bool isWaterDown;
    private static bool isReapRight;
    private static bool isReapLeft;
    private static bool isReapUp;
    private static bool isReapDown;

    private Rigidbody2D rigidBody2D;
    private float movementSpeed;
    private Direction direction;
    public const float walkingSpeed = 5.0f;
    public const float runningSpeed = 10.0f;
    public static float yBasis = 0.875f;

    private static bool disableInput = false;
    public static bool InputDisabled { get => disableInput; set => disableInput = value; }

    private SaveObject saveObject;
    public SaveObject SaveObject { get { return saveObject; } set { saveObject = value; } }

    private string saveID;
    public string SaveID {  get { return saveID; } set { saveID = value; } }

    public static bool disableTool = false;

    private static AnimationOverride animationOverride;
    private static List<CharacterDetailAttribute> characterDetailAttributes;
    
    [SerializeField] private SpriteRenderer holdingItemSpriteRenderer = null;

    private static SpriteRenderer sp;
    public static GameObject go;
    public static Vector3 position;
    [SerializeField] private Vector3 respawnPosition = new Vector3();

    private static CharacterDetailAttribute armsAttribute;
    private static CharacterDetailAttribute toolAttribute;

    private TileIndicator tileIndicator;
    private Indicator indicator;


    public static int coin = 0;
    public static int currentStamina = 100;
    private int decreaseStamina = 5;
    private bool hasEnoughStamina = true;

    private void OnEnable()
    {
        Register();
    }
    private void OnDisable()
    {
        Unregister();
    }

    private void Awake()
    {
        SaveID = GetComponent<GUIDGenerator>().GUID;
        SaveObject = new SaveObject();
    }

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animationOverride = GetComponentInChildren<AnimationOverride>();
        armsAttribute = new CharacterDetailAttribute(CharacterAnimationAttribute.Arm, TypeAnimationAttribute.None);
        toolAttribute = new CharacterDetailAttribute(CharacterAnimationAttribute.Tool, TypeAnimationAttribute.Hoe);
        characterDetailAttributes = new List<CharacterDetailAttribute>();
        
        sp = holdingItemSpriteRenderer;
        go = this.gameObject;
        tileIndicator = FindObjectOfType<TileIndicator>();
        indicator = FindObjectOfType<Indicator>();
    }

    private void Update()
    {
        
        if(!InputDisabled)
        {
            isToolRight = false;
            isToolLeft = false;
            isToolUp = false;
            isToolDown = false;
            isPullRight = false;
            isPullLeft = false;
            isPullUp = false;
            isPullDown = false;
            isWaterRight = false;
            isWaterLeft = false;
            isWaterUp = false;
            isWaterDown = false;
            isReapRight = false;
            isReapLeft = false;
            isReapUp = false;
            isReapDown = false;
       
            MovementInput();
            //transform.Translate(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime, 0);
            rigidBody2D.MovePosition(rigidBody2D.position + new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime));

            TestInput();
            ClickInput();

            EventHandler.CallActionEvent(xInput, yInput, isIdle, isWalking, isRunning, isHolding,
                idleRight, idleLeft, idleUp, idleDown,
                isToolRight, isToolLeft, isToolUp, isToolDown,
                isPullRight, isPullLeft, isPullUp, isPullDown,
                isWaterRight, isWaterLeft, isWaterUp, isWaterDown,
                isReapRight, isReapLeft, isReapUp, isReapDown);

        }
        PassOut();
      }


    private void ClickInput()
    {
        if(!disableTool)
        {
            if(Input.GetMouseButton(0))
            {
                if(tileIndicator.IndicatorEnabled || indicator.IndicatorEnabled)
                {
                    Vector3Int indicatorLocation = tileIndicator.GetLocationIndicator();
                    Vector3Int playerLocation = tileIndicator.GetLocationPlayer();
                    xInput = 0f;
                    yInput = 0f;
                    isWalking = false;
                    isRunning = false;
                    isIdle = true;

                    Vector3Int clickDirection = ClickDirection(indicatorLocation, playerLocation);
                    TileAttributeDetail tileAttributeDetail = TileManager.GetTileAttributeDetail(indicatorLocation.x, indicatorLocation.y);
                    ItemInfo itemInfo = InventoryManager.GetSelectedItemInfo(InventoryType.Player);
                
                    if(itemInfo != null)
                    {
                        switch(itemInfo.itemType)
                        {
                            case ItemType.Tool:
                                switch(itemInfo.itemName)
                                {
                                    case "Hoe":
                                        if (tileIndicator.IndicatorAllowed)
                                        {
                                            if(CheckStamina())
                                            {
                                                StartCoroutine(UseHoe(clickDirection, tileAttributeDetail));
                                                UseStamina();
                                            }
                                        }

                                        break;
                                    case "Watering Can":
                                        if (tileIndicator.IndicatorAllowed)
                                        {
                                            if(CheckStamina())
                                            {
                                                StartCoroutine(UseWateringCan(clickDirection, tileAttributeDetail));
                                                UseStamina();
                                            }
                                        }
                                        break;
                                    case "Scythe":
                                        if (indicator.IndicatorAllowed)
                                        {
                                            if(CheckStamina())
                                            {
                                                clickDirection = PlayerDirection(indicator.GetLocationIndicator(), GetCenter());
                                                StartCoroutine(UseScythe(clickDirection, itemInfo));
                                                UseStamina();
                                            }
                                        }
                                        break;
                                    case "Basket":
                                        if (tileIndicator.IndicatorAllowed)
                                        {
                                            if(CheckStamina())
                                            {
                                                StartCoroutine(UseBasket(clickDirection, tileAttributeDetail, itemInfo));
                                                UseStamina();
                                            }
                                        }
                                        else if (indicator.IndicatorAllowed)
                                        {
                                            if(CheckStamina())
                                            {
                                                StartCoroutine(UseBasket(clickDirection, itemInfo));
                                                UseStamina();
                                            }
                                        }
                                        break;
                                    case "Axe":
                                        if (tileIndicator.IndicatorAllowed)
                                        {
                                            if(CheckStamina())
                                            {
                                                StartCoroutine(UseAxe(clickDirection, tileAttributeDetail, itemInfo));
                                                UseStamina();
                                            }
                                        }

                                        break;
                                    case "Pickaxe":
                                        if (tileIndicator.IndicatorAllowed)
                                        {
                                            if (CheckStamina())
                                            {
                                                StartCoroutine(UsePickaxe(clickDirection, tileAttributeDetail, itemInfo));
                                                UseStamina();
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case ItemType.Seed:
                                if(Input.GetMouseButtonDown(0))
                                {
                                    if (itemInfo.isDroppable && tileIndicator.IndicatorAllowed && tileAttributeDetail.ageDig > -1 && tileAttributeDetail.seedNo == -1)
                                    {
                                        if(CheckStamina())
                                        {
                                            UseSeed(tileAttributeDetail, itemInfo);
                                            UseStamina();
                                        }
                                    }
                                    else EventHandler.CallDropEvent();
                                }
                                break;
                            case ItemType.Goods:
                                if(Input.GetMouseButtonDown(0))
                                {
                                    if (itemInfo.isDroppable && tileIndicator.IndicatorAllowed) EventHandler.CallDropEvent();
                                }
                                break;
                            case ItemType.Animal:
                                if (Input.GetMouseButtonDown(0))
                                {
                                    if (itemInfo.isDroppable && tileIndicator.IndicatorAllowed) EventHandler.CallDropEvent();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    private bool CheckStamina()
    {
        if(currentStamina - decreaseStamina < 0)
        {
            EventHandler.CallNotEnoughStamina();
            hasEnoughStamina = false;
        }
        else
        {
            hasEnoughStamina = true;
        }
        return hasEnoughStamina;
    }

    private void UseStamina()
    {
        if(currentStamina - decreaseStamina >= 0)
        {
            currentStamina = currentStamina - decreaseStamina;
            EventHandler.CallStaminaEvent(currentStamina);
            
        }
    }

    private void MovementInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        

        if(xInput !=0 || yInput != 0)
        {
            isWalking = true;
            isRunning = false;
            isIdle = false;
            movementSpeed = walkingSpeed;

            if (xInput > 0) direction = Direction.right;
            else if (xInput < 0) direction = Direction.left;
            else if (yInput > 0) direction = Direction.up;
            else if (yInput < 0) direction = Direction.down;
        }

        else if(xInput == 0 && yInput == 0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }

        if(xInput != 0 && yInput != 0)
        {
            xInput = xInput * 0.7f;
            yInput = yInput * 0.7f;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = runningSpeed;
        }
        else
        {
            isWalking = true;
            isRunning = false;
            isIdle = false;
            movementSpeed = walkingSpeed;
        }
    }
    
    public static void DisableInput()
    {
        InputDisabled = true;
        xInput = 0f;
        yInput = 0f;
        isWalking = false;
        isRunning = false;
        isIdle = true;

        EventHandler.CallActionEvent(xInput, yInput, isIdle, isWalking, isRunning, isHolding,
            idleRight, idleLeft, idleUp, idleDown,
            isToolRight, isToolLeft, isToolUp, isToolDown,
            isPullRight, isPullLeft, isPullUp, isPullDown,
            isWaterRight, isWaterLeft, isWaterUp, isWaterDown,
            isReapRight, isReapLeft, isReapUp, isReapDown);
    }

    public static void HoldingItem(int itemNo)
    {
        ItemInfo itemInfo = InventoryManager.GetItemInfo(itemNo);

        if(itemInfo != null)
        {          
            sp.sprite = itemInfo.itemSprite;
            sp.gameObject.SetActive(true);

            armsAttribute.typeAnimationAttr = TypeAnimationAttribute.Hold;
            characterDetailAttributes.Clear();
            characterDetailAttributes.Add(armsAttribute);
            animationOverride.OverrideCharacterAttributes(characterDetailAttributes);
            isHolding = true;
        }
    }

    public static void NotHoldingItem()
    {
        sp.sprite = null;
        sp.gameObject.SetActive(false);
        armsAttribute.typeAnimationAttr = TypeAnimationAttribute.None;
        characterDetailAttributes.Clear();
        characterDetailAttributes.Add(armsAttribute);
        animationOverride.OverrideCharacterAttributes(characterDetailAttributes);
        isHolding = false;
    }

    private void TestInput()
    {
        if(Input.GetKey(KeyCode.T))
        {
            TimeManager.TestMin();
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            TimeManager.TestDay();
        }

    }

    private Vector3Int PlayerDirection(Vector3 indicatorLocation, Vector3 playerLocation)
    {
        if (indicatorLocation.x > playerLocation.x && indicatorLocation.y < (playerLocation.y + indicator.Radius / 2f) && indicatorLocation.y > (playerLocation.y - indicator.Radius / 2f)) return Vector3Int.right;
        else if (indicatorLocation.x < playerLocation.x && indicatorLocation.y < (playerLocation.y + indicator.Radius / 2f) && indicatorLocation.y > (playerLocation.y - indicator.Radius / 2f)) return Vector3Int.left;
        else if (indicatorLocation.y > playerLocation.y) return Vector3Int.up;
        else return Vector3Int.down;
    }

    private Vector3Int ClickDirection(Vector3Int indicatorLocation, Vector3Int playerLocation)
    {
        if(indicatorLocation.x > playerLocation.x) return Vector3Int.right;
        else if (indicatorLocation.y > playerLocation.y) return Vector3Int.up;
        else if (indicatorLocation.x < playerLocation.x) return Vector3Int.left;
        else return Vector3Int.down;
    }


    private void UseSeed(TileAttributeDetail tileAttributeDetail, ItemInfo itemInfo)
    {
        if(TileManager.GetPlantInfo(itemInfo.itemNo) != null)
        {
            tileAttributeDetail.seedNo = itemInfo.itemNo;
            tileAttributeDetail.age = 0;

            TileManager.ShowPlantTile(tileAttributeDetail);
            EventHandler.CallRemoveEvent();
        }
    }
    private IEnumerator UsePickaxe(Vector3Int clickDirection, TileAttributeDetail tileAttributeDetail, ItemInfo itemInfo)
    {
        InputDisabled = true;
        disableTool = true;

        toolAttribute.typeAnimationAttr = TypeAnimationAttribute.Pickaxe;
        characterDetailAttributes.Clear();
        characterDetailAttributes.Add(toolAttribute);
        animationOverride.OverrideCharacterAttributes(characterDetailAttributes);

        if (clickDirection == Vector3Int.up) isToolUp = true;
        else if (clickDirection == Vector3Int.down) isToolDown = true;
        else if (clickDirection == Vector3Int.right) isToolRight = true;
        else if (clickDirection == Vector3Int.left) isToolLeft = true;


        Plant plant = TileManager.GetPlantInPosition(tileAttributeDetail);
        if (plant != null)
        {
            plant.ToolFarm(itemInfo, isToolUp, isToolDown, isToolRight, isToolLeft);
        }

        yield return new WaitForSeconds(0.75f);

        InputDisabled = false;
        disableTool = false;
    }

    private IEnumerator UseBasket(Vector3Int clickDirection, TileAttributeDetail tileAttributeDetail, ItemInfo itemInfo)
    {
        InputDisabled = true;
        disableTool = true;
        
        if (clickDirection == Vector3Int.up) isPullUp = true;
        else if (clickDirection == Vector3Int.down) isPullDown = true;
        else if (clickDirection == Vector3Int.right) isPullRight = true;
        else if (clickDirection == Vector3Int.left) isPullLeft = true;

        Plant plant = TileManager.GetPlantInPosition(tileAttributeDetail);
        if (plant != null)
        {
            plant.ToolFarm(itemInfo, isPullUp, isPullDown, isPullRight, isPullLeft);
        }
       
        yield return new WaitForSeconds(1f);

        InputDisabled = false;
        disableTool = false;
    }

    private IEnumerator UseBasket(Vector3Int clickDirection, ItemInfo itemInfo)
    {
        InputDisabled = true;
        disableTool = true;
        if (Input.GetMouseButton(0))
        {
            if (clickDirection == Vector3Int.up) isPullUp = true;
            else if (clickDirection == Vector3Int.down) isPullDown = true;
            else if (clickDirection == Vector3Int.right) isPullRight = true;
            else if (clickDirection == Vector3Int.left) isPullLeft = true;
        }

        float radius = 2.0f;
        Vector2 pivot = new Vector2(GetCenter().x + (clickDirection.x * (radius / 2f)), GetCenter().y + (clickDirection.y * (radius / 2f)));
        Vector2 scale = new Vector2(radius, radius);

        Item[] items = Others.GetObjectInPositionBoxNon<Item>(15, pivot, scale, 0f);
        for (int i = items.Length - 1; i >= 0; i--)
        {
            if (items[i] != null)
            {
                if (InventoryManager.GetItemInfo(items[i].ItemNo).itemType == ItemType.Animal)
                {
                    Vector3 visualEffectLocation = new Vector3(items[i].transform.position.x, items[i].transform.position.y + TileManager.gridSize / 2f, items[i].transform.position.z);
                    EventHandler.CallFarmEffect(visualEffectLocation, FarmEffectType.Animal);
                    Destroy(items[i].gameObject);
                    InventoryManager.AddItemInInventory(InventoryType.Player, items[i].ItemNo);
                }
            }
        }

        yield return new WaitForSeconds(1f);

        InputDisabled = false;
        disableTool = false;

    }

    private IEnumerator UseAxe(Vector3Int clickDirection, TileAttributeDetail tileAttributeDetail, ItemInfo itemInfo)
    {
        InputDisabled = true;
        disableTool = true;

        toolAttribute.typeAnimationAttr = TypeAnimationAttribute.Axe;
        characterDetailAttributes.Clear();
        characterDetailAttributes.Add(toolAttribute);
        animationOverride.OverrideCharacterAttributes(characterDetailAttributes);

        if (clickDirection == Vector3Int.up) isToolUp = true;
        else if (clickDirection == Vector3Int.down) isToolDown = true;
        else if (clickDirection == Vector3Int.right) isToolRight = true;
        else if (clickDirection == Vector3Int.left) isToolLeft = true;


        Plant plant = TileManager.GetPlantInPosition(tileAttributeDetail);
        if (plant != null)
        {
            plant.ToolFarm(itemInfo, isToolUp, isToolDown, isToolRight, isToolLeft);
        }

        yield return new WaitForSeconds(0.75f);

        InputDisabled = false;
        disableTool = false;
    }

    private IEnumerator UseScythe(Vector3Int clickDirection, ItemInfo itemInfo)
    {
        InputDisabled = true;
        disableTool = true;

        toolAttribute.typeAnimationAttr = TypeAnimationAttribute.Scythe;
        characterDetailAttributes.Clear();
        characterDetailAttributes.Add(toolAttribute);
        animationOverride.OverrideCharacterAttributes(characterDetailAttributes);

        if (Input.GetMouseButton(0))
        { 
            if (clickDirection == Vector3Int.up) isReapUp = true;
            else if (clickDirection == Vector3Int.down) isReapDown = true;
            else if (clickDirection == Vector3Int.right) isReapRight = true;
            else if (clickDirection == Vector3Int.left) isReapLeft = true;
        }

        float radius = 1.0f;
        Vector2 pivot = new Vector2(GetCenter().x + (clickDirection.x * (radius / 2f)), GetCenter().y + (clickDirection.y * (radius / 2f)));
        Vector2 scale = new Vector2(radius, radius);

        Item[] items = Others.GetObjectInPositionBoxNon<Item>(15, pivot, scale, 0f);
        int farmable = 0;
        for (int i = items.Length - 1; i >= 0; i--)
        {
            if (items[i] != null)
            {
                if (InventoryManager.GetItemInfo(items[i].ItemNo).itemType == ItemType.Farmable)
                {
                    Vector3 visualEffectLocation = new Vector3(items[i].transform.position.x, items[i].transform.position.y + TileManager.gridSize / 2f, items[i].transform.position.z);
                    EventHandler.CallFarmEffect(visualEffectLocation, FarmEffectType.Grass);
                    Destroy(items[i].gameObject);

                    Vector3 location = new Vector3(items[i].gameObject.transform.position.x + Random.Range(-1f, 1f), items[i].gameObject.transform.position.y + Random.Range(-1f, 1f), 0f);
                    ItemSave.SpawnItemInScene(1007, location);
                    farmable++;
                    if (farmable >= 2) break;
                }
            }
        }
        yield return new WaitForSeconds(1f);

        InputDisabled = false;
        disableTool = false;
    }

    private IEnumerator UseWateringCan(Vector3Int clickDirection, TileAttributeDetail tileAttributeDetail)
    {
        InputDisabled = true;
        disableTool = true;
        toolAttribute.typeAnimationAttr = TypeAnimationAttribute.WateringCan;
        characterDetailAttributes.Clear();
        characterDetailAttributes.Add(toolAttribute);
        animationOverride.OverrideCharacterAttributes(characterDetailAttributes);

        if (clickDirection == Vector3Int.up) isWaterUp = true;
        else if (clickDirection == Vector3Int.down) isWaterDown = true;
        else if (clickDirection == Vector3Int.right) isWaterRight = true;
        else if (clickDirection == Vector3Int.left) isWaterLeft = true;

        yield return new WaitForSeconds(0.5f);

        if (tileAttributeDetail.ageWater == -1) tileAttributeDetail.ageWater = 0;
        TileManager.SetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y, tileAttributeDetail);
        TileManager.ShowWaterTile(tileAttributeDetail);

        yield return new WaitForSeconds(0.5f);

        InputDisabled = false;
        disableTool = false;
    }

    private IEnumerator UseHoe(Vector3Int clickDirection, TileAttributeDetail tileAttributeDetail)
    {
        InputDisabled = true;
        disableTool = true;
        toolAttribute.typeAnimationAttr = TypeAnimationAttribute.Hoe;
        characterDetailAttributes.Clear();
        characterDetailAttributes.Add(toolAttribute);
        animationOverride.OverrideCharacterAttributes(characterDetailAttributes);

        if(clickDirection == Vector3Int.up) isToolUp = true;
        else if (clickDirection == Vector3Int.down) isToolDown = true;
        else if (clickDirection == Vector3Int.right) isToolRight = true;
        else if (clickDirection == Vector3Int.left) isToolLeft = true;

        yield return new WaitForSeconds(0.25f);

        if (tileAttributeDetail.ageDig == -1) tileAttributeDetail.ageDig = 0;
        TileManager.SetTileAttributeDetail(tileAttributeDetail.x, tileAttributeDetail.y, tileAttributeDetail);
        TileManager.ShowDigTile(tileAttributeDetail);

        yield return new WaitForSeconds(0.5f);

        InputDisabled = false;
        disableTool = false;
    }

    
    public Vector3 GetCenter()
    {
        return new Vector3(transform.position.x, transform.position.y + yBasis, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            ItemInfo itemInfo = InventoryManager.GetItemInfo(item.ItemNo);

            if(itemInfo.isPickable)
            {
                if (itemInfo.itemType == ItemType.Goods || itemInfo.itemType == ItemType.Seed || itemInfo.itemType == ItemType.Tool)
                {
                    StartCoroutine(PickUp(collision));
                    InventoryManager.AddItemInInventory(InventoryType.Player, item);
                }
            }
         
        }
    }

    private IEnumerator PickUp(Collider2D collision)
    {
        yield return new WaitForSeconds(0.3f);
        if(collision != null) Destroy(collision.gameObject);
    }

    private void PassOut()
    {
        int hour = TimeManager.GetTime().Hours;
        int min = TimeManager.GetTime().Minutes;
        
        if (hour == 23 && min == 0)
        {
            EventHandler.CallLateNightEvent();
        }

        if(hour == 23 && min == 50)
        {
            TimeManager.PassDay();
            float xCor = Mathf.Approximately(respawnPosition.x, 0f) ? transform.position.x : respawnPosition.x;
            float yCor = Mathf.Approximately(respawnPosition.y, 0f) ? transform.position.y : respawnPosition.y;
            SceneController.FadeOutLoad(SceneType.Home.ToString(), new Vector3(xCor, yCor, 0f));
            if(coin != 0)
            {
                double lostCoin = coin * 0.2f;
                coin = coin - (int)lostCoin;
                EventHandler.CallLostCoinEvent((int)lostCoin);
            }
        }
    }

    public void Register()
    {
        SaveManager.saveList.Add(this);
    }

    public void Unregister()
    {
        SaveManager.saveList.Remove(this);
    }

    public void Store(string scene)
    {

    }

    public void Recover(string scene)
    {
    
    }

    public void Load(SaveGame saveGame)
    {
        if(saveGame.saveObjectDict.TryGetValue(SaveID, out SaveObject saveObject))
        {
            if(saveObject.scene.TryGetValue("Base", out SaveScene saveScene))
            {
                if(saveScene.vector3Data != null && saveScene.vector3Data.TryGetValue("location", out SerializeVector3 location)) transform.position = new Vector3(location.x, location.y, location.z);
                if(saveScene.stringData != null)
                {
                    if (saveScene.stringData.TryGetValue("scene", out string scene)) SceneController.FadeOutLoad(scene, transform.position);
                    if (saveScene.stringData.TryGetValue("direction", out string pDir))
                    {
                        if (Enum.TryParse<Direction>(pDir, true, out Direction dir))
                        {
                            direction = dir;
                            switch(direction)
                            {
                                case Direction.right:
                                    EventHandler.CallActionEvent(0f, 0f, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                                    break;
                                case Direction.left:
                                    EventHandler.CallActionEvent(0f, 0f, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                                    break;
                                case Direction.up:
                                    EventHandler.CallActionEvent(0f, 0f, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false ,false);
                                    break;
                                case Direction.down:
                                    EventHandler.CallActionEvent(0f, 0f, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                                    break;
                                default:
                                    EventHandler.CallActionEvent(0f, 0f, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                                    break;
                            }
                        }
                    }
                }
                if (saveScene.intData != null && saveScene.intData.TryGetValue("coin", out int pCoin)) coin = pCoin;
                if (saveScene.intData != null && saveScene.intData.TryGetValue("stamina", out int pStamina))
                {
                    currentStamina = pStamina;
                    EventHandler.CallStaminaEvent(currentStamina);
                }
            }
        }
    }

    public SaveObject Save()
    {
        SaveObject.scene.Remove("Base");
        SaveScene saveScene = new SaveScene();
        saveScene.vector3Data = new Dictionary<string, SerializeVector3>();
        saveScene.stringData = new Dictionary<string, string>();
        saveScene.intData = new Dictionary<string, int>();
        SerializeVector3 serializeVector3 = new SerializeVector3(transform.position.x, transform.position.y, transform.position.z);
        
        saveScene.vector3Data.Add("location", serializeVector3);
        saveScene.stringData.Add("scene", SceneManager.GetActiveScene().name);
        saveScene.stringData.Add("direction", direction.ToString());
        saveScene.intData.Add("coin", coin);
        saveScene.intData.Add("stamina", currentStamina);
        SaveObject.scene.Add("Base", saveScene);

        return SaveObject;
    }

}
