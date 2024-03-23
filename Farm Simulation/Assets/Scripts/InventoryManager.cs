using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour, Save
{
    private static Dictionary<int, ItemInfo> itemDict;
    private InventoryBar inventoryBar;

    [SerializeField] private ItemList ItemList = null;

    public static List<InventoryItem>[] inventoryItemList = null;

    public int[] inventoryListCapacity;
    private static int[] selectedItem;

    private string saveID;
    public string SaveID { get { return saveID; } set { saveID = value; } }

    private SaveObject saveObject;
    public SaveObject SaveObject { get { return saveObject; } set { saveObject = value; } }

    private void OnEnable()
    {
        EventHandler.DayPass += EmptyChest;
        Register();
    }

    private void OnDisable()
    {
        EventHandler.DayPass -= EmptyChest;
        Unregister();
    }

    private void Awake()
    {
       inventoryItemList = new List<InventoryItem>[(int)InventoryType.Count];
       inventoryListCapacity = new int[(int)InventoryType.Count];
        inventoryBar = FindObjectOfType<InventoryBar>();

        for (int i = 0; i < (int)InventoryType.Count; i++)
        {
            inventoryItemList[i] = new List<InventoryItem>();
        }
        inventoryListCapacity = new int[(int)InventoryType.Count];
        inventoryListCapacity[(int)InventoryType.Player] = Inventory.inventoryCapacity;
        inventoryListCapacity[(int)InventoryType.Chest] = Inventory.inventoryCapacity;

        itemDict = new Dictionary<int, ItemInfo>();
        foreach (ItemInfo itemInfo in ItemList.itemInfo) 
        {
            itemDict.Add(itemInfo.itemNo, itemInfo);
        }

        selectedItem = new int[(int)InventoryType.Count];
        for(int i = 0; i < (int)InventoryType.Count; i++)
        {
            selectedItem[i] = -1; //no item selected
        }

        SaveObject = new SaveObject();
        SaveID = GetComponent<GUIDGenerator>().GUID;
    }


    public static ItemInfo GetItemInfo(int itemNo)
    {
        return itemDict.TryGetValue(itemNo, out ItemInfo itemInfo) ? itemInfo : null;
    }

    public static int SearchForItem(InventoryType inventoryType, int itemNo)
    {
        List<InventoryItem> inventoryItems = inventoryItemList[(int)inventoryType];

        for(int i=0; i<inventoryItems.Count; i++)
        {
            if (inventoryItems[i].itemNo == itemNo) return i;
        }

        return -1;
    }

    public static int GetSelectedItem(InventoryType inventoryType)
    {
        return selectedItem[(int)inventoryType];
    }

    public static ItemInfo GetSelectedItemInfo(InventoryType inventoryType)
    {
        if (GetSelectedItem(inventoryType) == -1) return null;
        else return GetItemInfo(GetSelectedItem(inventoryType));
    }

    public static void SetSelectedItem(InventoryType inventoryType, int itemNo)
    {
        selectedItem[(int)inventoryType] = itemNo;
    }


    public static void RemoveSelectedItem(InventoryType inventoryType)
    {
        selectedItem[(int)inventoryType] = -1;
    }

   
    public static void AddItemInInventory(InventoryType inventoryType, Item item)
    {
        int itemNo = item.ItemNo;
        List<InventoryItem> inventoryItems = inventoryItemList[(int)inventoryType];

        int itemIndex = -1;

        for(int i=0; i< inventoryItems.Count; i++ )
        {
            if (inventoryItems[i].itemNo == itemNo) itemIndex = i;
        }

        if(itemIndex == -1)
        {
            InventoryItem newInventoryItem = new InventoryItem();
            newInventoryItem.itemNo = itemNo;
            newInventoryItem.itemCount = 1;
            inventoryItems.Add(newInventoryItem);
        }
        else
        {
            InventoryItem newInventoryItem = new InventoryItem();
            newInventoryItem.itemNo = itemNo;
            int amount = inventoryItems[itemIndex].itemCount + 1;
            newInventoryItem.itemCount = amount;
            inventoryItems[itemIndex] = newInventoryItem;
        }

        EventHandler.CallInventoryEvent(inventoryType, inventoryItemList[(int)inventoryType]);
    }

    public static void AddItemInInventory(InventoryType inventoryType, int itemNo)
    {
        List<InventoryItem> inventoryItems = inventoryItemList[(int)inventoryType];
        int itemIndex = SearchForItem(inventoryType, itemNo);

        if (itemIndex == -1)
        {
            InventoryItem newInventoryItem = new InventoryItem();
            newInventoryItem.itemNo = itemNo;
            newInventoryItem.itemCount = 1;
            inventoryItems.Add(newInventoryItem);
        }
        else
        {
            InventoryItem newInventoryItem = new InventoryItem();
            int amount = inventoryItems[itemIndex].itemCount + 1;
            newInventoryItem.itemCount = amount;
            newInventoryItem.itemNo = itemNo;
            inventoryItems[itemIndex] = newInventoryItem;
        }

        EventHandler.CallInventoryEvent(inventoryType, inventoryItemList[(int)inventoryType]);
        EventHandler.CallChestEvent(inventoryType, inventoryItemList[(int)inventoryType]);
    }

    public static void RemoveItemFromInventory(InventoryType inventoryType, int itemNo)
    {
        List<InventoryItem> inventoryItems = inventoryItemList[(int)inventoryType];
        int itemIndex = -1;

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].itemNo == itemNo) itemIndex = i;
        }

        if(itemIndex != -1)
        {
            InventoryItem inventoryItem = new InventoryItem();

            int count = inventoryItems[itemIndex].itemCount - 1;
            if(count >0)
            {
                inventoryItem.itemNo = itemNo;
                inventoryItem.itemCount = count;
                inventoryItems[itemIndex] = inventoryItem;
            }
            else
            {
                inventoryItems.RemoveAt(itemIndex);
            }
        }
        EventHandler.CallInventoryEvent(inventoryType, inventoryItemList[(int)inventoryType]);
        EventHandler.CallChestEvent(inventoryType, inventoryItemList[(int)inventoryType]);
    }


    public static void EmptyInventory(InventoryType inventoryType)
    {
        List<InventoryItem> inventoryItems = inventoryItemList[(int)inventoryType];
        inventoryItems.Clear();
        EventHandler.CallChestEvent(inventoryType, inventoryItemList[(int)inventoryType]);
    }

    private void EmptyChest(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        if (InventoryManager.inventoryItemList[(int)InventoryType.Chest].Count > 0)
        {
            Player.coin += ChestUI.coinTotal;
            InventoryManager.EmptyInventory(InventoryType.Chest);
        }
    }


    public static void SwapItemInInventory(InventoryType inventoryType, int itemA, int itemB)
    {
        if (itemA < inventoryItemList[(int)inventoryType].Count && itemB < inventoryItemList[(int)inventoryType].Count && itemA >= 0 && itemB >= 0 && itemA != itemB)
        {
            InventoryItem inventoryItemA = inventoryItemList[(int)inventoryType][itemA];
            InventoryItem inventoryItemB = inventoryItemList[(int)inventoryType][itemB];

            inventoryItemList[(int)inventoryType][itemB] = inventoryItemA;
            inventoryItemList[(int)inventoryType][itemA] = inventoryItemB;
            EventHandler.CallInventoryEvent(inventoryType, inventoryItemList[(int)inventoryType]);
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
            SaveObject = saveObject;
            if(saveObject.scene.TryGetValue("Base", out SaveScene saveScene))
            {
                if(saveScene.inventoryItemArrList != null)
                {
                    inventoryItemList = saveScene.inventoryItemArrList;
                    for(int i=0; i<(int)InventoryType.Count; i++)
                    {
                        EventHandler.CallInventoryEvent((InventoryType)i, inventoryItemList[i]);
                        EventHandler.CallChestEvent((InventoryType)i, inventoryItemList[i]);
                    }

                    Player.NotHoldingItem();
                    inventoryBar.RemoveSelectedSlot();
                }
                if (saveScene.intArrData != null && saveScene.intArrData.TryGetValue("inventoryCapacity", out int[] inventoryCapacity)) inventoryListCapacity = inventoryCapacity;
            }
        }
    }

    public SaveObject Save()
    {
        SaveScene saveScene = new SaveScene();
        SaveObject.scene.Remove("Base");
        saveScene.inventoryItemArrList = inventoryItemList;
        saveScene.intArrData = new Dictionary<string, int[]>();
        saveScene.intArrData.Add("inventoryCapacity", inventoryListCapacity);

        SaveObject.scene.Add("Base", saveScene);
        return SaveObject;
    }
}
