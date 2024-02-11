using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchOutline : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.AfterLoad += SwapOutline; 
    }

    private void OnDisable()
    {
        EventHandler.AfterLoad -= SwapOutline;
    }

    private void SwapOutline()
    {
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag("Outline").GetComponent<PolygonCollider2D>();

        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
        cinemachineConfiner.InvalidatePathCache();
    }

}
