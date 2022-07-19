
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//The the some coordinates are created from the JSON file thats parsed through the LevelData class
public class JsonLoader : MonoBehaviour
{
    public LevelData levelData;
    private Vector2Int oldMin;
    private Vector2Int oldMax;
    private Vector2 newMin;
    private Vector2 newMax;
    private Vector2Int offset;
    private string title = "";
    private string story = "";
    private List<CoordData> coordinates;

    public Vector2 GetMin { get { return newMin; } }
    public Vector2 GetMax { get { return newMax; } }
    public string GetTitle { get { return title; } }
    public string GetStroy { get { return story; } }
    public List<CoordData> GetCoordinates { get{ return coordinates; } }

    public string jsonInfoTest = "";
    public void clearAll()
    {
        if(coordinates != null)
            coordinates.Clear();
        title = "";
        story = "";

    }
    public void Intialize(string textJSON)
    {
        //load the text and convert from json into the levelData class
        levelData = JsonUtility.FromJson<LevelData>(textJSON);
        title = levelData.title;
        story = levelData.story;
        coordinates = new List<CoordData>();
        SetUpCoordAndMinMax();
    }
    private void SetUpCoordAndMinMax()
    {
        GetInitialCoordinatess();
        GetInitialMinAndMax();
        SetNewMinAndMax();
        SetNewCoordinates();
    }
    private void GetInitialCoordinatess()
    {
        int roomNumber = 0;
        foreach (LevelData.Rect rect in levelData.rects)
        {
            roomNumber++;
            int x = rect.x;
            int y = rect.y;
            int xLoop = rect.w;
            int yLoop = rect.h;
            for (int i = 0; i < xLoop; i++)
            {
                for (int ii = 0; ii < yLoop; ii++)
                {

                    Vector2Int posiiton = new Vector2Int(x + i, y + ii);
                    CoordData newCoord = new CoordData();
                    newCoord.roomNumber = roomNumber;
                    newCoord.roomWidth = rect.w;
                    newCoord.roomLength = rect.h;
                    newCoord.SetGrid(posiiton);
                    newCoord.isRound = rect.rotunda;
                    coordinates.Add(newCoord);
                }
            }
        }
    }
    private void GetInitialMinAndMax()
    {
        int lowX = 0;
        int highX = 0;
        int lowY = 0;
        int highY = 0;
        foreach (CoordData rect in coordinates)
        {
            if (rect.gridPosition.x <= lowX)
                lowX = rect.gridPosition.x;
            else if (rect.gridPosition.x >= highX)
                highX = rect.gridPosition.x;
            if (rect.gridPosition.y <= lowY)
                lowY = rect.gridPosition.y;
            else if (rect.gridPosition.y >= highY)
                highY = rect.gridPosition.y;
        }
        oldMin.x = lowX;
        oldMin.y = lowY;
        oldMax.x = highX;
        oldMax.y = highY;
        offset = new Vector2Int((1 * Mathf.Abs(oldMin.x)), (1 * Mathf.Abs(oldMin.y)));
    }

    private void SetNewMinAndMax()
    {
        float x = (1 * Mathf.Abs(oldMin.x));
        float z = (1 * Mathf.Abs(oldMin.y));

        int newMaxX = (int)(oldMax.x + (x));
        int newMaxY = (int)(oldMax.y + (z));

        newMin.x = 0;
        newMin.y = 0;
        newMax.x = newMaxX + 1;
        newMax.y = newMaxY + 1;
    }
    private void SetNewCoordinates()
    {
        #region Looping through coordinate list
        foreach (CoordData coord in coordinates)
        {
            #region Adding door Data to coordinates
            foreach (LevelData.Door door in levelData.doors)
            {
                if (door.x == coord.gridPosition.x &&
                    door.y == coord.gridPosition.y)
                {
                    Vector2 doorPosition = new Vector2(door.x += offset.x, door.y += offset.y);
                    Vector2 doorDirection = new Vector2(door.dir.x, door.dir.y);
                    coord.newDoor(doorPosition, door.type += 1, doorDirection);
                    // up the values by one as the jsonUtility seems to create a class and default all to 0
                    //make the door type value above 0 so I can check whether it's 1 or higher that means it is a door that was from the json file
                }
                else
                {
                    //Some of the positions repeat so I need to check so I don't override the type with the negative
                    if (coord.door != null)
                    {
                        if (coord.door.type < 1)
                        {
                            coord.newDoor(-Vector2.zero, -1, -Vector2.zero);
                        }
                    }
                    else
                    {
                        coord.newDoor(-Vector2.zero, -1, -Vector2.zero);
                    }
                }
            }
            #endregion

            #region Adding note data to coordinate list 

            /*Issue with original note JSON file
            //Notes has a parameter call 'ref' that keeps track of note number
            //Cannot transfer ref into Unity as ref is an inbuilt function in the C# programming langauge
            //And in order to transfer the data across using JsonUtility
            the class that it is transfering to must have the exact same name spelling for it's variables*/

            foreach (LevelData.Notes notes in levelData.notes)
            {


                int posx = Mathf.RoundToInt(notes.pos.x);
                int posy = Mathf.RoundToInt(notes.pos.y);

                Vector2 posiiton = new Vector2(posx, posy);
                if (posiiton.x == coord.gridPosition.x &&
                    posiiton.y == coord.gridPosition.y)
                {
                    //print("They line up!");

                    Vector2 pos = new Vector2(posx += offset.x, posy += offset.y);
                    Vector2 rawPos = new Vector2(notes.pos.x += offset.x, notes.pos.y += offset.y);

                    coord.NewNotes(notes.text, notes.reference, pos, rawPos, true);
                }
                else
                {
                    if (coord.note != null)
                    {
                        if (!coord.note.text.Equals(""))
                        {
                            continue;
                        }
                    }
                    coord.NewNotes("", "", Vector2.zero, Vector2.zero, false);
                }
            }

            #endregion

            #region Adding colum data to coordinate list
            foreach (LevelData.Columns column in levelData.columns)
            {
                if (column.x == coord.gridPosition.x &&
                    column.y == coord.gridPosition.y)
                {
                    Vector2 columnPosition = new Vector2(column.x += offset.x, column.y += offset.y);
                    coord.NewColumn(columnPosition, true);
                }
                else
                {
                    coord.NewColumn(Vector2.zero, false);
                    coord.column = null;
                }
            }
            #endregion

            #region Adding water data to coordinate list
            foreach (LevelData.Water water in levelData.water)
            {
                if (water.x == coord.gridPosition.x &&
                    water.y == coord.gridPosition.y)
                {
                    Vector2 waterPosition = new Vector2(water.x += offset.x, water.y += offset.y);
                    coord.NewWater(waterPosition, true);
                }
                else
                {
                    coord.water = null;
                    //coord.NewWater(Vector2.zero, false);
                }
            }
            #endregion

            //Applying offset to coordinate gridposition after the positions are used to match up placement with original coordinates
            coord.gridPosition.x += offset.x;
            coord.gridPosition.y += offset.y;
            coord.newTiles(coord.gridPosition, coord.isRound);
        }
        
        // reset value back to match the value from the original json file
        foreach (CoordData coord in coordinates)
        {
            coord.door.type -= 1;
        }
        #endregion
    }
}
