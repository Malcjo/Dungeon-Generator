using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator
{
    Cell[,] grid;
    Vector2 min;
    Vector2 max;
    private int rotation;

    public int Rotation  { get { return rotation; } } 
    public TileGenerator(Cell[,] Grid, Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
        grid = Grid;
    }
    //NOTE WHEN CHECKING VALUIES REMEMBER DEFAULT IS SET TO FLIPPED
    //placeholder way, I think these could be recursive as well
    public PieceType CheckPiece(Vector2Int position)
    {
        if (IsDoorWayMiddle(position))
        {
            return PieceType.Doorway;
        }
        else if (IsTJunction(position))
        {
            return PieceType.TJunction;
        }
        else if (IsHallwayCorner(position))
        {
            return PieceType.HallwayCorner;
        }
        else if (IsDoorwayOnLeftCorner(position))
        {
            return PieceType.DoorwayLeftCorner;
        }
        else if (IsDoorwayOnRightCorner(position))
        {
            return PieceType.DoorwayRightCorner;
        }
        else if (IsHallway(position))
        {
            return PieceType.Hallway;
        }

        else if (IsCorner(position))
        {
            return PieceType.Corner;
        }
        else if (IsEdge(position))
        {
            return PieceType.Edge;
        }
        else if (IsDeadEnd(position))
        {
            return PieceType.Deadend;
        }
        else if (isCrossCenter(position))
        {
            int[] rotation = new int[] { 0, 90, 180, 270 };
            int index = Random.Range(0, 4);
            this.rotation = rotation[index];
            //grid[x, y].rotation = rotation[index];
            return PieceType.Empty;
        }
        return PieceType.Null;
    }
    //With all of these y-1 is up and y 1 is down due computers calculating xy 0,0 coordinates at the top left of a screen
    // and the orinal json files 
    bool IsPiece(Vector2Int coordinates)
    {
        if (coordinates.x < min.x || coordinates.y < min.y || coordinates.x > max.x - 1 || coordinates.y > max.y - 1)
        {
            //print(coordinates + " Out of bounds");
            return false;
        }
        if (grid[coordinates.x, coordinates.y].content == Contents.Tile)
        {
            //print(coordinates + " tile");
            return true;
        }
        else if ((grid[coordinates.x, coordinates.y].content == Contents.Empty))
        {
            //print(coordinates + " Empty");
            return false;
        }
        //print(coordinates + " Null");
        return false;

    }

    bool IsDoorwayOnLeftCorner(Vector2Int startCoord)
    {
        //Checking if tile is at a corner
        if (IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down) &&
            IsPiece(startCoord + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.left)) // nothin on LEFT
        {
            if (!IsPiece(startCoord + (Vector2Int.left + Vector2Int.up)) && //Checking the TOP Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.right + Vector2Int.up)) &&
                IsPiece(startCoord + Vector2Int.right + Vector2Int.down ))
            {
                rotation = 0;
                return true;
            }
        }
        else if (!IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down) &&
            IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left)) //TOP
        {
            if (!IsPiece(startCoord + (Vector2Int.right + Vector2Int.up)) && //Checking the RIGHT Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.right + Vector2Int.down)) &&
                IsPiece(startCoord + Vector2Int.left+ Vector2Int.down))
            {
                rotation = 90;
                return true;
            }
        }
        else if (IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down) &&
            !IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left)) //RIGHT
        {
            if (!IsPiece(startCoord + (Vector2Int.right + Vector2Int.down)) && //Checking the BOTTOM Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.left + Vector2Int.down))&&
                IsPiece(startCoord + Vector2Int.left + Vector2Int.up))
            {
                rotation = 180;
                return true;
            }
        }
        else if (IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + Vector2Int.down) &&
            IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left)) //BOTTOM
        {
            if (!IsPiece(startCoord + (Vector2Int.left + Vector2Int.up)) && //Checking the LEFT Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.left + Vector2Int.down)) &&
                IsPiece(startCoord + Vector2Int.right + Vector2Int.up))
            {
                rotation = 270;
                return true;
            }
        }
        return false;
    }

    bool IsDoorwayOnRightCorner(Vector2Int startCoord)
    {
        //Checking if tile is at a corner
        if (IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down) &&
            !IsPiece(startCoord + Vector2Int.right) && //RIGHT
            IsPiece(startCoord + Vector2Int.left)) 
        {
            if (!IsPiece(startCoord + (Vector2Int.left + Vector2Int.up)) && //Checking the Bottom Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.right + Vector2Int.up)))
            {
                rotation = 0;
                return true;
            }
        }
        else if (IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + Vector2Int.down) &&//BOTTOM
            IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left)) 
        {
            if (!IsPiece(startCoord + (Vector2Int.right + Vector2Int.up)) && //Checking the RIGHT Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.right + Vector2Int.down)))
            {
                rotation = 90;
                return true;
            }
        }
        else if (IsPiece(startCoord + Vector2Int.up) &&
        IsPiece(startCoord + Vector2Int.down) &&
        IsPiece(startCoord + Vector2Int.right) &&//LEFT
        !IsPiece(startCoord + Vector2Int.left)) 
        {
            if (!IsPiece(startCoord + (Vector2Int.right + Vector2Int.down)) && //Checking the BOTTOM Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.left + Vector2Int.down)))
            {
                rotation = 180;
                return true;
            }
        }
        else if (!IsPiece(startCoord + Vector2Int.up) &&//TOP
        IsPiece(startCoord + Vector2Int.down) &&
        IsPiece(startCoord + Vector2Int.right) &&
        IsPiece(startCoord + Vector2Int.left)) 
        {
            if (!IsPiece(startCoord + (Vector2Int.left + Vector2Int.up)) && //Checking the LEFT Tile has pieces on the left and right
                !IsPiece(startCoord + (Vector2Int.left + Vector2Int.down)))
            {
                rotation = 270;
                return true;
            }
        }
        return false;
    }

    /**
     * Determining if there is a tile on all 4 sides
     *    
     *      X
     *     XxX
     *      X
     */
    bool isCrossCenter(Vector2Int startCoord)
    {
        if (
            IsPiece(startCoord + Vector2Int.left) &&
            IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down)
            )
        {
            //print(startCoord + " True Tile");
            return true;
        }
        //print(startCoord + " Null Tile");
        return false;
    }
    bool IsDoorWayMiddle(Vector2Int coordinates)
    {
        if (isCrossCenter(coordinates))
        {
            if (CheckIfDoorIsNextToHallway(coordinates))
            {
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    bool CheckIfDoorIsNextToHallway(Vector2Int startCoord)
    {
        if (IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + (Vector2Int.up + Vector2Int.left)) &&
            !IsPiece(startCoord + (Vector2Int.up + Vector2Int.right)))
        {
            if (IsPiece(startCoord + Vector2Int.down) &&
                !IsPiece(startCoord + (Vector2Int.down + Vector2Int.left)) &&
                !IsPiece(startCoord + (Vector2Int.down + Vector2Int.right)))
            {
                return false;
            }
            rotation = 0;
            return true;
        }
        else if (IsPiece(startCoord + (Vector2Int.right)) &&
            !IsPiece(startCoord + (Vector2Int.right + Vector2Int.up)) &&
            !IsPiece(startCoord + (Vector2Int.right + Vector2Int.down)))
        {
            if (IsPiece(startCoord + (Vector2Int.left)) &&
            !IsPiece(startCoord + (Vector2Int.left + Vector2Int.up)) &&
            !IsPiece(startCoord + (Vector2Int.left + Vector2Int.down)))
            {
                return false;
            }
            rotation = 90;
            return true;
        }
        else if (IsPiece(startCoord + (Vector2Int.down)) &&
            !IsPiece(startCoord + (Vector2Int.down + Vector2Int.right)) &&
            !IsPiece(startCoord + (Vector2Int.down + Vector2Int.left)))
        {
            if (IsPiece(startCoord + (Vector2Int.up)) &&
                !IsPiece(startCoord + (Vector2Int.up + Vector2Int.right)) &&
                !IsPiece(startCoord + (Vector2Int.up + Vector2Int.left)))
            {
                return false;
            }
            rotation = 180;
            return true;
        }
        else if (IsPiece(startCoord + (Vector2Int.left)) &&
                !IsPiece(startCoord + (Vector2Int.left + Vector2Int.up)) &&
                !IsPiece(startCoord + (Vector2Int.left + Vector2Int.down)))
        {
            if (IsPiece(startCoord + (Vector2Int.right)) &&
                !IsPiece(startCoord + (Vector2Int.right + Vector2Int.up)) &&
                !IsPiece(startCoord + (Vector2Int.right + Vector2Int.down)))
            {
                return false;
            }
            rotation = 270;
            return true;
        }
        return false;
    }
    bool IsHallway(Vector2Int startCoord)
    {
        if (!IsPiece(startCoord + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.left) &&
            IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down))
        {
            rotation = 0;
            return true;
        }
        else if (IsPiece(startCoord + Vector2Int.right) && //Checking Left and Right tiles
            IsPiece(startCoord + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + Vector2Int.down))
        {
            rotation = 90;
            return true;
        }
        return false;
    }
    bool IsCorner(Vector2Int startCoord)
    {
        if (IsPiece(startCoord + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down)) //Top Left x+1 y, x y+1
        {
            this.rotation = 0;
            return true;
        }
        else if (!IsPiece(startCoord + Vector2Int.right) &&
                IsPiece(startCoord + Vector2Int.left) &&
                !IsPiece(startCoord + Vector2Int.up) &&
                IsPiece(startCoord + Vector2Int.down)) //Top Right x y+1, x-1 y
        {
            this.rotation = 90;
            return true;
        }
        else if (!IsPiece(startCoord + Vector2Int.right) &&
                IsPiece(startCoord + Vector2Int.left) &&
                IsPiece(startCoord + Vector2Int.up) &&
                !IsPiece(startCoord + Vector2Int.down))//Bottom Left x-1 y, x y-1
        {
            this.rotation = 180;
            return true;
        }
        else if (IsPiece(startCoord + Vector2Int.right) &&
                !IsPiece(startCoord + Vector2Int.left) &&
                IsPiece(startCoord + Vector2Int.up) &&
                !IsPiece(startCoord + Vector2Int.down))//Bottom Right x y-1, x+1 y
        {
            this.rotation = 270;
            return true;
        }
        return false;
    }

    bool IsHallwayCorner(Vector2Int startCoord)
    {
        if (!IsPiece(startCoord + Vector2Int.up + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.up + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.down + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.down + Vector2Int.left))
        {
            if (IsPiece(startCoord + Vector2Int.right) &&
                !IsPiece(startCoord + Vector2Int.left) &&
                !IsPiece(startCoord + Vector2Int.up) &&
                IsPiece(startCoord + Vector2Int.down)) //Top Left x+1 y, x y+1
            {
                this.rotation = 0;
                return true;
            }
            else if (!IsPiece(startCoord + Vector2Int.right) &&
                    IsPiece(startCoord + Vector2Int.left) &&
                    !IsPiece(startCoord + Vector2Int.up) &&
                    IsPiece(startCoord + Vector2Int.down)) //Top Right x y+1, x-1 y
            {
                this.rotation = 90;
                return true;
            }
            else if (!IsPiece(startCoord + Vector2Int.right) &&
                    IsPiece(startCoord + Vector2Int.left) &&
                    IsPiece(startCoord + Vector2Int.up) &&
                    !IsPiece(startCoord + Vector2Int.down))//Bottom Left x-1 y, x y-1
            {
                this.rotation = 180;
                return true;
            }
            else if (IsPiece(startCoord + Vector2Int.right) &&
                    !IsPiece(startCoord + Vector2Int.left) &&
                    IsPiece(startCoord + Vector2Int.up) &&
                    !IsPiece(startCoord + Vector2Int.down))//Bottom Right x y-1, x+1 y
            {
                this.rotation = 270;
                return true;
                
            }
            return false;
        }

        return false;
    }

    bool IsEdge(Vector2Int startCoord)
    {
        if (IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down))
        {
            rotation = 0;
            return true;
        }
        else if (!IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left) &&
            IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down))
        {
            rotation = 90;
            return true;
        }
        else if (IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left) &&
            IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + Vector2Int.down))
        {
            rotation = 180;
            return true;
        }
        else if (IsPiece(startCoord + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.left) &&
            IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down))
        {
            rotation = 270;
            return true;
        }
        return false;
    }

    bool IsTJunction(Vector2Int startCoord)
    {
        if (!IsPiece(startCoord + Vector2Int.up + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.up + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.down + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.down + Vector2Int.left))
        {
            if (IsPiece(startCoord + Vector2Int.right) &&
                IsPiece(startCoord + Vector2Int.left) &&
                !IsPiece(startCoord + Vector2Int.up) &&
                IsPiece(startCoord + Vector2Int.down))
            {
                rotation = 0;
                return true;
            }
            else if (!IsPiece(startCoord + Vector2Int.right) &&
                IsPiece(startCoord + Vector2Int.left) &&
                IsPiece(startCoord + Vector2Int.up) &&
                IsPiece(startCoord + Vector2Int.down))
            {
                rotation = 90;
                return true;
            }
            else if (IsPiece(startCoord + Vector2Int.right) &&
                IsPiece(startCoord + Vector2Int.left) &&
                IsPiece(startCoord + Vector2Int.up) &&
                !IsPiece(startCoord + Vector2Int.down))
            {
                rotation = 180;
                return true;
            }
            else if (IsPiece(startCoord + Vector2Int.right) &&
                !IsPiece(startCoord + Vector2Int.left) &&
                IsPiece(startCoord + Vector2Int.up) &&
                IsPiece(startCoord + Vector2Int.down))
            {
                rotation = 270;
                return true;
            }
            return false;
        }
        return false;
    }

    bool IsDeadEnd(Vector2Int startCoord)
    {
        if (!IsPiece(startCoord + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.up) &&
            IsPiece(startCoord + Vector2Int.down))
        {
            this.rotation = 0;
            return true;
        }
        else if (!IsPiece(startCoord + Vector2Int.right) &&
            IsPiece(startCoord + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + Vector2Int.down))
        {
            this.rotation = 90;
            return true;
        }
        else if (!IsPiece(startCoord + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.left) &&
            IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + Vector2Int.down))
        {
            this.rotation = 180;
            return true;
        }
        else if (IsPiece(startCoord + Vector2Int.right) &&
            !IsPiece(startCoord + Vector2Int.left) &&
            !IsPiece(startCoord + Vector2Int.up) &&
            !IsPiece(startCoord + Vector2Int.down))
        {
            this.rotation = 270;
            return true;
        }
        return false;
    }
}
