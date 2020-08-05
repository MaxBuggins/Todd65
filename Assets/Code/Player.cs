using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool p1 = true;
    public bool dead = false;
    private Vector2 move;
    private bool isGrounded;
    public PlayerType playerType;

    [Header("Player Charchteristics")]
    private float moveForce;
    private float jumpForce;
    private float gravityMultiplyer = 1;

    [Header("Unity Things")]
    private MainControls controls;
    private Rigidbody rb;

    void Awake()
    {
        //sets up componet refrences
        rb = GetComponent<Rigidbody>();
        
        //applys player type variables
        moveForce = playerType.moveForce * rb.mass;
        jumpForce = playerType.jumpForce * rb.mass;
        gravityMultiplyer = playerType.gravityMultiplyer;

        controls = new MainControls();
        //sets up input events
        if (p1 == true)
        {
            controls.Player1.Jump.performed += ctx => Jump();
            controls.Player1.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => move = Vector2.zero;
        }
        else
        {
            controls.Player2.Jump.performed += ctx => Jump();
            controls.Player2.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => move = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    void FixedUpdate()
    {
        if(rb.useGravity == true) //only if it uses gravity
            rb.AddForce(Physics.gravity * gravityMultiplyer, ForceMode.Acceleration);

        isGrounded = Physics.Raycast(transform.position, -Vector3.up, transform.localScale.y * 0.5f + 0.55f);

        if (dead == true)
            return;
        var movement = new Vector3(-move.x, 0, -move.y) * moveForce * Time.deltaTime;
        if (isGrounded == false)
            movement = movement * 0.4f; //less force when not on ground

        rb.AddForce(movement, ForceMode.Force);
    }

    void Jump()
    {
        if (dead == true)
            return;

        if(isGrounded == true)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
    }

    public void Dead()
    {
        dead = true;
        rb.constraints = RigidbodyConstraints.None;
        Camera.main.GetComponent<SharedCamera>().targets.Remove(transform); //removes it self from the cameras focus
    }
}
