using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject editableGameMenu = null;
    [SerializeField] private InventoryBar editableInventoryBar = null;

    private static GameObject gameMenu = null;
    private static InventoryBar inventoryBar = null;

    private static bool isGameMenuOpen = false;
    public static bool GameMenuOpen { get => isGameMenuOpen; set => isGameMenuOpen = value; }

    private void Awake()
    {
       gameMenu = editableGameMenu;
       inventoryBar = editableInventoryBar;
       gameMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InventoryUIManager.InventoryOpen)
            {
                InventoryUIManager.CloseInventory();

            }
            else if (GameMenuOpen)
            {
                CloseGameMenu();
            }
            else if (!InventoryUIManager.InventoryOpen && !GameMenuOpen)
            {
                OpenGameMenu();
            }
        }

        if (Chest.ChestOpen || Shop.ShopOpen)
        {
            if (!InventoryUIManager.InventoryOpen)
            {
                InventoryUIManager.OpenInventory();
            }
        }
    }

    public static void OpenGameMenu()
    {
        inventoryBar.RemoveItemDragged();
        inventoryBar.RemoveSelectedItem();
        GameMenuOpen = true;
        Player.InputDisabled = true;
        Time.timeScale = 0;
        gameMenu.SetActive(true);
    }

    public static void CloseGameMenu()
    {
        GameMenuOpen = false;
        Player.InputDisabled = false;
        Time.timeScale = 1;
        gameMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
