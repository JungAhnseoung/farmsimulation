using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [ItemNoName]
    [SerializeField]
    private int itemNo;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject shadow = null;
    public int ItemNo 
    { 
        get 
        { return itemNo; } 
        set 
        { itemNo = value; } 
    }

    public void Initailize(int itemNo)
    {
        if (itemNo != 0)
        {
            this.itemNo = itemNo;
            ItemInfo itemInfo = InventoryManager.GetItemInfo(itemNo);
            spriteRenderer.sprite = itemInfo.itemSprite;

            if (itemInfo.itemType == ItemType.Farmable)
            {
                gameObject.AddComponent<ObjectVisualEffect>();
            }

            if(itemInfo.itemType == ItemType.Animal)
            {
                gameObject.transform.localScale = new Vector3(6.8f, 5.3f, 1f);
                gameObject.AddComponent<Animal>();
                GameObject shadowObject = Instantiate(shadow, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.38f, gameObject.transform.position.z), gameObject.transform.rotation);
                shadowObject.transform.SetParent(gameObject.transform);
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