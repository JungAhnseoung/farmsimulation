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

    [SerializeField] private Image timeImg = null;
    [SerializeField] private Image[] timeBar = null; 

    private void OnEnable()
    {
        EventHandler.MinutePass += UpdateTime;
        for(int i =0; i<6; i++)
        {
            timeBar[i].gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        EventHandler.MinutePass -= UpdateTime;
    }

    private void UpdateTime(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        min = min - (min % 10); //show only per 10 min
        string minute;

        timeImg.transform.Rotate(Vector3.forward * 0.25f);
       

        if (hour > 24) hour -= 24;

        if (hour == 8) timeBar[0].gameObject.SetActive(true);
        if (hour == 10) timeBar[1].gameObject.SetActive(true);
        if (hour == 12) timeBar[2].gameObject.SetActive(true);
        if (hour == 16) timeBar[3].gameObject.SetActive(true);
        if (hour == 20) timeBar[4].gameObject.SetActive(true);
        if (hour == 0) timeBar[5].gameObject.SetActive(true);

        if(hour == 6)
        {
            for (int i = 0; i < 6; i++)
            {
                timeBar[i].gameObject.SetActive(false);
            }
        }
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
