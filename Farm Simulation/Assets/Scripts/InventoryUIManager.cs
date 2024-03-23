using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject editableinventory = null;
    [SerializeField] private InventoryBar inventoryBar = null;
    [SerializeField] private InventoryOpen editableInventoryOpen = null;
    public static  GameObject inventory = null;
    public static InventoryOpen inventoryOpen = null;
    private static bool isInventoryOpen = false;

    public static bool InventoryOpen {  get => isInventoryOpen; set => isInventoryOpen = value; }



    private void Awake()
    {
        inventory = editableinventory;
        inventoryOpen = editableInventoryOpen;
        inventory.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(InventoryOpen)
            {
                CloseInventory();
                inventoryOpen.RemoveSelectedSlot();
            }
            else
            {
                OpenInventory();
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(InventoryOpen)
            {
                CloseInventory();
                inventoryOpen.RemoveSelectedSlot();
            }
            else if(GameMenuManager.GameMenuOpen)
            {
                GameMenuManager.CloseGameMenu();
            }
            else if(!InventoryOpen && !GameMenuManager.GameMenuOpen)
            {
                GameMenuManager.OpenGameMenu();
            }
        }

        if(Chest.ChestOpen)
        {
            if(!InventoryOpen)
            {
                OpenInventory();
            }
        }
    }

    public static void OpenInventory()
    {
        InventoryOpen = true;
        Player.InputDisabled = true;
        inventory.SetActive(true);
    }

    public static void CloseInventory()
    {
        inventoryOpen.RemoveItemDragged();
        InventoryOpen = false;
        Player.InputDisabled = false;
        inventory.SetActive(false);
    }


}
