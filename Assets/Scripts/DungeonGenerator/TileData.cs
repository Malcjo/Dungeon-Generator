using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RoomPrefabs
{
    public List<GameObject> SquareEmptyPieceGroup;
    public List<GameObject> SquareEdgePieceGroup;
    public List<GameObject> SquareCornerPieceGroup;
    public List<GameObject> SquareHallwayPieceGroup;
    public List<GameObject> SquareDoorwayPieceGroup;
    public List<GameObject> SquareDoorwayLeftCornerPieceGroup;
    public List<GameObject> SquareDoorwayRightCornerPieceGroup;
    public List<GameObject> SquareDeadendPieceGroup;
    public List<GameObject> SquareHallwayCornerPieceGroup;
    public List<GameObject> SqaureTJunctionPieceGroup;
}

[System.Serializable]
public class TileData : MonoBehaviour
{
    public RoomPrefabs[] roomPrefabs;
    public PieceType type;

    private GameObject SquareEmptyPiece;
    private GameObject SquareEdgePiece;
    private GameObject SquareCornerPiece;
    private GameObject SquareHallwayPiece;
    private GameObject SquareDoorwayPiece;
    private GameObject SquareDoorwayLeftCornerPiece;
    private GameObject SquareDoorwayRightCornerPiece;
    private GameObject SquareDeadendPiece;
    private GameObject SquareHallwayCornerPiece;
    private GameObject SquareTJunctionPiece;
    private GameObject tileMesh;
    public GameObject nullObj;

    public GameObject pieceToSpawn;
    public float rotation = 0;
    public Vector2Int tilePosition;
    private Vector2 minPosition;
    private Vector2 maxPosition;
    public bool isRound;
    public int roomType;
    private int prefabNum = 0;
    public void Initialize(Cell cell, int roomNum)
    {
        int num = roomNum - 1;
        FigureOutRoomTheme(cell, num);
        FigureOutPieceGroup();
        SpawnMesh();
        cell.prefabNumber = prefabNum;
    }
    private void SpawnMesh()
    {
        tileMesh = Instantiate(pieceToSpawn, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(0, rotation, 0)) as GameObject;
        tileMesh.transform.parent = this.transform;
    }

    private void FigureOutRoomTheme(Cell cell, int roomNum)
    {
        roomType = roomNum;
    }
    private void FigureOutPieceGroup()
    {
        if (!isRound) // Bool setup for if you want to incorporate round rooms
        {
            switch (type)
            {
                case PieceType.Empty:
                    int num = Random.Range(0, roomPrefabs[roomType].SquareEmptyPieceGroup.Count);
                    SquareEmptyPiece = roomPrefabs[roomType].SquareEmptyPieceGroup[num];
                    pieceToSpawn = SquareEmptyPiece;
                    prefabNum = num;
                    break;
                case PieceType.Corner:
                    int num2 = Random.Range(0, roomPrefabs[roomType].SquareCornerPieceGroup.Count);
                    SquareCornerPiece = roomPrefabs[roomType].SquareCornerPieceGroup[num2];
                    pieceToSpawn = SquareCornerPiece;
                    prefabNum = num2;
                    break;
                case PieceType.Edge:
                    int num3 = Random.Range(0, roomPrefabs[roomType].SquareEdgePieceGroup.Count);
                    SquareEdgePiece = roomPrefabs[roomType].SquareEdgePieceGroup[num3];
                    pieceToSpawn = SquareEdgePiece;
                    prefabNum = num3;
                    break;
                case PieceType.Hallway:
                    int num4 = Random.Range(0, roomPrefabs[roomType].SquareHallwayPieceGroup.Count);
                    SquareHallwayPiece = roomPrefabs[roomType].SquareHallwayPieceGroup[num4];
                    pieceToSpawn = SquareHallwayPiece;
                    prefabNum = num4;
                    break;
                case PieceType.Doorway:
                    int num5 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayPieceGroup.Count);
                    SquareDoorwayPiece = roomPrefabs[roomType].SquareDoorwayPieceGroup[num5];
                    pieceToSpawn = SquareDoorwayPiece;
                    prefabNum = num5;
                    break;
                case PieceType.DoorwayLeftCorner:
                    int num6 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayLeftCornerPieceGroup.Count);
                    SquareDoorwayLeftCornerPiece = roomPrefabs[roomType].SquareDoorwayLeftCornerPieceGroup[num6];
                    pieceToSpawn = SquareDoorwayLeftCornerPiece;
                    prefabNum = num6;
                    break;
                case PieceType.DoorwayRightCorner:
                    int num7 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayRightCornerPieceGroup.Count);
                    SquareDoorwayRightCornerPiece = roomPrefabs[roomType].SquareDoorwayRightCornerPieceGroup[num7];
                    pieceToSpawn = SquareDoorwayRightCornerPiece;
                    prefabNum = num7;
                    break;
                case PieceType.Deadend:
                    int num8 = Random.Range(0, roomPrefabs[roomType].SquareDeadendPieceGroup.Count);
                    SquareDeadendPiece = roomPrefabs[roomType].SquareDeadendPieceGroup[num8];
                    pieceToSpawn = SquareDeadendPiece;
                    prefabNum = num8;
                    break;
                case PieceType.HallwayCorner:
                    int num9 = Random.Range(0, roomPrefabs[roomType].SquareHallwayCornerPieceGroup.Count);
                    SquareHallwayCornerPiece = roomPrefabs[roomType].SquareHallwayCornerPieceGroup[num9];
                    pieceToSpawn = SquareHallwayCornerPiece;
                    break;
                case PieceType.TJunction:
                    int num10 = Random.Range(0, roomPrefabs[roomType].SqaureTJunctionPieceGroup.Count);
                    SquareTJunctionPiece = roomPrefabs[roomType].SqaureTJunctionPieceGroup[num10];
                    pieceToSpawn = SquareTJunctionPiece;
                    break;
                case PieceType.Null:
                    pieceToSpawn = nullObj;
                    prefabNum = -1;
                    break;
            }
        }
    }
}