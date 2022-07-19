using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUserID : MonoBehaviour
{
    //This is a singleton
    //it persists forever once the game has started
    //it is created in the login screen
    //Storing user data here instead of in the playerData settings
    //because there was an error occuring when doing that
    //this stores the meta wallet information
    public string userID ="";
    public bool isLoggedin = false;
    public string response = "";
    public static StoreUserID instance;
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    public void ResetValues()
    {
        string userID = "";
        bool isLoggedin = false;
        string response = "";
    }
}
