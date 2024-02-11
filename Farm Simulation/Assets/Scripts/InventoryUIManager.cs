using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory = null;
    [SerializeField] private InventoryBar inventoryBar = null;
    [SerializeField] private InventoryOpen inventoryOpen = null;
    private static bool isInventoryOpen = false;
    public static bool InventoryOpen {  get => isInventoryOpen; set => isInventoryOpen = value; }
    private void Awake()
    {
        inventory.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(InventoryOpen)
            {
                CloseInventory();
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
            }
        }
    }

    public void OpenInventory()
    {
        InventoryOpen = true;
        Player.InputDisabled = true;
        Time.timeScale = 0;
        inventory.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryOpen.RemoveItemDragged();
        InventoryOpen = false;
        Player.InputDisabled = false;
        Time.timeScale = 1;
        inventory.SetActive(false);
    }
}
