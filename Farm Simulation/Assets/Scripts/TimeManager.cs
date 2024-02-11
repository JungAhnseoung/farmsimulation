using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public enum Season
{
   Spring,
   Summer,
   Fall,
   Winter
}
public class TimeManager : MonoBehaviour, Save
{
    public const float secondGameTime = 0.012f;

    private static Season season = Season.Spring;
    private static int year = 1;
    private static int day = 1;
    private static int hour = 6;
    private static int min = 30;
    private static int sec = 0;
    private static string weekDay = "Mon";
    private static bool pause = false;
    private static float tick = 0f;

    private string saveID;
    public string SaveID { get { return saveID; } set { saveID = value; } }
    private SaveObject saveObject;
    public SaveObject SaveObject { get { return saveObject; } set { saveObject = value; } }

    private void OnEnable()
    {
        Register();
    }

    private void OnDisable()
    {
        Unregister();
    }

    void Awake()
    {
        SaveID = GetComponent<GUIDGenerator>().GUID;
        SaveObject = new SaveObject();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.CallMinutePass(season, year, day, weekDay, hour, min, sec);
    }

    // Update is called once per frame
    void Update()
    {
        if(!pause)
        {
            tick += Time.deltaTime;
            if(tick >= secondGameTime)
            {
                tick -= secondGameTime;
                sec++;
                if(sec>59)
                {
                    sec = 0;
                    min++;
                    if(min>59)
                    {
                        min = 0;
                        hour++;
                        if(hour>23)
                        {
                            hour = 0;
                            day++;
                            if(day>30)
                            {
                                day = 1;
                                int gameSeason = (int)season;
                                gameSeason++;

                                season = (Season)gameSeason;
                                if(gameSeason>3)
                                {
                                    gameSeason = 0;
                                    season = (Season)gameSeason;
                                    year++;

                                    EventHandler.CallYearPass(season, year, day, weekDay, hour, min, sec);
                                }
                                EventHandler.CallSeasonPass(season, year, day, weekDay, hour, min, sec);
                            }
                            int days = (((int)season) * 30) + day;
                            int gameWeekDay = days % 7;

                            switch(gameWeekDay)
                            {
                                case 1:
                                    weekDay = "Mon";
                                    break;
                                case 2:
                                    weekDay = "Tue";
                                    break;
                                case 3:
                                    weekDay = "Wed";
                                    break;
                                case 4:
                                    weekDay = "Thu";
                                    break;
                                case 5:
                                    weekDay = "Fri";
                                    break;
                                case 6:
                                    weekDay = "Sat";
                                    break;
                                case 7:
                                    weekDay = "Sun";
                                    break;
                                default:
                                    weekDay = "";
                                    break;
                            }

                            EventHandler.CallDayPass(season, year, day, weekDay, hour, min, sec);
                        }
                        EventHandler.CallHourPass(season, year, day, weekDay, hour, min, sec);
                    }
                    EventHandler.CallMinutePass(season, year, day, weekDay, hour, min, sec);

                    //Debug.Log("year :" + year + "   season :" + season + "    day :" + day + "    hour :" + hour + "  min :" + min);
                }
            }
        }
    }

    public static void TestMin()
    {

        for(int i=0; i<60; i++)
        {
            sec++;
            if (sec > 59)
            {
                sec = 0;
                min++;
                if (min > 59)
                {
                    min = 0;
                    hour++;
                    if (hour > 23)
                    {
                        hour = 0;
                        day++;
                        if (day > 30)
                        {
                            day = 1;
                            int gameSeason = (int)season;
                            gameSeason++;

                            season = (Season)gameSeason;
                            if (gameSeason > 3)
                            {
                                gameSeason = 0;
                                season = (Season)gameSeason;
                                year++;

                                EventHandler.CallYearPass(season, year, day, weekDay, hour, min, sec);
                            }
                            EventHandler.CallSeasonPass(season, year, day, weekDay, hour, min, sec);
                        }
                        int days = (((int)season) * 30) + day;
                        int gameWeekDay = days % 7;

                        switch (gameWeekDay)
                        {
                            case 1:
                                weekDay = "Mon";
                                break;
                            case 2:
                                weekDay = "Tue";
                                break;
                            case 3:
                                weekDay = "Wed";
                                break;
                            case 4:
                                weekDay = "Thu";
                                break;
                            case 5:
                                weekDay = "Fri";
                                break;
                            case 6:
                                weekDay = "Sat";
                                break;
                            case 7:
                                weekDay = "Sun";
                                break;
                            default:
                                weekDay = "";
                                break;
                        }

                        EventHandler.CallDayPass(season, year, day, weekDay, hour, min, sec);
                    }
                    EventHandler.CallHourPass(season, year, day, weekDay, hour, min, sec);
                }
                EventHandler.CallMinutePass(season, year, day, weekDay, hour, min, sec);

                //Debug.Log("year :" + year + "   season :" + season + "    day :" + day + "    hour :" + hour + "  min :" + min);
            }
        }
    
    }

