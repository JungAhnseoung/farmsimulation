using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[ExecuteAlways]
public class TileGridAttributes : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private TileAttributes tileAttributes = null;
    [SerializeField] private TileBoolean tileBoolean = TileBoolean.isPath;
    private Tilemap tilemap;

    private void OnEnable()
    {
        if(!Application.IsPlaying(this.gameObject))
        {
            tilemap = GetComponent<Tilemap>();
            if (tileAttributes != null) tileAttributes.tileAttributes.Clear();
        }
    }

    private void OnDisable()
    {
        if(!Application.IsPlaying(this.gameObject))
        {
            tilemap.CompressBounds();
           
            if(tileAttributes != null)
            {
                Vector3Int min = tilemap.cellBounds.min;
                Vector3Int max = tilemap.cellBounds.max;

                for(int x=min.x; x<max.x; x++)
                {
                    for(int y=min.y; y<max.y; y++)
                    {
                        TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tileBase != null) tileAttributes.tileAttributes.Add(new TileAttribute(new TilePosition(x, y), tileBoolean, true));
                    }
                }
            }
            
            if (tileAttributes != null) EditorUtility.SetDirty(tileAttributes);
        }
    }
#endif
}
