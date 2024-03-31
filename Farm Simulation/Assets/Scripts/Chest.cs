using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject chest = null;
    [SerializeField] private Sprite closeChestSprite = null;
    [SerializeField] private Sprite openChestSprite = null;
    private Player player = null;
    private static bool isChestOpen = false;

    public static bool ChestOpen { get => isChestOpen; set => isChestOpen = value; }

    private void OnMouseDown()
    {
        if(!ChestOpen && IsPlayerClose())
        {
            OpenChest();
        }

    }
    void Awake()
    {
        player = FindObjectOfType<Player>();
        chest.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(ChestOpen)
            {
                CloseChest();
            }
        }
    }

    private bool IsPlayerClose()
    {
        Vector3 playerLocation = player.GetCenter();
        Vector3 chestLocation = this.transform.position;
        float distance = Vector3.Distance(playerLocation, chestLocation);
        return (distance > 2f) ? false : true;
    }

    public void OpenChest()
    {
        ChestOpen = true;
        Player.InputDisabled = true;
        chest.SetActive(true);
        this.GetComponent<SpriteRenderer>().sprite = openChestSprite;
    }

    public void CloseChest()
    {
        ChestOpen = false;
        Player.InputDisabled = false;
        chest.SetActive(false);
        this.GetComponent<SpriteRenderer>().sprite = closeChestSprite;
        InventoryUIManager.CloseInventory();
    }
}
