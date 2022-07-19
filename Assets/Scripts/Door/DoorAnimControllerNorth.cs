using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimControllerNorth : MonoBehaviour
{
    public Animator anim;
    public BoxCollider SouthTrigger;
    public BoxCollider NorthTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.Play("Door_Opening_North");
            SouthTrigger.gameObject.SetActive(false);
            NorthTrigger.gameObject.SetActive(false);
        }
    }
}
