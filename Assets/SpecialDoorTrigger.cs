using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DoorType
{
    EntranceDoor,
    ExitDoor
}
public class SpecialDoorTrigger : MonoBehaviour
{
    public float rotation;
    public float offset;
    public float flipDirection;
    public float yOffset;
    public float doorOffset;
    public GameObject entranceDoor, exitdoor;
    public DoorType doorType;
    public Vector2Int positionInt;
    public Vector3 positiion;
    public Material normal, exit;
    GameObject newDoor;
    private void Start()
    {


        switch (doorType)
        {

            case DoorType.EntranceDoor:
                if(rotation == 270 || rotation == 90)
                {
                    newDoor = Instantiate(entranceDoor, new Vector3(positiion.x + doorOffset, positiion.y, positiion.z), Quaternion.Euler(0, rotation, 0));
                }
                else if(rotation == 0 || rotation == 180)
                {
                    newDoor = Instantiate(entranceDoor, new Vector3(positiion.x, positiion.y, positiion.z + doorOffset), Quaternion.Euler(0, rotation, 0));
                }
                //Instantiate(entranceDoor, new Vector3((positionInt.x * offset) + doorOffset, 2, ((positionInt.y * flipDirection) + yOffset) * offset), Quaternion.Euler(0, rotation, 0));
                print("Entrance Door");
                break;
            case DoorType.ExitDoor:
                if (rotation == 270 || rotation == 90)
                {
                    newDoor = Instantiate(exitdoor, new Vector3(positiion.x + doorOffset, positiion.y, positiion.z), Quaternion.Euler(0, rotation, 0));
                }
                else if (rotation == 0 || rotation == 180)
                {
                    newDoor = Instantiate(exitdoor, new Vector3(positiion.x, positiion.y, positiion.z + doorOffset), Quaternion.Euler(0, rotation, 0));
                }

                //Instantiate(exitdoor, new Vector3((positionInt.x * offset) + doorOffset, 2, ((positionInt.y * flipDirection) + yOffset) * offset), Quaternion.Euler(0, rotation, 0));
                print("Exit Door");
                break;


        }
        newDoor.transform.SetParent(this.transform);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        switch (doorType)
    //        {
    //            case DoorType.EntranceDoor:
                    
    //                print("Entrance Door");
    //                break;
    //            case DoorType.ExitDoor:
    //                uiManager.ShowItsLocked();
    //                print("Exit Door");
    //                break;
    //        }
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        switch (doorType)
    //        {
    //            case DoorType.EntranceDoor:
    //                break;
    //            case DoorType.ExitDoor:
    //                uiManager.HideItsLocked();
    //                break;
    //        }
    //    }
    //}
}
