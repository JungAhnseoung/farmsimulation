using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    up,
    down,
    left,
    right,
    none
}


public static class PlayerMovement
{
    public const float walkingSpeed = 3.0f;
    public const float runningSpeed = 7.0f;
}
