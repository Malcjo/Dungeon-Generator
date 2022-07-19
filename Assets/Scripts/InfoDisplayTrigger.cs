using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDisplayTrigger : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uIManager;
    private infoTag tag;
    [SerializeField]private string info1;
    [SerializeField]private string info2;

    Vector3 rayStart;
    Vector3 direction;
    private bool wallCheck = false;
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        uIManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (wallCheck == false)
        {
            direction = Vector3.zero;
            rayStart = Vector3.zero;

            RaycastHit hit;
            rayStart = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            direction = -transform.forward * 3;

            if (!Physics.Raycast(rayStart, direction, 3))
            {
                transform.Rotate(transform.up * 90);
            }
            else
            {
                //this.transform.position -= transform.forward * 1.5f;
                wallCheck = true;
            }
        }

    }
    public void SetUpInfo(string info1, string info2,infoTag Tag)
    {
        this.tag = Tag;
        this.info1 = info1;
        this.info2 = info2;
    }
    public void SetUpInfo(string info1,infoTag Tag)
    {
        this.tag = Tag;
        this.info1 = info1;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (tag)
        {
            case infoTag.LevelDisplay:
                uIManager.ShowLevelInfo(info1, info2);
                break;
            case infoTag.NoteDisplay:
                uIManager.ShowLevelInfo(info1);
                break;
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
