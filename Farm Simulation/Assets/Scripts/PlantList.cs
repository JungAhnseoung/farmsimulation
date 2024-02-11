using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlantList", menuName ="Plant List")]
public class PlantList : ScriptableObject
{
    [SerializeField] public List<PlantInfo> plantInfo;
    
    public PlantInfo GetPlantInfo(int seedNo)
    {
        return plantInfo.Find(x => x.seedNo == seedNo);
    }
}

[System.Serializable]
public class PlantInfo
{

    public GameObject[] agingObject;
    public Sprite[] ageSprite;
    public Sprite farmSprite;
    public Season[] season;

    [ItemNoName]
    public int seedNo;
    [ItemNoName]
    public int plantChangeNo;
    [ItemNoName]
    public int[] toolNo;
    [ItemNoName]
    public int[] goodsNo;

    public int[] age;

    public int[] toolAction;
    public int[] plantMax;
    public int[] plantMin;
    public int reAge;

    public bool disablePlant;
    public bool disablePlantCollider;
    public bool isFarmAnimation;
    public bool isFarmEffect = false;
    public bool generatePlantInInventory;
    public FarmEffectType farmEffectType;

    public bool IsToolUsablePlant(int itemNo)
    {
        return (ToolFarmAction(itemNo) == -1) ? false : true;
    }

    public int ToolFarmAction(int itemNo)
    {
        for (int i = 0; i < toolNo.Length; i++)
        {
            if (toolNo[i] == itemNo) return toolAction[i];
        }
        return -1;
    }
}
