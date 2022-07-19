using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        this.transform.Rotate(transform.forward * speed * Time.deltaTime);
    }
}
