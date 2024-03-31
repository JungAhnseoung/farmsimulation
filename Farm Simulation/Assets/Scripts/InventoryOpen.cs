using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    [SerializeField] private InventoryOpenSlot[] inventoryOpenSlot = null;
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private TextMeshProUGUI coinTextMeshPro = null;
    private int coin = 0;
    public GameObject itemDraggedObject;
    [HideInInspector] public GameObject itemDescription;
    
    private void OnEnable()
    {
        EventHandler.InventoryEvent += MovePlayerInventory;
        if(InventoryManager.inventoryItemList != null) MovePlayerInventory(InventoryType.Player, InventoryManager.inventoryItemList[(int)InventoryType.Player]);
    }

    private void OnDisable()
    {
        EventHandler.InventoryEvent -= MovePlayerInventory;
        RemoveItemTextObject();
    }

    public void RemoveItemTextObject()
    {
        if (itemDescription != null) Destroy(itemDescription);
    }

    public void RemoveItemDragged()
    {
        for(int i=0; i < InventoryManager.inventoryItemList[(int)InventoryType.Player].Count; i++)
        {
            if(i>9)
            {
                if (inventoryOpenSlot[i-10] != null)
                {
                    if(inventoryOpenSlot[i-10].itemDragged != null) Destroy(inventoryOpenSlot[i-10].itemDragged);
                }
            }
        }
    }

    private void MovePlayerInventory(InventoryType inventoryType, List<InventoryItem> inventoryItems)
    {
        if(inventoryType == InventoryType.Player)
        {
            InitInventoryOpenSlot();
            for(int i=0; i < InventoryManager.inventoryItemList[(int)InventoryType.Player].Count; i++)
            {
                if(i>9)
                {
                    inventoryOpenSlot[i-10].itemInfo = InventoryManager.GetItemInfo(inventoryItems[i].itemNo);
                    inventoryOpenSlot[i-10].itemAmount = inventoryItems[i].itemCount;

                    if (inventoryOpenSlot[i-10].itemInfo !=null)
                    {
                        inventoryOpenSlot[i-10].inventoryOpenImage.sprite = inventoryOpenSlot[i-10].itemInfo.itemSprite;
                        inventoryOpenSlot[i-10].text.text = inventoryOpenSlot[i-10].itemAmount.ToString();
                    }
                }
            }

            coin = Player.coin;
            coinTextMeshPro.text = coin.ToString();
        }
    }


    public void SetSelectedSlot()
    {
        if(inventoryOpenSlot.Length>0)
        {
            for(int i=0; i<inventoryOpenSlot.Length;i++)
            {
                if(inventoryOpenSlot.Length>0 && inventoryOpenSlot[i].itemInfo!=null)
                {
                    if (inventoryOpenSlot[i].isItemSelected)
                    {
                        inventoryOpenSlot[i].inventoryOpenSlotSelected.gameObject.SetActive(true);
                        inventoryOpenSlot[i].inventoryOpenSlotSelected.gameObject.GetComponent<Animator>().SetBool("isSelected", true);
                    }
                }
            }
        }
    }
    public void RemoveSelectedSlot()
    {
        if(inventoryOpenSlot.Length>0)
        {
            for(int i=0; i<inventoryOpenSlot.Length;i++)
            {
                if (inventoryOpenSlot[i].isItemSelected)
                {
                    inventoryOpenSlot[i].inventoryOpenSlotSelected.gameObject.SetActive(false);
                    inventoryOpenSlot[i].isItemSelected = false;
                    inventoryOpenSlot[i].inventoryOpenSlotSelected.gameObject.GetComponent<Animator>().SetBool("isSelected", false);
                 }
            }
        }
    }

    private void InitInventoryOpenSlot()
    {
        for(int i=0; i<16; i++)
        {
            inventoryOpenSlot[i].itemInfo = null;
            inventoryOpenSlot[i].itemAmount = 0;
            inventoryOpenSlot[i].inventoryOpenImage.sprite = emptySprite;
            inventoryOpenSlot[i].text.text = "";

        }
    }
}
