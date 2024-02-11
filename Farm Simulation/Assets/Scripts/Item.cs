using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [ItemNoName]
    [SerializeField]
    private int itemNo;
    private SpriteRenderer spriteRenderer;
    public int ItemNo 
    { 
        get 
        { return itemNo; } 
        set 
        { itemNo = value; } 
    }

    public void Initailize(int itemNo)
    {
        if(itemNo!=0)
        {
            this.itemNo = itemNo;
            ItemInfo itemInfo = InventoryManager.GetItemInfo(itemNo);
            spriteRenderer.sprite = itemInfo.itemSprite;

            if (itemInfo.itemType == ItemType.Farmable)
            {
                gameObject.AddComponent<ObjectVisualEffect>();
            }
        }
    }

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (itemNo != 0) Initailize(itemNo);
    }

}