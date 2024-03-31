using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private static Slider slider;
    [SerializeField] private int maxStamina = 100;

    private void OnEnable()
    {
        EventHandler.StaminaEvent += SetStamina;
        EventHandler.DayPass += ResetStamina;
    }

    private void OnDisable()
    {
        EventHandler.StaminaEvent -= SetStamina;
        EventHandler.DayPass -= ResetStamina;
    }


    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
    }

    private void SetStamina(int stamina)
    {
        slider.value = stamina;
    }

    private void ResetStamina(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        slider.value = maxStamina;
        Player.currentStamina = maxStamina;
    }

}
