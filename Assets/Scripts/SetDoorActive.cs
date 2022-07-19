using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDoorActive : MonoBehaviour
{
    public GameObject northTrigger, SouthTrigger;
    public void SetTriggersoff()
    {
        northTrigger.SetActive(false);
        SouthTrigger.SetActive(false);
    }
}
