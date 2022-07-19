using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject player;
    void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
    }
}
