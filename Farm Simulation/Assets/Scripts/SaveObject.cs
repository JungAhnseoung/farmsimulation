using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializeVector3
{
    public float x, y, z;
    public SerializeVector3()
    {

    }

    public SerializeVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[System.Serializable]
public class ItemInScene
{
    public int itemNo;
    public string itemName;
    public SerializeVector3 location;

    public ItemInScene()
    {
        location = new SerializeVector3();
    }
}

[System.Serializable]
public class SaveScene
{
    public List<ItemInScene> itemsInSceneList;
    public Dictionary<string, TileAttributeDetail> tileAttributeDetailData;
    public Dictionary<string, bool> boolData;
    public Dictionary<string, string> stringData;
    public Dictionary<string, SerializeVector3> vector3Data;
    public List<InventoryItem>[] inventoryItemArrList;
    public Dictionary<string, int[]> intArrData;
    public Dictionary<string, int> intData;
}

[System.Serializable]
public class SaveObject
{
    public Dictionary<string, SaveScene> scene;
    
    public SaveObject()
    {
        scene = new Dictionary<string, SaveScene>();
    }
    
    public SaveObject(Dictionary<string, SaveScene> scene)
    {
        this.scene = scene;
    }
}
