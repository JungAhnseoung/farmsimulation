using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TileAttribute", menuName ="Tile Attributes")]
public class TileAttributes : ScriptableObject
{
    public SceneType sceneType;
    public int height;
    public int width;
    public int maxHeight;
    public int maxWidth;

    [SerializeField]
    public List<TileAttribute> tileAttributes;
}
