using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInTheWay : MonoBehaviour
{
    public GameObject mesh;
    private void Awake()
    {
        ShowSolid();
    }

    public void ShowSolid()
    {
        mesh.SetActive(true);
    }
    public void ShowInvisible()
    {
        mesh.SetActive(false);
    }
}
