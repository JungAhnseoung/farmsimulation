using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchOutline : MonoBehaviour
{

    void Start()
    {
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag("TileMapOutline").GetComponent<PolygonCollider2D>();

        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
        cinemachineConfiner.InvalidatePathCache();
    }

}
