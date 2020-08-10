using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;


public enum Rotation { x, y, z} //stupid dumb

public class Player : MonoBehaviour
{
    //full disclosuer spelling is for the weak
    //grammer also sucks ass
    private float time;

    [Header("Player State")]
    public bool p1 = true;
    public bool dead = false;
    public Rotation rotation;

    private Vector2 move;
    private bool isGrounded;

    [Header("Player Charchteristics")]
    public PlayerType playerType;
    private float moveForce;
    private float jumpForce;
    private float gravityMultiplyer = 1;
    public float jumpDelay = 0.1f;

    private bool canJump = false;

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

        changeOritation(rotation);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    void FixedUpdate()
    {
        time -= Time.deltaTime;

        if(rb.useGravity == true) //only if it uses gravity
            rb.AddForce(Physics.gravity * gravityMultiplyer, ForceMode.Acceleration);

        if (time <= 0) //to stop unessery double jumps
            isGrounded = Physics.Raycast(transform.position, -Vector3.up, transform.localScale.y * 0.5f + 0.55f);
        else
            isGrounded = false;

        if (isGrounded == true)
            canJump = true;

        if (dead == true)
            return;

        var movey = new Vector3(0,0,0);

        if (rotation == Rotation.x) //very bad way but what are you gonna do
            movey = new Vector3(move.y, 0, move.x);

        else
            movey = new Vector3(-move.x, 0, -move.y);

        var movement = movey * moveForce * Time.deltaTime;

        if (isGrounded == false)
            movement = movement * 0.4f; //less force when not on ground

        rb.AddForce(movement, ForceMode.Force);
    }

    void Jump()
    {
        if (dead == true)
            return;

        if (canJump == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
            time = jumpDelay;
        }

    }

    public void changeOritation(Rotation rot)
    {
        rotation = rot;
        switch (rotation)
        {
            case(Rotation.x):
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX;
                    break;
                }
            case (Rotation.z):
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionZ;
                    break;
                }
            case (Rotation.y): //bit diffrent as player is not constrained cause top down = cool up - gottem
                {
                    break;
                }
        }
    }

    public void Dead()
    {
        dead = true;
        rb.constraints = RigidbodyConstraints.None;
        Camera.main.GetComponent<SharedCamera>().targets.Remove(transform); //removes it self from the cameras focus
    }
}
