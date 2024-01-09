using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private int itemNo;
    public int ItemNo 
    { 
        get 
        { return itemNo; } 
        set 
        { itemNo = value; } 
    }

}
