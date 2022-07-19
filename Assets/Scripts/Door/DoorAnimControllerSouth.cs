using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimControllerSouth : MonoBehaviour
{
    public Animator anim;
    public BoxCollider SouthTrigger;
    public BoxCollider NorthTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.Play("Door_Opening_South");
            SouthTrigger.gameObject.SetActive(false);
            NorthTrigger.gameObject.SetActive(false);
        }
    }
}
