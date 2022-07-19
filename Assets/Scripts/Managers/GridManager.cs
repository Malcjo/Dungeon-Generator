using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum infoTag
{
    LevelDisplay,
    NoteDisplay,
}

public class GridManager : MonoBehaviour
{

    private TileGenerator tileCalculator;
    public UIManager uiManager;

    [SerializeField] private GameObject defaultCamera;
    [SerializeField] private GameObject worldGrp;


    [Header("Grid INFO")]
    private bool flip = false;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;
    private Cell[,] grid;
    private Cell[] Grid1D;
    [SerializeField] private List<Cell> ListOfTiles = new List<Cell>();//Cannot make a 2dArray visble in the inspector so use this list instead
    private List<Cell> NumEdgePieces = new List<Cell>();
    [SerializeField] private float offset;

    [SerializeField] private GameObject tile;


    [Header("PLAYER INFO")]
    public bool playerSpawned = false;
    public bool isFPSPlayer = true;
    private GameObject playerOBJ;
    public GameObject FPSPlayerOBJ;
    public GameObject TopDownPlayerOBJ;
    private GameObject player;
    [SerializeField] private Vector3 playerStartCoord;
    [SerializeField] private List<TileData> SpawnLocations = new List<TileData>();
    [SerializeField] private List<GameObject> specialDoorList = new List<GameObject>();
    [SerializeField] private Vector2Int playerStartGrid;


    [Header("Objects to spawn")]
    [SerializeField] private GameObject doorObj;
    [SerializeField] private GameObject levelInfoObj;
    [SerializeField] private bool SpawnInNotes = false;
    [SerializeField] private GameObject displayNotesObj;
    [SerializeField] private GameObject ChestObj;
    [SerializeField] private GameObject speicalDoorObj;

    [Header("Rooms")]
    public string title;
    public string story;
    //When add to the array group in the tile prefab remeber to change this number to match the size
    public int NumberOfDifferentRoomTypes;
    private Dictionary<int, int> roomType = new Dictionary<int, int>();


    [Header("DEBUG")]
    [SerializeField] private string JsonText = "";
    [SerializeField] private bool showGrid = false;
    [SerializeField] private bool showEmptySpace = false;
    [SerializeField] private GameObject gridNumber;
    [SerializeField] private List<int> VisualizernumberOfRooms = new List<int>();
    private HashSet<int> numberOfRooms = new HashSet<int>();

    [SerializeField] private bool infoRoomBeenPlaced = false;
    [SerializeField] private int playerStatingRoomNumber;
    [SerializeField] private GameObject emptySpace;


    public void Start()
    {
        CheckWhatPlayerType();
    }

    #region Check whether FPSPlayer or TopDownPlayer
    private void CheckWhatPlayerType()
    {
        if (isFPSPlayer)
        {
            playerOBJ = FPSPlayerOBJ;
        }
        else
        {
            playerOBJ = TopDownPlayerOBJ;
        }
    }
    #endregion

    public void SetTextAsset(string asset)
    {
        JsonText = asset;
    }

