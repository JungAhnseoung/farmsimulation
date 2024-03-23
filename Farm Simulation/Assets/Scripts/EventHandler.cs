using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActionDelegate(float xInput, float yInput, bool isIdle, bool isWalking, bool isRunning, bool isHolding,
    bool idleRight, bool idleLeft, bool idleUp, bool idelDown,
    bool isToolRight, bool isToolLeft, bool isToolUp, bool isToolDown,
    bool isPullRight, bool isPullLeft, bool isPullUp, bool isPullDown,
    bool isWaterRight, bool isWaterLeft, bool isWaterUp, bool isWaterDown,
    bool isReapRight, bool isReapLeft, bool isReapUp, bool isReapDown);

public static class EventHandler
{
    public static event ActionDelegate ActionEvent;

    public static event Action<InventoryType, List<InventoryItem>> InventoryEvent;

    public static event Action<InventoryType, List<InventoryItem>> ChestEvent;

    public static event Action<Season, int, int, string, int, int, int> SeasonPass;

    public static event Action<Season, int, int, string, int, int, int> YearPass;

    public static event Action<Season, int, int, string, int, int, int> DayPass;

    public static event Action<Season, int, int, string, int, int, int> HourPass;

    public static event Action<Season, int, int, string, int, int, int> MinutePass;

    public static event Action BeforeFadeOut;

    public static event Action BeforeUnload;

    public static event Action AfterLoad;

    public static event Action AfterFadeIn;

    public static event Action DropEvent;

    public static event Action RemoveEvent;

    public static event Action<Vector3, FarmEffectType> FarmEffect;

    public static event Action GeneratePlant;
    
    public static void CallSeasonPass(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        if (SeasonPass != null) SeasonPass(season, year, day, weekDay, hour, min, sec);
    }
    public static void CallYearPass(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        if (YearPass != null) YearPass(season, year, day, weekDay, hour, min, sec);
    }
    public static void CallDayPass(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        if (DayPass != null) DayPass(season, year, day, weekDay, hour, min, sec);
    }
    public static void CallHourPass(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        if (HourPass != null) HourPass(season, year, day, weekDay, hour, min, sec);
    }
    public static void CallMinutePass(Season season, int year, int day, string weekDay, int hour, int min, int sec)
    {
        if (MinutePass != null) MinutePass(season, year, day, weekDay, hour, min, sec);
    }

    public static void CallBeforeFadeOut()
    {
        if (BeforeFadeOut != null) BeforeFadeOut();
    }

    public static void CallBeforeUnload()
    {
        if (BeforeUnload != null) BeforeUnload();
    }

    public static void CallAfterLoad()
    {
        if (AfterLoad != null) AfterLoad();
    }

    public static void CallAfterFadeIn()
    {
        if (AfterFadeIn != null) AfterFadeIn();
    }


    public static void CallInventoryEvent(InventoryType inventoryType, List<InventoryItem> inventoryItemList)
    {
        if(InventoryEvent != null) InventoryEvent(inventoryType, inventoryItemList);
    }

    public static void CallChestEvent(InventoryType inventoryType, List<InventoryItem> inventoryItemList)
    {
        if (ChestEvent != null) ChestEvent(inventoryType, inventoryItemList);
    }

    public static void CallActionEvent(float xInput, float yInput, bool isIdle, bool isWalking, bool isRunning, bool isHolding,
    bool idleRight, bool idleLeft, bool idleUp, bool idleDown,
    bool isToolRight, bool isToolLeft, bool isToolUp, bool isToolDown,
    bool isPullRight, bool isPullLeft, bool isPullUp, bool isPullDown,
    bool isWaterRight, bool isWaterLeft, bool isWaterUp, bool isWaterDown,
    bool isReapRight, bool isReapLeft, bool isReapUp, bool isReapDown)
    {
        if (ActionEvent != null)
        {
            ActionEvent(xInput, yInput, isIdle, isWalking, isRunning, isHolding,
            idleRight, idleLeft, idleUp, idleDown,
            isToolRight, isToolLeft, isToolUp, isToolDown,
            isPullRight, isPullLeft, isPullUp, isPullDown,
            isWaterRight, isWaterLeft, isWaterUp, isWaterDown,
            isReapRight, isReapLeft, isReapUp, isReapDown);
        }
    }

    public static void CallDropEvent()
    {
        if(DropEvent != null) DropEvent();
    }

    public static void CallRemoveEvent()
    {
        if(RemoveEvent != null) RemoveEvent();
    }
    public static void CallFarmEffect(Vector3 effectLocation, FarmEffectType farmEffectType)
    {
        if(FarmEffect != null) FarmEffect(effectLocation, farmEffectType);
    }

    public static void CallGeneratePlant()
    {
        if (GeneratePlant != null) GeneratePlant();
    }
}