using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;


public enum Rotation {x, y, z} //stupid dumb

public class Player : MonoBehaviour
{
    //full disclosuer spelling is for the weak
    //grammer also sucks ass
    private float time;

    [Header("Player State")]
    public bool p1 = true; //determins if this instance is for player 1 or player 2
    public bool dead = false;
    public Rotation rotation;

    public bool sucked = false; //for when player is traveling through pipes and does not get effected by fixed position
    private Vector2 move; //2D Vector movement refrence
    private bool isGrounded;
    private float lockPos;

    [Header("Player Charchteristics")]
    public PlayerType playerType;
    private float moveForce;
    private float jumpForce;
    public float gravityMultiplyer = 1;
    public float jumpDelay = 0.1f;
    public float maxVelocityXZ;

    private bool canJump = false;

    public GameRotationChange currentZone;

    [Header("Unity Things")]
    private MainControls controls; //refrence to Unitys input system
    private Rigidbody rb;


    void Awake()
    {
        //sets up componet refrences
        rb = GetComponent<Rigidbody>();

        Camera.main.GetComponent<SharedCamera>().targets.Add(this.transform); //gets the camera to focus on it


        //applys player type variables
        moveForce = playerType.moveForce * rb.mass;
        jumpForce = playerType.jumpForce * rb.mass;
        gravityMultiplyer = playerType.gravityMultiplyer;

        //sets up input events
        controls = new MainControls();

        if (p1 == true) //checks if this script is for player1 if so takes player 1 inputs
        {
            controls.Player1.Jump.performed += ctx => Jump();
            controls.Player1.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => move = Vector2.zero;
        }
        else //else it must be player 2 and takes in player2's input
        {
            controls.Player2.Jump.performed += ctx => Jump();
            controls.Player2.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => move = Vector2.zero;
        }

        if(rotation == Rotation.x)
        {
            lockPos = transform.position.x;
        }
        if (rotation == Rotation.z)
        {
            lockPos = transform.position.z;
        }

        changeOritation(rotation, lockPos);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    void FixedUpdate()
    {
        time -= Time.deltaTime; //time is decressed depending on how much time has passed since last update

        if (time <= 0) //dosen't check untill jump delay has completed
            isGrounded = Physics.Raycast(transform.position, -Vector3.up, transform.localScale.y * 0.5f + 0.1f);
        else
            isGrounded = false; //for if timer has not runout

        if (isGrounded == true) //if the raycast hist then the player can jump
            canJump = true;

        //constrains the max velocity of the player
        Vector3 newVelocity = rb.velocity;
        //clamps for each axis
        newVelocity.x = (Mathf.Clamp(rb.velocity.x, -maxVelocityXZ, maxVelocityXZ   ));
        newVelocity.z = (Mathf.Clamp(rb.velocity.z, -maxVelocityXZ, maxVelocityXZ));
        //falling and jumping are not clamped

        //assigns new clamped velocity
        rb.velocity = newVelocity;

        if (rb.useGravity == true) //only if it uses gravity
            rb.AddForce(Physics.gravity * gravityMultiplyer, ForceMode.Acceleration);

        if (dead == true)
            return;

        var movey = (SharedCamera.instance.transform.right)* move.x + (SharedCamera.instance.transform.forward) * move.y;
        var movement = movey * moveForce * Time.deltaTime;

        if (isGrounded == false)
            movement = movement * 0.4f; //less force when not on ground

        if (sucked == false)
            rb.AddForce(movement, ForceMode.Force);
    }

    void Jump()
    {
        if (dead == true)
            return;

        if (canJump == true && sucked == false) //checks if jumping is allowed
        {   
            //adds an instant force upwards
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; //stops player from jumping untill its hit floor again
            time = jumpDelay; //sets a small delay until it can jump again
        }

    }
    public void changeOritation(Rotation rot, float lockPos)
    {
        rotation = rot;
        switch (rotation)
        {
            case(Rotation.x):
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX;
                    transform.position = new Vector3(lockPos, transform.position.y, transform.position.z);
                    break;
                }
            case (Rotation.z):
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionZ;
                    transform.position = new Vector3(transform.position.x, transform.position.y, lockPos);
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
