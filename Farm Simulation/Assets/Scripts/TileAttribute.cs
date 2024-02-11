using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileBoolean
{
    isPath,
    isBlock,
    isDroppable,
    isDiggable,
    isFurniturable
}

[System.Serializable]
public class TilePosition
{
    public int xPos, yPos;

    public TilePosition(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public static explicit operator Vector2(TilePosition tilePositoin)
    {
        return new Vector2((float)tilePositoin.xPos, (float)tilePositoin.yPos);
    }

    public static explicit operator Vector3(TilePosition tilePosition)
    {
        return new Vector3((float)tilePosition.xPos, (float)tilePosition.yPos, 0f);
    }

    public static explicit operator Vector2Int(TilePosition tilePosition)
    {
        return new Vector2Int(tilePosition.xPos, tilePosition.yPos);
    }

    public static explicit operator Vector3Int(TilePosition tilePosition)
    {
        return new Vector3Int(tilePosition.xPos, tilePosition.yPos, 0);
    }
}

[System.Serializable]
public class TileAttribute
{
    public TilePosition tilePosition;
    public TileBoolean tileBoolean;
    public bool tileBool = false;

    public TileAttribute(TilePosition tilePosition, TileBoolean tileBoolean, bool tileBool)
    {
        this.tilePosition = tilePosition;
        this.tileBoolean = tileBoolean;
        this.tileBool = tileBool;
    }
}

[System.Serializable]
public class TileAttributeDetail
{
    public int x;
    public int y;
    public int seedNo = -1;
    public int age = -1;
    public int ageFarm = -1;
    public int ageDig = -1;
    public int ageWater = -1;
    public bool isPath = false;
    public bool isBlock = false;
    public bool isDroppable = false;
    public bool isDiggable = false;
    public bool isFurniturable = false;

    public TileAttributeDetail()
    {

    }
}