    #region Load Json and setup data
    public void LoadJsonFile(string asset)
    {
        DestroyDungeon();
        JsonText = "";
        try
        {
            ConvertFromJsonToGridClass data = JsonUtility.FromJson<ConvertFromJsonToGridClass>(asset);
            try
            {
                JsonText = asset;
                title = data.title;
                story = data.story;

                Grid1D = new Cell[data.grid.Length];
                for (int i = 0; i < data.grid.Length; i++)
                {
                    if (!numberOfRooms.Contains(data.grid[i].roomNumber))
                    {
                        numberOfRooms.Add(data.grid[i].roomNumber);
                        VisualizernumberOfRooms.Add((int)data.grid[i].roomNumber);
                    }
                    Cell newCell = new Cell(data.grid[i].position.x, data.grid[i].position.y);
                    newCell.pieceType = data.grid[i].pieceType;
                    newCell.content = data.grid[i].content;
                    newCell.position = data.grid[i].position;
                    newCell.rotation = data.grid[i].rotation;
                    newCell.objectInTile = data.grid[i].tilePiece;

                    newCell.roomNumber = data.grid[i].roomNumber;
                    newCell.roomWidth = data.grid[i].roomWidth;
                    newCell.roomLength = data.grid[i].roomLength;

                    newCell.tile = data.grid[i].tile;
                    newCell.door = data.grid[i].door;
                    newCell.note = data.grid[i].note;
                    newCell.column = data.grid[i].column;
                    newCell.water = data.grid[i].water;

                    Grid1D[i] = newCell;
                }
                FigureOutMinMax(Grid1D);
                grid = new Cell[(int)max.x, (int)max.y];
                ConvertTo2DArray(data);
                SetUpRoomTypes();
                Createtile();
            }
            catch
            {
                print("Failed to generate tiles");
            }

        }
        catch
        {
            print("Not correct Json format!");
        }
    }
    public void SetUpRoomTypes()
    {
        foreach(int num in numberOfRooms)
        {
            print("room type " +num);
        }
        //HashSet<int> temp = new HashSet<int>();
        for (int i = 0; i < numberOfRooms.Count + 1; i++)
        {
            int randomNum;
            randomNum = Random.Range(1, NumberOfDifferentRoomTypes +1);
            roomType.Add(i, randomNum);
        }
    }
    private void ConvertTo2DArray(ConvertFromJsonToGridClass data)
    {
        foreach (ConvertFromJsonToGridClass.Grid newGrid in data.grid)
        {
            int tileX = (int)newGrid.position.x;
            int tileY = (int)newGrid.position.y;
            for (int x = 0; x < max.x; x++)
            {
                for (int y = 0; y < max.y; y++)
                {
                    if (tileX == x && tileY == y)
                    {
                        Cell newCell = new Cell(newGrid.position.x, newGrid.position.y);
                        newCell.pieceType = newGrid.pieceType;
                        newCell.content = newGrid.content;
                        newCell.position = newGrid.position;
                        newCell.rotation = newGrid.rotation;
                        newCell.objectInTile = newGrid.tilePiece;

                        newCell.roomNumber = newGrid.roomNumber;
                        newCell.roomWidth = newGrid.roomWidth;
                        newCell.roomLength = newGrid.roomLength;

                        newCell.tile = newGrid.tile;
                        newCell.door = newGrid.door;
                        newCell.note = newGrid.note;
                        newCell.column = newGrid.column;
                        newCell.water = newGrid.water;

                        grid[x, y] = newCell;
                    }
                }
            }
        }
    }

    private void FigureOutMinMax(Cell[] newGrid)
    {
        float lowX = 0;
        float highX = 0;
        float lowY = 0;
        float highY = 0;
        foreach (Cell grid in newGrid)
        {
            if (grid.position.x <= lowX)
                lowX = grid.position.x;
            else if (grid.position.x >= highX)
                highX = grid.position.x;
            if (grid.position.y <= lowY)
                lowY = grid.position.y;
            else if (grid.position.y >= highY)
                highY = grid.position.y;
        }
        min.x = lowX;
        max.x = highX + 1;
        min.y = lowY;
        max.y = highY + 1;
    }

    #endregion

    #region Resetting functions
    private void DestroyPlayer()
    {
        print("Destroy player");
        defaultCamera.SetActive(true);
        Destroy(player.gameObject);
        playerSpawned = false;
    }

    public void BackToMenu()
    {
        if (playerSpawned)
        {
            DestroyPlayer();
        }
    }
    //need to make sure all values etc are cleared
    public void DestroyDungeon()
    {
        GameObject World;
        if (World = GameObject.FindGameObjectWithTag("World_Grp"))
        {
            specialDoorList.Clear();
            numberOfRooms.Clear();
            VisualizernumberOfRooms.Clear();
            ListOfTiles.Clear();
            playerStatingRoomNumber = 0;
            roomType.Clear();
            NumEdgePieces.Clear();
            SpawnLocations.Clear();
            World = GameObject.FindGameObjectWithTag("World_Grp");
            tileCalculator = null;
            //loader = null;
            this.min = Vector2.zero;
            this.max = Vector2.zero;
            playerStartCoord = Vector3.zero;
            grid = null;
            playerSpawned = false;
            Destroy(World.gameObject);
        }
    }

    #endregion

