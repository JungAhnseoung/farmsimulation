using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private InventorySlot[] inventorySlot = null;
    public GameObject itemDragged;
    [HideInInspector] public GameObject inventoryItemDescription;

    private void OnEnable()
    {
        EventHandler.InventoryEvent += InventoryUpdate;
    }

    private void OnDisable()
    {
        EventHandler.InventoryEvent -= InventoryUpdate;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            InventoryItemDescription itemDescription = FindObjectOfType<InventoryItemDescription>();
            if(itemDescription != null)
            {
                Destroy(itemDescription.gameObject);
            }
        }
    }

    private void InventoryUpdate(InventoryType inventoryType, List<InventoryItem> inventoryList)
    {
        if(inventoryType == InventoryType.Player)
        {
            if (inventorySlot.Length > 0)
            {
                for (int i = 0; i < inventorySlot.Length; i++)
                {
                    inventorySlot[i].inventorySlotIcon.sprite = emptySprite;
                    inventorySlot[i].TextMeshProUGUI.text = "";
                    inventorySlot[i].itemInfo = null;
                    inventorySlot[i].itemCount = 0;
                    SetSelectedSlotWithIndex(i);
                }
            }

            if (inventorySlot.Length > 0 && inventoryList.Count > 0)
            {
                for (int i = 0; i < inventorySlot.Length; i++)
                {
                    if (i < inventoryList.Count)
                    {
                        int itemNo = inventoryList[i].itemNo;

                        ItemInfo itemInfo = InventoryManager.GetItemInfo(itemNo);

                        if (itemInfo != null)
                        {
                            inventorySlot[i].itemInfo = itemInfo;
                            inventorySlot[i].TextMeshProUGUI.text = inventoryList[i].itemCount.ToString();
                            inventorySlot[i].inventorySlotIcon.sprite = itemInfo.itemIcon;
                            inventorySlot[i].itemCount = inventoryList[i].itemCount;
                            SetSelectedSlotWithIndex(i);
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
        }
    }

    public void SetSelectedSlot()
    {
        if(inventorySlot.Length>0)
        {
            for(int i=0; i<inventorySlot.Length; i++)
            {
                SetSelectedSlotWithIndex(i);
            }
        }
    }

    public void SetSelectedSlotWithIndex(int itemIndex)
    {
        if (inventorySlot.Length > 0 && inventorySlot[itemIndex].itemInfo != null)
        {
            if (inventorySlot[itemIndex].isItemSelected)
            {
                inventorySlot[itemIndex].inventorySlotSelected.gameObject.SetActive(true);
                inventorySlot[itemIndex].inventorySlotSelected.gameObject.GetComponent<Animator>().SetBool("isSelected", true);
                InventoryManager.SetSelectedItem(InventoryType.Player, inventorySlot[itemIndex].itemInfo.itemNo);
            }
        }
    }

    public void RemoveSelectedSlot()
    {
        if(inventorySlot.Length>0)
        {
            for(int i=0; i<inventorySlot.Length;i++)
            {
                if (inventorySlot[i].isItemSelected)
                {
                    inventorySlot[i].inventorySlotSelected.gameObject.SetActive(false);
                    inventorySlot[i].isItemSelected = false;
                    inventorySlot[i].inventorySlotSelected.gameObject.GetComponent<Animator>().SetBool("isSelected", false);
                    InventoryManager.RemoveSelectedItem(InventoryType.Player);
                }
            }
        }
    }

    public void RemoveSelectedItem()
    {
        for (int i = 0; i < inventorySlot.Length; i++) inventorySlot[i].RemoveSelectedItem();
    }

    public void RemoveItemDragged()
    {
        for(int i=0; i<inventorySlot.Length; i++)
        {
            if (inventorySlot[i] != null && inventorySlot[i].itemDragged != null) Destroy(inventorySlot[i].itemDragged);
        }
    }
}
