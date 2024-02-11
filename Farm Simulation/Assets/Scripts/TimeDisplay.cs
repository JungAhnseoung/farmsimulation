using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private Image seasonImg = null;
    [SerializeField] private TextMeshProUGUI yearTxt = null;
    [SerializeField] private TextMeshProUGUI dateTxt = null;
    [SerializeField] private TextMeshProUGUI timeTxt = null;

    [SerializeField] private Sprite spring = null;
    [SerializeField] private Sprite summer = null;
    [SerializeField] private Sprite fall = null;
    [SerializeField] private Sprite winter = null;

    private void OnEnable()
    {
        EventHandler.MinutePass += UpdateTime;
    }

    private void OnDisable()
    {
        EventHandler.MinutePass -= UpdateTime;
    }

    private void UpdateTime(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        min = min - (min % 10); //show only per 10 min
        string minute;

        if (hour > 24) hour -= 24;

        if (min < 10) minute = "0" + min.ToString();
        else minute= min.ToString();

        string timeDp = hour.ToString() + " : " + minute;

        switch(season)
        {
            case Season.Spring:
                seasonImg.sprite = spring;
                break;
            case Season.Summer:
                seasonImg.sprite = summer;
                break;
            case Season.Fall:
                seasonImg.sprite = fall;
                break;
            case Season.Winter:
                seasonImg.sprite = winter;
                break;
        }

        yearTxt.SetText("Year " + year);
        dateTxt.SetText(weekDay + ". " + day.ToString());
        timeTxt.SetText(timeDp);
    }
}
