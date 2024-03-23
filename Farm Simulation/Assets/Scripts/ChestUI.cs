using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestUI : MonoBehaviour
{
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private ChestSlot[] chestSlot = null;
    [SerializeField] private TextMeshProUGUI coinTextMeshPro = null;

    public static int coinTotal = 0;

    private void OnEnable()
    {
        EventHandler.ChestEvent += ChestUpdate;
        if (InventoryManager.inventoryItemList != null) ChestUpdate(InventoryType.Chest, InventoryManager.inventoryItemList[(int)InventoryType.Chest]);
    }

    private void OnDisable()
    {
        EventHandler.ChestEvent -= ChestUpdate;
    }

    private void ChestUpdate(InventoryType inventoryType, List<InventoryItem> inventoryList)
    {
        if(inventoryType == InventoryType.Chest)
        {
            if(chestSlot.Length > 0)
            {
                for(int i = 0; i < chestSlot.Length; i++)
                {
                    chestSlot[i].chestSlotIcon.sprite = emptySprite;
                    chestSlot[i].textMeshProUGUI.text = "";
                    chestSlot[i].itemInfo = null;
                    chestSlot[i].itemCount = 0;
                    coinTotal = 0;
                    coinTextMeshPro.text = coinTotal.ToString();
                }
            }

            if(chestSlot.Length > 0 && inventoryList.Count > 0)
            {
                for(int i=0; i< chestSlot.Length; i++)
                {
                    if (i < inventoryList.Count)
                    {
                        int itemNo = inventoryList[i].itemNo;
                        ItemInfo itemInfo = InventoryManager.GetItemInfo(itemNo);

                        if (itemInfo != null)
                        {
                            chestSlot[i].itemInfo = itemInfo;
                            chestSlot[i].textMeshProUGUI.text = inventoryList[i].itemCount.ToString();
                            chestSlot[i].chestSlotIcon.sprite = itemInfo.itemIcon;
                            chestSlot[i].itemCount = inventoryList[i].itemCount;
                            coinTotal += itemInfo.sellPrice * inventoryList[i].itemCount;
                            coinTextMeshPro.text = coinTotal.ToString();
                        }
                    }
                    else break;
                }
            }
        }
    }


}