    public static void TestDay()
    {
        for(int i=0; i<86400; i++)
        {
            sec++;
            if (sec > 59)
            {
                sec = 0;
                min++;
                if (min > 59)
                {
                    min = 0;
                    hour++;
                    if (hour > 23)
                    {
                        hour = 0;
                        day++;
                        if (day > 30)
                        {
                            day = 1;
                            int gameSeason = (int)season;
                            gameSeason++;

                            season = (Season)gameSeason;
                            if (gameSeason > 3)
                            {
                                gameSeason = 0;
                                season = (Season)gameSeason;
                                year++;

                                EventHandler.CallYearPass(season, year, day, weekDay, hour, min, sec);
                            }
                            EventHandler.CallSeasonPass(season, year, day, weekDay, hour, min, sec);
                        }
                        int days = (((int)season) * 30) + day;
                        int gameWeekDay = days % 7;

                        switch (gameWeekDay)
                        {
                            case 1:
                                weekDay = "Mon";
                                break;
                            case 2:
                                weekDay = "Tue";
                                break;
                            case 3:
                                weekDay = "Wed";
                                break;
                            case 4:
                                weekDay = "Thu";
                                break;
                            case 5:
                                weekDay = "Fri";
                                break;
                            case 6:
                                weekDay = "Sat";
                                break;
                            case 0:
                                weekDay = "Sun";
                                break;
                            default:
                                weekDay = "";
                                break;
                        }

                        EventHandler.CallDayPass(season, year, day, weekDay, hour, min, sec);
                    }
                    EventHandler.CallHourPass(season, year, day, weekDay, hour, min, sec);
                }
                EventHandler.CallMinutePass(season, year, day, weekDay, hour, min, sec);
            }
        }
    }

    public void Register()
    {
        SaveManager.saveList.Add(this);
    }

    public void Unregister()
    {
        SaveManager.saveList.Remove(this);
    }

    public void Store(string scene)
    {
    
    }

    public void Recover(string scene)
    {
    
    }

    public void Load(SaveGame saveGame)
    {
        if(saveGame.saveObjectDict.TryGetValue(SaveID, out SaveObject saveObject))
        {
            SaveObject = saveObject;
            if(SaveObject.scene.TryGetValue("Base", out SaveScene saveScene))
            {
                if(saveScene.intData != null && saveScene.stringData != null)
                {
                    if (saveScene.stringData.TryGetValue("season", out string sSeason))
                    {
                        if (Enum.TryParse<Season>(sSeason, out Season gSeason)) season = gSeason; 
                    }
                    if (saveScene.intData.TryGetValue("year", out int sYear)) year = sYear;
                    if (saveScene.intData.TryGetValue("day", out int sDAy)) day = sDAy;
                    if (saveScene.stringData.TryGetValue("weekDay", out string sWeekDay)) weekDay = sWeekDay;
                    if (saveScene.intData.TryGetValue("hour", out int sHour)) hour = sHour;
                    if (saveScene.intData.TryGetValue("min", out int sMin)) min = sMin;
                    if (saveScene.intData.TryGetValue("sec", out int sSec)) sec = sSec;
                    tick = 0;
                    EventHandler.CallMinutePass(season, year, day, weekDay, hour, min, sec);
                }
            }
        }
    }

    public SaveObject Save()
    {
        SaveObject.scene.Remove("Base");
        SaveScene saveScene = new SaveScene();
        saveScene.intData = new Dictionary<string, int>();
        saveScene.stringData = new Dictionary<string, string>();

        saveScene.stringData.Add("season", season.ToString());
        saveScene.intData.Add("year", year);
        saveScene.intData.Add("day", day);
        saveScene.stringData.Add("weekDay", weekDay);
        saveScene.intData.Add("hour", hour);
        saveScene.intData.Add("min", min);
        saveScene.intData.Add("sec", sec);
        SaveObject.scene.Add("Base", saveScene);
        return SaveObject;
    }
}
