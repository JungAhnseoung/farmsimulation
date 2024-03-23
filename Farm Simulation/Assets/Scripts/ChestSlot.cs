using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestSlot : MonoBehaviour
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

    
}
