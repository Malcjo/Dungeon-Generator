using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInfoDisplay : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uIManager;

    private string info;
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        uIManager = FindObjectOfType<UIManager>();
    }

    public void SetUpInfo(string info1)
    {
        this.info = info1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            uIManager.ShowLevelInfo(info);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            uIManager.HideLevelInfo();
        }
    }
}
