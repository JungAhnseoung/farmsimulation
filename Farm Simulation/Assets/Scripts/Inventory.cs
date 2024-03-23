using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryType
{
    Player,
    Chest,
    Shop,
    Count
}

[System.Serializable]
public struct InventoryItem
{
    public int itemNo;
    public int itemCount;
}


public class Inventory : MonoBehaviour
{
    public static int inventoryCapacity = 26;
}

