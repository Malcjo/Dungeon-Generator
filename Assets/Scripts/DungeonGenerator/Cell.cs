using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cell borken into smaller classes to make the data easier to manage through
[System.Serializable]
public enum Contents
{
    Empty,
    Tile,
}

[System.Serializable]
public class Cell
{
    public PieceType pieceType;
    public Contents content = new Contents(); //This is to tell whether the cell is empty or has something in it
    public Vector2 position;
    public float rotation = 0;
    public GameObject tileObject;
    public GameObject objectInTile;
    public int prefabNumber = -1;

    public int roomNumber = -1;
    public int roomWidth;
    public int roomLength;

    public Tiles tile;
    public Door door;
    public Notes note;
    public Columns column;
    public Water water;

    public Cell(float x, float y)
    {
        this.position.x = x;
        this.position.y = y;
    }

    public void AddTileData(GameObject tile)
    {
        objectInTile = tile;
    }
    public void SetContents(Contents cont)
    {
        this.content = cont;
    }
    public void newTiles(Vector2 position, bool isRound)
    {
        tile = new Tiles(position, isRound);
    }
    public void newDoor(Vector2 position, int type, Vector2 dir)
    {
        door = new Door(position, type, dir);
    }
    public void NewNotes(string text, string reference, Vector2 pos, Vector2 rawPos, bool exist)
    {
        note = new Notes(text, reference, pos, rawPos, exist);
    }
    public void NewColumn(Vector2 position, bool exist)
    {
        column = new Columns(position, exist);
    }

    public void NewWater(Vector2 position, bool exist)
    {
        water = new Water(position, exist);
    }
}
[System.Serializable]
public class Tiles
{
    public Vector2 position;
    public bool isRound;
    public Tiles(Vector2 position, bool isRound)
    {
        this.position = position;
        this.isRound = isRound;
    }
}
[System.Serializable]
public class Door
{
    public Vector2 position;
    public int type;
    public Direction dir;
    public Door(Vector2 position, int type, Vector2 dir)
    {
        this.position = position;
        this.type = type;
        this.dir = new Direction(dir);
    }
}
[System.Serializable]
public class Direction
{
    public Vector2 dir;
    public Direction(Vector2 dir)
    {
        this.dir = dir;
    }
}
[System.Serializable]
public class Notes
{
    public string text = "";
    public string reference = "";
    public Pos pos;
    public bool exist;

    public Notes(string text, string reference, Vector2 pos, Vector2 rawPos, bool exist)
    {
        this.pos = new Pos(pos, rawPos);
        this.text = text;
        this.reference = reference;
        this.exist = exist;
    }
}
[System.Serializable]
public class Pos
{
    public Vector2 position;
    public Vector2 rawPosition;
    public Pos(Vector2 position, Vector2 rawPosition)
    {
        this.position = position;
        this.rawPosition = rawPosition;
    }
}
[System.Serializable]
public class Columns
{
    public Vector2 position;
    public bool exist;
    public Columns(Vector2 position, bool exist)
    {
        this.position = position;
        this.exist = exist;
    }
}
[System.Serializable]
public class Water
{
    public Vector2 position;
    public bool exist;
    public Water(Vector2 position, bool exist)
    {
        this.position = position;
        this.exist = exist;
    }
}