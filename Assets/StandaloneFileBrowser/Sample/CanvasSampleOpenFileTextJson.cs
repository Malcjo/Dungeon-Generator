using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using TMPro;

[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileTextJson : MonoBehaviour, IPointerDownHandler
{
    //The is a file browser class found online that's been modified to grab json files
    //It's used to allow file browser to work on a WebGL application
    public UIManager uiManager;
    private string jsonString = "";

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".json", false);//The ".json" means it is only looking for json files
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutine(url));
    }
#else

    //Standalone platforms & editor

    public void OnPointerDown(PointerEventData eventData) { }

    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "json", false);
        if (paths.Length > 0)
        {
            string newPath = new System.Uri(paths[0]).AbsoluteUri;
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }

#endif
    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);

        yield return loader;
        jsonString = loader.text;
        //parsing the text from the loaded json file into SetJson function which calls the manualConversion function
        uiManager.SetJson(jsonString);
    }
}