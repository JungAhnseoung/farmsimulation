using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class GUIDGenerator : MonoBehaviour
{
    [SerializeField]
    private string guid = "";
    public string GUID
    {
        get => guid;
        set => guid = value;
    }

    private void Awake()
    {
        if(!Application.IsPlaying(gameObject))
        {
            if(guid=="")
            {
                guid = System.Guid.NewGuid().ToString();
            }
        }
    }
}
