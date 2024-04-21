using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour, IPointerClickHandler
{

    public TextMeshProUGUI textMeshProUGUI;
    public Image chestSlotIcon;

    [SerializeField] private InventoryBar inventoryBar = null;
    [SerializeField] private InventoryOpen inventoryOpen = null;
    [SerializeField] public int slotIndex;
    [SerializeField] GameObject itemPrefab = null;

    public ItemInfo itemInfo;
    public int itemCount;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
        {
            if(itemInfo != null)
            {
                int itemNo = itemInfo.itemNo;
                InventoryManager.RemoveItemFromInventory(InventoryType.Chest, itemNo);
                InventoryManager.AddItemInInventory(InventoryType.Player, itemNo);
            }
        }
    }
}
