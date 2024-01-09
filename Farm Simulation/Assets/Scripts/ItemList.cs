using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ItemList", menuName ="Item List")]
public class ItemList : ScriptableObject
{
    [SerializeField]
    public List<ItemInfo> item; 
}


[System.Serializable]
public class ItemInfo
{

    public enum ItemType
    {
        Tool,
        Seed,
        Goods,
        Farmable,
        Furniture,
        Count,
        None
    }

    public ItemType itemType;
    public int itemNo;
    public string itemName;
    public string itemDescription;
    public bool isPickable;
    public bool isCarriable;
    public bool isEdible;

    public Sprite itemSprite;

}

