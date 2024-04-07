using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EatUI : MonoBehaviour
{
    public  static TextMeshProUGUI eatText = null;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        eatText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Eat()
    {
        Player.currentStamina += 20;
        if (Player.currentStamina > 100) Player.currentStamina = 100;
        EventHandler.CallStaminaEvent(Player.currentStamina);
        this.gameObject.SetActive(false);
    }

    public void NotEat()
    {
        this.gameObject.SetActive(false);
    }
}
