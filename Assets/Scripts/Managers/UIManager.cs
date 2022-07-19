using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public static bool isInMenu = true;
    public static bool isInGame = false;
    public GridManager gridManager;
    public JsonConvertManager conversionManager;
    public CanvasSampleOpenFileTextJson fileBrowser;

    //Need to check if spawning player in topdownMode
    public bool isTopDown = false;

    [SerializeField]private GameObject MainMenu;
    [SerializeField]private GameObject PauseMenu;
    [SerializeField]private GameObject playBtn;
    [SerializeField] private GameObject userIcon;
    [SerializeField] private TextMeshProUGUI dungeonTitle;
    [SerializeField] private GameObject metaWalletInfo;
    [SerializeField] private string ifpsfirstHalf = "https://gateway.pinata.cloud/ipfs/";
    [SerializeField] private GameObject itsLockedText;

    public GameObject HudUIObj;

    public GameObject loadingText;
    public GameObject playLevelBtnImageCover;


    public string jsonLink;
    private string jsonData;

    public GameObject levelInfo;
    public TextMeshProUGUI titleInfoText;
    public TextMeshProUGUI InfoText;
    public TextMeshProUGUI userIDText;

    public string userID;
    public int currentDemoNum = 0;
    public TextAsset[] DefaultJson;
    public string[] DefaultJsonIFPSLinks;
    public string jsonView;

    [Header("DEBUG")]
    [SerializeField] private bool testingMode = false;


    private void Start()
    {
        currentDemoNum = 0;
        LoadDefaultJson();
        if (!testingMode)
        {
            if (StoreUserID.instance.isLoggedin == true)
            {
                metaWalletInfo.SetActive(true);
                userIcon.SetActive(true);
            }
            else if (StoreUserID.instance.isLoggedin == false)
            {
                metaWalletInfo.SetActive(false);
                userIcon.SetActive(false);
            }
            userID = StoreUserID.instance.userID;
            userIDText.text = userID;
        }

    }

    public void ShowItsLocked()
    {
        itsLockedText.SetActive(true);
    }
    public void HideItsLocked()
    {
        itsLockedText.SetActive(false);
    }
    private void Update()
    {
        CheckForEscapeButtonPressed();
        if (!isTopDown)
        {
            if (isInGame && !isInMenu )
            {
                HideMouse();
                HudUIObj.SetActive(true);
            }
        }
    }

    //play button calls this funtion
    public void SpawnPlayer()
    {
        gridManager.SpawnPlayer();
        if (gridManager.playerSpawned)
        {
            isInGame = true;
            isInMenu = false;
            if (!isTopDown)
            {
                HideMouse();
                HudUIObj.SetActive(true);
            }
            MainMenu.SetActive(false);
            PauseMenu.SetActive(false);
            ResumeGame();
        }
    }

    private void CheckForEscapeButtonPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInGame)
            {
                if (isInMenu)
                    ResumeGame();
                else
                    ToPauseMenu();
            }
        }
    }

    #region SetUp levels
    public void SetUpLevel(string json)
    {
        jsonView = json;
        try
        {
            gridManager.LoadJsonFile(json);
            SetUpLevelInfo();
            LevelFinshedLoading();
        }
        catch
        {
            print("Error couldn't load level!");
        }
    }

    private void SetUpLevelInfo()
    {
        dungeonTitle.text = gridManager.title;
        titleInfoText.text = gridManager.title;
        InfoText.text = gridManager.story;
    }

    #endregion

    private void LoadCurrentDefaultDungeon()
    {
        string json = DefaultJson[currentDemoNum].text;
        string name = DefaultJson[currentDemoNum].name;
        string newJson = conversionManager.ManualConversion(json);
        LoadFileFromBrowser(newJson);
    }
    #region Load Default JSON
    public void LoadDefaultJson()
    {
        if (currentDemoNum >= DefaultJson.Length - 1)
            currentDemoNum = 0;
        else
            currentDemoNum++;

        gridManager.DestroyDungeon();
        LoadCurrentDefaultDungeon();

    }

    #endregion

    #region Load JSON from the web
    int currentlinkNum = 0;

    //put in a ipfs link that matches the json format and it'll generate from online
    public void LoadFromWeb()
    {

        gridManager.DestroyDungeon();
        if (currentlinkNum >= DefaultJsonIFPSLinks.Length - 1)
            currentlinkNum = 0;
        else
            currentlinkNum++;
        string link = ifpsfirstHalf+DefaultJsonIFPSLinks[currentlinkNum];
        print(link);
        LoadingLevel();
        StopAllCoroutines();
        StartCoroutine(Check(link));

    }
    private IEnumerator Check(string link)
    {
        print("Link " + link);
        using (UnityWebRequest request = UnityWebRequest.Get(link))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError
                || request.result == UnityWebRequest.Result.ConnectionError)
            {
                print(request.error);
            }
            else
            {
                print("Successfully downloaded text!");
                print(request.downloadHandler.text);
                jsonData = request.downloadHandler.text;

                string newJsonData = conversionManager.ManualConversion(jsonData);
                print(newJsonData);
                SetUpLevel(newJsonData);
            }
        }
        yield break;

    }

    #endregion

    #region Load JSON from file browser
    // this is called from CanvasSampleOpenFileTextJson scrip to allow the file browser to work in a webgl application
    public void SetJson(string json)
    {
        string newJson = conversionManager.ManualConversion(json);
        LoadFileFromBrowser(newJson);
    }
    public void LoadFileFromBrowser(string loadedJson)
    {
        LoadingLevel();
        SetUpLevel(loadedJson);
    }

    #endregion


    #region Set up level info UI
    public void ShowLevelInfo(string info1)
    {
        InfoText.text = info1;
    }
    public void ShowLevelInfo(string info1, string info2)
    {
        titleInfoText.text = info1;
        InfoText.text = info2;
        levelInfo.SetActive(true);
    }
    public void HideLevelInfo()
    {
        titleInfoText.text = "";
        InfoText.text = "";
        levelInfo.SetActive(false);
    }

    #endregion


    #region Mouse control
    public static void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion

    #region Loading Functions
    public void LoadingLevel()
    {
        loadingText.SetActive(true);
    }
    public void LevelFinshedLoading()
    {
        loadingText.SetActive(false);
        playBtn.SetActive(true);
        playLevelBtnImageCover.SetActive(false);
    }
    #endregion


    #region Menus

    public void ToMainMenu()
    {

        isInGame = false;
        isInMenu = true;
        dungeonTitle.text = "";
        Time.timeScale = 1;
        HideItsLocked();
        gridManager.BackToMenu();
        gridManager.DestroyDungeon();
        playLevelBtnImageCover.SetActive(true);
        MainMenu.SetActive(true);
        PauseMenu.SetActive(false);
        ShowMouse();
        HudUIObj.SetActive(false);
        levelInfo.SetActive(false);
        LoadCurrentDefaultDungeon();
    }

    public void ToPauseMenu()
    {
        if(levelInfo.activeSelf == true)
        {
            levelInfo.SetActive(false);
        }
        ShowMouse();
        HideItsLocked();
        isInMenu = true;
        MainMenu.SetActive(false);
        PauseMenu.SetActive(true);
        HudUIObj.SetActive(false);
        Time.timeScale = 0;
    }

    [Obsolete]
    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (!isTopDown)
        {
            Application.ExternalEval("window.focus();");//this method is obsolete and sent me here https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html
            //but I couldn't see anything that could help
            HudUIObj.SetActive(true);
            HideMouse();
        }
        isInMenu = false;
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);
    }

    #endregion



}



