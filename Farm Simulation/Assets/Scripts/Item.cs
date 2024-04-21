using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [ItemNoName]
    [SerializeField]
    private int itemNo;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    [SerializeField] private GameObject shadow = null;
    [SerializeField] private RuntimeAnimatorController[] animationControllers = null;

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
            boxCollider.size = new Vector2(1f, 1f);
            if (itemInfo.itemType == ItemType.Farmable)
            {
                gameObject.AddComponent<ObjectVisualEffect>();
            }

            if(itemInfo.itemType == ItemType.Animal)
            {
                gameObject.transform.localScale = new Vector3(6.8f, 5.3f, 1f);
                Animal animal = gameObject.AddComponent<Animal>();
                animal.itemNo = itemNo;

                GameObject shadowObject = Instantiate(shadow, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.38f, gameObject.transform.position.z), gameObject.transform.rotation);
                shadowObject.transform.SetParent(gameObject.transform);

                if(gameObject.GetComponent<Animator>() == null)
                {
                    Animator animator = gameObject.AddComponent<Animator>();
                    if (animator != null)
                    {
                        if(itemNo == 1031) animator.runtimeAnimatorController = animationControllers[0];
                        else if(itemNo == 1032) animator.runtimeAnimatorController = animationControllers[1];
                    }
                }

                if(gameObject.GetComponent<Rigidbody2D>() == null)
                {
                    Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
                    rigidbody.gravityScale = 0f;
                    rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    rigidbody.freezeRotation = true;
                }

            }
        }
    }

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        if (itemNo != 0) Initailize(itemNo);
    }

}