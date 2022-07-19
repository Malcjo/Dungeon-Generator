using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void BackToLoginScreen()
    {
        if(StoreUserID.instance != null)
        {
            StoreUserID.instance.ResetValues();
        }
        SceneManager.LoadScene(0);
    }
}
