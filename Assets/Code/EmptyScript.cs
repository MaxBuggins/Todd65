using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EmptyScript : MonoBehaviour
{
    private float time;
    public bool p1 = true; //determins if this instance is for player 1 or player 2
    private float jumpForce;

    private Vector2 move; //2D Vector movement refrence
    private MainControls controls; //refrence to Unitys input system for the entire script

    private Rigidbody rb;
    private bool canJump = false;

    void Awake()
    {
        //sets up new input refrence
        controls = new MainControls();

        if (p1 == true) //checks if this script is for player1 if so takes player 1 inputs
        {
            controls.Player1.Jump.performed += ctx => Jump(); //if input pressed then run jump
            controls.Player1.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => move = Vector2.zero;
        }
        else //else it must be player 2 and takes in player2's input
        {
            controls.Player2.Jump.performed += ctx => Jump(); //if input pressed then run jump
            controls.Player2.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => move = Vector2.zero;
        }
    }

    void Jump()
    {
        //adds an instant force upwards
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
    }
}
