using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EatUI : MonoBehaviour
{
    public static TextMeshProUGUI eatText = null;
    public static int increaseStamina = 0;
    public static int itemNo;
    private void Awake()
    {
        this.gameObject.SetActive(false);
        eatText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Eat()
    {
        Player.currentStamina += increaseStamina;
        InventoryManager.RemoveItemFromInventory(InventoryType.Player, itemNo);
        Debug.Log(increaseStamina);
        if (Player.currentStamina > 100) Player.currentStamina = 100;
        EventHandler.CallStaminaEvent(Player.currentStamina);
        this.gameObject.SetActive(false);
    }

    public void NotEat()
    {
        this.gameObject.SetActive(false);
    }
}
