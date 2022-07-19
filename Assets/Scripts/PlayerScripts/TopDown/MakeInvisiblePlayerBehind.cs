using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInvisiblePlayerBehind : MonoBehaviour
{
    [SerializeField] private List<PlayerInTheWay> currentlyInTheWay;
    [SerializeField] private List<PlayerInTheWay> alreadtTransparent;
    [SerializeField] private Transform player;
    private Transform camera;

    private void Awake()
    {
        currentlyInTheWay = new List<PlayerInTheWay>();
        alreadtTransparent = new List<PlayerInTheWay>();

        camera = this.gameObject.transform;
    
    }
    void Update()
    {
        GetAllObjectsInTheWay();

        MakeObjectsTransparent();
        MakeObjectsSolid();
    }
    public float SideRayOffset = 5;
    private void GetAllObjectsInTheWay()
    {
        currentlyInTheWay.Clear();

        float camerPlayerDistance = Vector3.Magnitude(camera.position - player.position);

        Ray ray1_Forward = new Ray(camera.position, player.position - camera.position);
        Ray ray1_Backward = new Ray(player.position, camera.position - player.position);
        
        Ray ray1_ForwardLeft1 = new Ray(camera.position + (Vector3.left * 1), player.position - camera.position);
        Ray ray1_ForwardRight1 = new Ray(camera.position + (Vector3.right * 1), player.position - camera.position);

        Ray ray1_ForwardLeft2 = new Ray(camera.position + (Vector3.left * 2), player.position - camera.position);
        Ray ray1_ForwardRight2 = new Ray(camera.position + (Vector3.right * 2), player.position - camera.position);

        var hits1_forward = Physics.RaycastAll(ray1_Forward, camerPlayerDistance);
        var hits1_backward = Physics.RaycastAll(ray1_Backward, camerPlayerDistance);

        var hits1_forwardLeft1 = Physics.RaycastAll(ray1_ForwardLeft1, camerPlayerDistance);
        var hits1_forwardRight1 = Physics.RaycastAll(ray1_ForwardRight1, camerPlayerDistance);

        var hits1_forwardLeft2 = Physics.RaycastAll(ray1_ForwardLeft2, camerPlayerDistance);
        var hits1_forwardRight2 = Physics.RaycastAll(ray1_ForwardRight2, camerPlayerDistance);

        foreach (var hit in hits1_forward)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }
        foreach (var hit in hits1_backward)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }
        foreach (var hit in hits1_forwardLeft1)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }
        foreach (var hit in hits1_forwardRight1)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }
        foreach (var hit in hits1_forwardLeft2)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }
        foreach (var hit in hits1_forwardRight2)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerInTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }

    }

    private void MakeObjectsTransparent()
    {
        for (int i = 0; i < currentlyInTheWay.Count; i++)
        {
            PlayerInTheWay inTheWay = currentlyInTheWay[i];

            if (!alreadtTransparent.Contains(inTheWay))
            {
                inTheWay.ShowInvisible();
                alreadtTransparent.Add(inTheWay);
            }
        }
    }
    private void MakeObjectsSolid()
    {
        for (int i = 0; i < alreadtTransparent.Count; i++)
        {
            PlayerInTheWay wasInTheWay = alreadtTransparent[i];

            if (!currentlyInTheWay.Contains(wasInTheWay))
            {
                wasInTheWay.ShowSolid();
                alreadtTransparent.Remove(wasInTheWay);
            }
        }
    }
}
