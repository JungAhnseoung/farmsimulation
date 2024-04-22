using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static bool isItemReady = false;
    private void OnEnable()
    {
        EventHandler.DayPass += SetItemReady;
    }

    private void OnDisable()
    {
        EventHandler.DayPass -= SetItemReady;
    }

    private void SetItemReady(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        isItemReady = true;
    }
}
