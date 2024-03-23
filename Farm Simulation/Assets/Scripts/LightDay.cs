using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="DayLight", menuName ="DayLight")]
public class LightDay : ScriptableObject
{
    public LightStrength[] lightStrengths;
}

[System.Serializable]
public struct LightStrength
{
    public Season season;
    public float brightness;
    public int time;

}
