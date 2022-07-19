using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }


        private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            uiManager.ShowItsLocked();
            print("Exit Door");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            uiManager.HideItsLocked();
        }
    }
}
