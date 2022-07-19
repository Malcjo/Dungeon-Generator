using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotToMouse : MonoBehaviour
{
    public float speed = 5f;
    public Camera cam;
    public Vector3 direction;
    public Vector3 mouseInput;
	void Start()
	{
	}
	void Update()
    {
        mouseInput = Input.mousePosition;
        Ray cameraRay = cam.ScreenPointToRay(mouseInput);
        Plane groundplane = new Plane(Vector3.up, Vector3.zero);
        float raylength;

        if(groundplane.Raycast(cameraRay, out raylength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(raylength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

    }
}
