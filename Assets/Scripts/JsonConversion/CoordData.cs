using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used for storing data from the JSON file when creating coordinates for a grid
[System.Serializable]
public class CoordData
{
    public RawTiles tile;
    public RawDoor door;
    public RawNotes note;
    public RawColumns column;
    public RawWater water;
    public bool isRound;
    public Vector2Int gridPosition;
    public int roomNumber;
    public int roomWidth;
    public int roomLength;

    public void SetGrid(Vector2Int position)
    {
        gridPosition = position;
    }
    public void newTiles(Vector2 position, bool isRound)
    {
        tile = new RawTiles(position, isRound);
    }
    public void newDoor(Vector2 position, int type, Vector2 dir)
    {
        door = new RawDoor(position, type, dir);
    }
    public void NewNotes(string text, string reference, Vector2 pos, Vector2 rawPos, bool exist)
    {
        note = new RawNotes(text, reference, pos, rawPos, exist);
    }
    public void NewColumn(Vector2 position, bool exist)
    {
        column = new RawColumns(position, exist);
    }
    public void NewWater(Vector2 position, bool exist)
    {
        water = new RawWater(position, exist);
    }
}
[System.Serializable]
public class RawTiles
{
    public Vector2 position;
    public bool isRound;
    public RawTiles(Vector2 position, bool isRound)
    {
        this.position = position;
        this.isRound = isRound;
    }
}
[System.Serializable]
public class RawDoor
{
    public Vector2 position;
    public int type;
    public RawDirection dir;
    public RawDoor(Vector2 position, int type, Vector2 dir)
    {
        this.position = position;
        this.type = type;
        this.dir = new RawDirection(dir);
    }
}
[System.Serializable]
public class RawDirection
{
    public Vector2 dir;
    public RawDirection(Vector2 dir)
    {
        this.dir = dir;
    }
}
[System.Serializable]
public class RawNotes
{
    public string text = "";
    public string reference = "";
    public RawPos pos;
    public bool exist;

    public RawNotes(string text, string reference, Vector2 pos, Vector2 rawPos, bool exist)
    {
        this.pos = new RawPos(pos, rawPos);
        this.text = text;
        this.reference = reference;
        this.exist = exist;
    }
}
[System.Serializable]
public class RawPos
{
    public Vector2 position;
    public Vector2 rawPosition;
    public RawPos(Vector2 position, Vector2 rawPosition)
    {
        this.position=position;
        this.rawPosition=rawPosition;
    }
}
[System.Serializable]
public class RawColumns
{
    public Vector2 position;
    public bool exist;
    public RawColumns(Vector2 position, bool exist)
    {
        this.position = position;
        this.exist = exist;
    }
}
[System.Serializable]
public class RawWater
{
    public Vector2 position;
    public bool exist;
    public RawWater(Vector2 position, bool exist)
    {
        this.position = position;
        this.exist = exist;
    }
}
