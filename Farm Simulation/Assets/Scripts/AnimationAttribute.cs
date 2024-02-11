using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="AnimationAttributes", menuName ="Animation Attritbues")]
public class AnimationAttribute : ScriptableObject
{
    public AnimationName animationName;
    public CharacterAnimationAttribute characterAnimationAttr;
    public TypeAnimationAttribute typeAnimationAttr;
    public AnimationClip animationClip;
}

[System.Serializable]
public struct CharacterDetailAttribute
{
    public CharacterAnimationAttribute characterAnimationAttr;
    public TypeAnimationAttribute typeAnimationAttr;

    public CharacterDetailAttribute(CharacterAnimationAttribute characterAnimationAttr, TypeAnimationAttribute typeAnimationAttr)
    {
        this.characterAnimationAttr = characterAnimationAttr;
        this.typeAnimationAttr = typeAnimationAttr;
    }
}

public enum AnimationName
{
    IdleUp,
    IdleDown,
    IdleRight,
    IdleLeft,
    WalkUp,
    WalkDown,
    WalkLeft,
    WalkRight,
    RunUp,
    RunDown,
    RunLeft,
    RunRight,
    ToolUp,
    ToolDown,
    ToolLeft,
    ToolRight,
    ReapUp,
    ReapDown,
    ReapLeft,
    ReapRight,
    PullUp,
    PullDown,
    PullLeft,
    PullRight,
    WaterUp,
    WaterDown,
    WaterLeft,
    WaterRight,
    HoldUp,
    HoldDown,
    HoldLeft,
    HoldRight
}

public enum CharacterAnimationAttribute
{
    Arm,
    Body,
    Tool
}

public enum TypeAnimationAttribute
{
    Hold,
    Axe,
    Pickaxe,
    Scythe,
    WateringCan,
    Hoe,
    None
}
