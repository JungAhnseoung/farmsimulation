using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject editableGameMenu = null;
    [SerializeField] private InventoryBar inventoryBar = null;

    private static GameObject gameMenu = null;
    private static bool isGameMenuOpen = false;
    public static bool GameMenuOpen { get => isGameMenuOpen; set => isGameMenuOpen = value; }

    private void Awake()
    {
       gameMenu = editableGameMenu;
       gameMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameMenuOpen)
            {
                CloseGameMenu();
            }
            else
            {
                OpenGameMenu();
            }
        }
    }

    public void OpenGameMenu()
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
