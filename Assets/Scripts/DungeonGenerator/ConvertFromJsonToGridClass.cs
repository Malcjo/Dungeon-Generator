using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConvertFromJsonToGridClass
{
    public string title;
    public string story;
    public Grid[] grid;
    [System.Serializable]
    public class Grid
    {
        public PieceType pieceType;
        public Contents content;
        public Vector2 position;
        public float rotation;
        public GameObject tilePiece;
        public int prefabNumber;

        public int roomNumber;
        public int roomWidth;
        public int roomLength;

        public Tiles tile;
        public Door door;
        public Notes note;
        public Columns column;
        public Water water;
    }
}
