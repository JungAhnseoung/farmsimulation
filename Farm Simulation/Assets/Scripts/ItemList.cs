using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ItemList", menuName ="Item List")]
public class ItemList : ScriptableObject
{
    [SerializeField] public List<ItemInfo> itemInfo; 
}

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

[System.Serializable]
public class ItemInfo
{
    public ItemType itemType;
    public int itemNo;
    public string itemName;
    public string itemDescription;
    public bool isPickable;
    public bool isCarriable;
    public bool isEdible;
    public bool isDroppable;
    public int distance;
    public int nontiledistance;
    public Sprite itemIcon;
    public Sprite itemSprite;

}

