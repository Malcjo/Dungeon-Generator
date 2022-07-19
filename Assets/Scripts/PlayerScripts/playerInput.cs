using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class playerInput : MonoBehaviour
{
    private PlayerInputConfig starterAssetsInput;

    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool interact;

    private bool jumpQueued;
    private void Awake()
    {
        starterAssetsInput = new PlayerInputConfig();
    }

    private void OnLook(CallbackContext context)
    {

    }
    private void OnMove (CallbackContext context)
    {
        if (context.started)
        {
            print("Move");
            move = context.ReadValue<Vector2>();
        }
    } 
    private void OnJump(CallbackContext context)
    {
        if (context.started)
        {
            jumpQueued = true;
        }
        if (context.canceled)
        {
        }
    }

    private void JumpQueued()
    {
        if (jumpQueued)
        {
            print("Jump");
            jumpQueued = false;
            jump = true;
        }
        jump = false;
    }

}
