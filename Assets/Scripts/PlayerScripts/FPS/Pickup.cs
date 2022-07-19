using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private PlayerInputConfig _input;

    //pick up object
    public Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    public float pickUpRange = 5;
    public float moveForce = 150f;

    private void Start()
    {
        _input = GetComponent<PlayerInputConfig>();
    }

    private void Update()
    {
        PickUpObj();
    }



    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        if (GameObject.FindGameObjectWithTag("World_Grp"))
        {
            heldObjRB.transform.parent = GameObject.FindGameObjectWithTag("World_Grp").transform;
        }
        else
        {
            heldObjRB.transform.parent = null;
        }
        
        heldObj = null;
    }

    private void PickUpObj()
    {
        if (_input.interact)
        {
            _input.interact = false;
            if (heldObj == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickUpRange))
                {
                    PickUpObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }
        if (heldObj != null)
        {
            MoveObj();
        }
    }

    private void MoveObj()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * moveForce);
        }
    }
    void PickUpObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }
    private void DropObj()
    {
    }
}
