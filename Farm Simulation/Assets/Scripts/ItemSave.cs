using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ItemSave : MonoBehaviour, Save
{
    [SerializeField] private GameObject editableItem = null;
    private static GameObject item = null;
    private static Transform itemsGroup;
    private string saveID;
    private SaveObject saveObject;

    public string SaveID { get  { return saveID; } set { saveID = value; } }
    public SaveObject SaveObject { get { return saveObject; } set { saveObject = value; } }


    private void OnEnable()
    {
        Register();
        EventHandler.AfterLoad += AfterLoad;
    }

    private void OnDisable()
    {
        Unregister();
        EventHandler.AfterLoad -= AfterLoad;
    }

    private void AfterLoad()
    {
        itemsGroup = GameObject.FindGameObjectWithTag("ItemsGroup").transform;
    }

    private void Awake()
    {
        saveID = GetComponent<GUIDGenerator>().GUID;
        SaveObject = new SaveObject();
        item = editableItem;
    }

    private void ClearItems()
    {
        Item[] items = GameObject.FindObjectsOfType<Item>();

        for (int i = items.Length - 1; i > -1; i--)
        {
            Destroy(items[i].gameObject);
        }
    }

    public void Recover(string scene)
    {
        if(SaveObject.scene.TryGetValue(scene, out SaveScene saveScene))
        {
            if(saveScene.itemsInSceneList != null)
            {
                ClearItems();
                SpawnItemsInScene(saveScene.itemsInSceneList);
            }
        }
    }

    public void Register()
    {
        SaveManager.saveList.Add(this);
    }

    public void Store(string scene)
    {
        SaveObject.scene.Remove(scene);
        Item[] items = FindObjectsOfType<Item>();
        List<ItemInScene> itemsInScene = new List<ItemInScene>();

        foreach(Item item in items)
        {
            ItemInScene itemInScene = new ItemInScene();
            itemInScene.itemNo = item.ItemNo;
            itemInScene.itemName = item.name;
            itemInScene.location = new SerializeVector3(item.transform.position.x, item.transform.position.y, item.transform.position.z);

            itemsInScene.Add(itemInScene);
        }

        SaveScene saveScene = new SaveScene();
        saveScene.itemsInSceneList = itemsInScene;
        SaveObject.scene.Add(scene, saveScene);
    }

    public void Unregister()
    {
        SaveManager.saveList.Remove(this);
    }
    public void Load(SaveGame saveGame)
    {
        if(saveGame.saveObjectDict.TryGetValue(SaveID, out SaveObject saveObject))
        {
            SaveObject = saveObject;
            Recover(SceneManager.GetActiveScene().name);
        }
    }

    public SaveObject Save()
    {
        Store(SceneManager.GetActiveScene().name);
        return SaveObject;
    }

    private void SpawnItemsInScene(List<ItemInScene> itemsInScene)
    {
        GameObject itemObject;
        foreach(ItemInScene itemInScene in itemsInScene)
        {
            itemObject = Instantiate(item, new Vector3(itemInScene.location.x, itemInScene.location.y, itemInScene.location.z), Quaternion.identity, itemsGroup);
            Item spawnedItem = itemObject.GetComponent<Item>();
            spawnedItem.ItemNo = itemInScene.itemNo;
            spawnedItem.name = itemInScene.itemName;
        }
    }

    public static void SpawnItemInScene(int itemNo, Vector3 location)
    {
        GameObject itemObject = Instantiate(item, location, Quaternion.identity, itemsGroup);
        Item spawingItem = itemObject.GetComponent<Item>();
        spawingItem.Initailize(itemNo);
    }

}