    #region Creating tiles
    public void Createtile()
    {
        print("Create tile call");
        worldGrp = new GameObject();
        worldGrp.tag = "World_Grp";
        worldGrp.name = "World_Grp";
        float yOffset = 0;
        int flipDirection = 1;
        if (flip)
        {
            yOffset = max.y;
            flipDirection = -1;
        }
        tileCalculator = new TileGenerator(grid, min, max);

        #region Debugging
        //Debug to show grid value
        if (showGrid)
        {
            DisplayGridValues(yOffset, flipDirection);
        }
        if (showEmptySpace)
        {
            DisplayEmptyGrid(yOffset, flipDirection);
        }
        #endregion

        //Creating the tiles in the grid
        foreach (Cell cell in grid)
        {
            if (cell != null)
            {
                if (cell.content == Contents.Tile)
                {
                    if (cell.tile.isRound)
                    {
                        //For if want to spawn in round pieces, hasn't been tested for a while
                        //InstantiateTilePiece(cell, yOffset, flipDirection, true);
                    }
                    else
                    {
                        InstantiateTilePiece(cell, yOffset, flipDirection, false);
                    }
                }
                #region adding note data
                //Spawn in notes from JSON file, these only really work if the json notes name is changed from "ref" to "reference"
                //Creator of the generator mentioned will add another parameter and add the note data into that, so just need to update to add that in
                if (SpawnInNotes)
                {
                    //if the string on the cell has something on it then instantiate a displaynote object
                    if (!cell.note.text.Equals(""))
                    {
                        //print("Note Text " + cell.note.text);
                        GameObject levelInfo = Instantiate(displayNotesObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                        levelInfo.GetComponent<InfoDisplayTrigger>().SetUpInfo(cell.note.text, infoTag.LevelDisplay);
                        levelInfo.transform.SetParent(worldGrp.transform);
                        cell.AddTileData(levelInfo);
                    }
                }
                #endregion

                #region Adding door data
                //Instantiate a door into the grid
                //This is done after all of the piece types have been spawned in
                if (cell.door.type > 0)
                {
                    bool createDoor = false;
                    GameObject newDoor = null;
                    //According to the creator of the Json generator door type 3 are both the front and backdoor of the dungeon
                    //This is assuming there is only going to be 2
                    if (cell.door.type == 3)
                    {
                        print("door type 3");
                        newDoor = Instantiate(speicalDoorObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                        SpecialDoorTrigger trigger = newDoor.GetComponent<SpecialDoorTrigger>();
                        trigger.offset = offset;
                        trigger.yOffset = yOffset; ;
                        trigger.flipDirection = flipDirection;
                        trigger.positionInt = new Vector2Int((int)cell.position.x, (int)cell.position.y);
                        trigger.positiion = new Vector3(cell.position.x * offset, 3.5f, ((cell.position.y * flipDirection) + yOffset) * offset);
                        trigger.rotation = cell.rotation;
                        print(trigger.positionInt);
                        createDoor = true;
                    }
                    if(!createDoor)
                        newDoor = Instantiate(doorObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                    //newDoor = Instantiate(doorObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                    
                    if (cell.door.dir.dir == new Vector2(1, 0) || cell.door.dir.dir == new Vector2(-1, 0))//splt between which direction the door is facing, the offset position is applied to either x or z depending
                    {
                        //offsets the doors if they are in a deadend tile piece
                        if(cell.pieceType == PieceType.Deadend)
                        {
                            float doorOffset = 0;
                            if(cell.door.dir.dir == new Vector2(1, 0))
                            {
                                if (cell.door.type == 3)
                                {
                                    SpecialDoorTrigger trigger = newDoor.GetComponent<SpecialDoorTrigger>();
                                    if (cell.rotation == 270)
                                        trigger.doorOffset = -2.5f;
                                    if (cell.rotation == 90)
                                        trigger.doorOffset = 2.5f;
                                }
                                else
                                {
                                    if (cell.rotation == 270)
                                        doorOffset = -2.5f;
                                    if (cell.rotation == 90)
                                        doorOffset = 2.5f;
                                }
                            }
                            else if(cell.door.dir.dir == new Vector2(-1, 0))
                            {
                                if (cell.door.type == 3)
                                {
                                    SpecialDoorTrigger trigger = newDoor.GetComponent<SpecialDoorTrigger>();
                                    if (cell.rotation == 270)
                                        trigger.doorOffset = -2.5f;
                                    if (cell.rotation == 90)
                                        trigger.doorOffset = 2.5f;
                                }
                                else
                                {
                                    if (cell.rotation == 270)
                                        doorOffset = -2.5f;
                                    if (cell.rotation == 90)
                                        doorOffset = 2.5f;
                                }

                            }
                            newDoor.transform.position = new Vector3(newDoor.transform.position.x + doorOffset, newDoor.transform.position.y, newDoor.transform.position.z);

                           
                            if (cell.door.type != 3)
                                newDoor.GetComponent<SetDoorActive>().SetTriggersoff();
                        }
                        newDoor.transform.Rotate(transform.up * 90);
                    }
                    else if (cell.door.dir.dir == new Vector2(0, 1) || cell.door.dir.dir == new Vector2(0, -1))
                    {
                        if (cell.pieceType == PieceType.Deadend)
                        {
                            float doorOffset = 0;
                            if (cell.door.dir.dir == new Vector2(0, 1))
                            {
                                if (cell.door.type == 3)
                                {
                                    SpecialDoorTrigger trigger = newDoor.GetComponent<SpecialDoorTrigger>();
                                    if (cell.rotation == 0)
                                        trigger.doorOffset = 2.5f;
                                    if (cell.rotation == 180)
                                        trigger.doorOffset = -2.5f;
                                }
                                else
                                {
                                    if (cell.rotation == 0)
                                        doorOffset = 2.5f;
                                    if (cell.rotation == 180)
                                        doorOffset = -2.5f;
                                }

                            }
                            else if (cell.door.dir.dir == new Vector2(0, -1))
                            {
                                if (cell.door.type == 3)
                                {
                                    SpecialDoorTrigger trigger = newDoor.GetComponent<SpecialDoorTrigger>();
                                    if (cell.rotation == 0)
                                        trigger.doorOffset = 2.5f;
                                    if (cell.rotation == 180)
                                        trigger.doorOffset = -2.5f;
                                }
                                else
                                {
                                    if (cell.rotation == 0)
                                        doorOffset = 2.5f;
                                    if (cell.rotation == 180)
                                        doorOffset = -2.5f;
                                }

                            }

                            newDoor.transform.position = new Vector3(newDoor.transform.position.x, newDoor.transform.position.y, newDoor.transform.position.z + doorOffset);
                            if(cell.door.type != 3)
                                newDoor.GetComponent<SetDoorActive>().SetTriggersoff();
                        }
                        newDoor.transform.Rotate(transform.up * 0);
                    }
                    if(cell.door.type == 3)
                    {
                        specialDoorList.Add(newDoor);
                    }
                    newDoor.transform.SetParent(worldGrp.transform);
                }
                #endregion
            }
        }
        //if there is only one door with the type = 3 then the special door count will be 1 and will just spawn in the player at a random deadenn tile piece
        if (specialDoorList.Count <= 1)
            SetPlayerSpawn(yOffset, flipDirection);
        else
            setSpecialDoors(yOffset, flipDirection);
        FigureOutPlayerStartingRoomNumber();

        SpawnChest(yOffset, flipDirection);
        SpawnInfoDesk(yOffset, flipDirection);

    }
    //special doors are really the entrance door and exit/ back door
    private void setSpecialDoors(float yOffset, float flipDirection)
    {
        int doortype = 1;
        print("special door count " + specialDoorList.Count);
        if (specialDoorList.Count == 1)
        {
            foreach (GameObject door in specialDoorList)
            {
                SpecialDoorTrigger trigger = door.GetComponent<SpecialDoorTrigger>();
                switch (doortype)
                {
                    case 1:
                        trigger.doorType = DoorType.EntranceDoor;
                        break;
                }

                if (doortype == 1)
                {
                    SetPlayerSpawn(trigger.positionInt, yOffset, flipDirection);
                }
                doortype++;
            }
        }
        else if (specialDoorList.Count >= 2)
        {
            foreach(GameObject door in specialDoorList)
            {
                SpecialDoorTrigger trigger = door.GetComponent<SpecialDoorTrigger>();
                switch (doortype)
                {
                    case 1:
                        trigger.doorType = DoorType.EntranceDoor;
                        break;
                    case 2:
                        trigger.doorType = DoorType.ExitDoor;
                        break;
                }
                
                if(doortype == 1)
                {
                    SetPlayerSpawn(trigger.positionInt, yOffset, flipDirection);
                }
                doortype++;
            }
        }

    }
    void InstantiateTilePiece(Cell cell, float yOffset, float flipDirection, bool isRound)
    {
        float posX = cell.position.x;
        float posY = (cell.position.y * flipDirection) + yOffset;
        GameObject newtile = Instantiate(tile, new Vector3(posX * offset, 2, posY * offset), Quaternion.identity);

        newtile.transform.SetParent(worldGrp.transform);

        TileData tileData = newtile.GetComponent<TileData>();

        tileData.tilePosition.x = (int)cell.position.x;
        tileData.tilePosition.y = (int)cell.position.y;

        tileData.type = tileCalculator.CheckPiece(tileData.tilePosition);
        //flipping was done early on to match the dungeon layout, if marked true I think the pieces don't spawn in correctly
        if (!flip)
        {
            float flipValue = tileData.rotation += 90;
            newtile.transform.rotation = Quaternion.Euler(newtile.transform.rotation.x, newtile.transform.localRotation.y + 180, newtile.transform.rotation.z);
        }
        //Checking if the current tile is a deadend piece, if it is add to a list that will be used for potential player spawn locations
        if (tileData.type == PieceType.Deadend)
        {
            SpawnLocations.Add(tileData);
        }
        if (tileData.type == PieceType.Edge)
        {
            NumEdgePieces.Add(cell);
        }
        tileData.rotation = tileCalculator.Rotation;
        tileData.isRound = isRound;

        tileData.Initialize(cell, (roomType[cell.roomNumber]));
        cell.pieceType = tileData.type;
        cell.rotation = tileCalculator.Rotation;
        cell.tileObject = newtile;

        ListOfTiles.Add(cell);
    }

    #endregion

    #region Spawn in chest and levelinfo desk
    private void SpawnInfoDesk(float yOffset, float flipDirection)
    {
        //just cycling through and finding a tile that matches the player starting room number
        //remove tile from list if it's the wrong room number and randomly pick again
        //loops until a valid room is found
        List<Cell> templist = NumEdgePieces;
        bool hasSpawned = false;
        int num = 0;
        //int NumOfLoops = 0;
        while (hasSpawned == false)
        {
            num = Random.Range(0, templist.Count);
            if(templist[num].roomNumber == playerStatingRoomNumber)
            {
                hasSpawned = true;
            }
            else
            {
                templist.RemoveAt(num);
            }
            //NumOfLoops++;
            //if (NumOfLoops == 100)//Stop an infinite loop from happening
            //    print("Error couldn't find player starting room!");
            //    break;
        }
        //spawns in the info desk and removes that current edge piece from the list so nothing else can spawn there
        Cell cell = NumEdgePieces[num];
        if (cell.objectInTile != null)//checks if there is an object in the tile, if there is then call the function again this is hopefully to stop the chest and desk from spawning on each other
        {
            SpawnInfoDesk(yOffset, flipDirection);
        }
        else
        {
            GameObject NewSpawnedobject = Instantiate(levelInfoObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
            NewSpawnedobject.GetComponent<InfoDisplayTrigger>().SetUpInfo(title, story, infoTag.LevelDisplay);
            NewSpawnedobject.transform.SetParent(worldGrp.transform);
            cell.AddTileData(NewSpawnedobject);
            grid[(int)NumEdgePieces[num].position.x, (int)NumEdgePieces[num].position.y].AddTileData(NewSpawnedobject);
            NumEdgePieces.Remove(cell);
        }
    }

    private void SpawnChest(float yOffset, float flipDirection)
    {
        int num = Random.Range(0, NumEdgePieces.Count);
        Cell cell = NumEdgePieces[num];
        if (cell.objectInTile != null)
        {
            SpawnChest(yOffset, flipDirection);
        }
        else
        {
            GameObject NewSpawnedobject = Instantiate(ChestObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
            NewSpawnedobject.transform.SetParent(worldGrp.transform);
            cell.AddTileData(NewSpawnedobject);
            grid[(int)NumEdgePieces[num].position.x, (int)NumEdgePieces[num].position.y].AddTileData(NewSpawnedobject);
            NumEdgePieces.Remove(cell);
        }
    }
    #endregion

    #region Player related
    //unsure what the starting door coordinates are so currently just randomly pick between all of the deadend pieces coordinates
    private void SetPlayerSpawn(float yOffset, float flipDirection)
    {
        print("Not enought special doors");
        int num = Random.Range(0, SpawnLocations.Count);
        playerStartGrid = new Vector2Int((int)SpawnLocations[num].tilePosition.x, (int)SpawnLocations[num].tilePosition.y);
        playerStartCoord = new Vector3(SpawnLocations[num].tilePosition.x * offset, 0.5f, ((SpawnLocations[num].tilePosition.y * flipDirection) + yOffset) * offset);

    }

    private void SetPlayerSpawn(Vector2Int nePos, float yOffset, float flipDirection)
    {
        playerStartGrid = new Vector2Int(nePos.x, nePos.y);
        playerStartCoord = new Vector3(nePos.x * offset, 0.5f, ((nePos.y * flipDirection) + yOffset) * offset);

    }

    //This just checks the surrounding tiles around the player starting position
    //since the player always starts in a deadend tile there will always only be one tile with a room number
    //stores that room number to figure out which room the player is adjacent to
    //The x and y max values are 1 bigger than where the max grid pieces generate, so whenever comparing a value to a max must minus 1 from it
    private void FigureOutPlayerStartingRoomNumber()
    {
        print("room numbers around player");
        if (playerStartGrid.x + 1 <= max.x-1)
        {
            if(grid[playerStartGrid.x + 1, playerStartGrid.y].content == Contents.Empty)
            {
                print("Empty!");
            }
            if (grid[playerStartGrid.x + 1, playerStartGrid.y].content == Contents.Tile)
                playerStatingRoomNumber = grid[playerStartGrid.x + 1, playerStartGrid.y].roomNumber;
        }
        if (playerStartGrid.x - 1 >= min.x)//checking out of bounds
        {
            if (grid[playerStartGrid.x - 1, playerStartGrid.y].content == Contents.Empty)
            {
                print("Empty!");
            }
            if (grid[playerStartGrid.x - 1, playerStartGrid.y].content == Contents.Tile)
                playerStatingRoomNumber = grid[playerStartGrid.x - 1, playerStartGrid.y].roomNumber;
        }
        if (playerStartGrid.y + 1 <= max.y -1)//checking out of bounds// the max needs to minus 1 from it
        {

            if (grid[playerStartGrid.x, playerStartGrid.y + 1].content == Contents.Empty)
            {
                print("Empty!");
            }
            if (grid[playerStartGrid.x, playerStartGrid.y + 1].content == Contents.Tile)
                playerStatingRoomNumber = grid[playerStartGrid.x, playerStartGrid.y + 1].roomNumber;

        }
        if (playerStartGrid.y - 1 >= min.y)//checking out of bounds
        {
            if (grid[playerStartGrid.x, playerStartGrid.y - 1].content == Contents.Empty)
            {
                print("Empty!");
            }
            if (grid[playerStartGrid.x, playerStartGrid.y - 1].content == Contents.Tile)
                playerStatingRoomNumber = grid[playerStartGrid.x, playerStartGrid.y - 1].roomNumber;
        }
    }

    public void SpawnPlayer()
    {
        if (playerStartCoord == Vector3.zero)
            print("No Dungeon to Spawn Player");
        else
        {
            if (playerSpawned == false)
            {
                defaultCamera.SetActive(false);
                playerSpawned = true;
                player = Instantiate(playerOBJ, playerStartCoord, Quaternion.identity);
            }
        }
    }

    #endregion


    #region Debugging
    //Used for debugging and having something in scene that displays the grid number
    public void CheckGrid()
    {
        for (int x = 0; x < max.x; x ++)
        {
            for (int y = 0; y < max.y; y++)
            {
                GameObject number = Instantiate(gridNumber, new Vector3(grid[x, y].position.x * offset, 10, grid[x, y].position.y * offset), Quaternion.identity);
                TextMeshProUGUI text = number.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "x: " + x + " y: " + y + "   " + grid[x,y].content;
                print("grid position x " + (grid[x, y].position.x) + " x " + x);
                print("grid position y " + (grid[x, y].position.y) + " y " + y);
            }
        }
    }


    private void DisplayEmptyGrid(float yOffset, float flipDirection) 
    {
        foreach (Cell cell in grid)
        {
            if (cell != null)
            {
                if (cell.content == Contents.Empty)
                {
                    Instantiate(emptySpace, new Vector3(cell.position.x * offset, 5.1f, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                }
            }
        }
    }
    private void DisplayGridValues(float yOffset, float flipDirection)
    {
        foreach (Cell cell in grid)
        {
            if (cell != null)
            {
                GameObject number = Instantiate(gridNumber, new Vector3(cell.position.x * offset, 5.1f, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                TextMeshProUGUI text = number.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "x: " + cell.position.x + " y: " + cell.position.y + "   " + cell.content;
            }
        }
    }

    #endregion
}