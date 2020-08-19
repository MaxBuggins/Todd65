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

    public GameRotationChange currentZone;

    [Header("Unity Things")]
    private MainControls controls; //refrence to Unitys input system
    private Rigidbody rb;
    private GameManager gameManager;
    private RaycastHit hit;
    private List<ContactPoint> contacts = new List<ContactPoint>(); //array of recent collisions

    void Awake()
    {
        //sets up componet refrences
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

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
            if(Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.3f, out hit, 0.75f))
            {
                if (hit.collider.transform == transform) //its self doesn't count
                    isGrounded = false;
                else
                    isGrounded = true;
            }   
        else
            isGrounded = false; //for if timer has not runout

        //constrains the max velocity of the player
        Vector3 newVelocity = rb.velocity;
        //clamps for each axis
        newVelocity.x = (Mathf.Clamp(rb.velocity.x, -maxVelocityXZ, maxVelocityXZ));
        newVelocity.z = (Mathf.Clamp(rb.velocity.z, -maxVelocityXZ, maxVelocityXZ));
        //falling and jumping are not clamped

        //assigns new clamped velocity
        rb.velocity = newVelocity;

        if (rb.useGravity == true) //only if it uses gravity
            rb.AddForce(Physics.gravity * gravityMultiplyer, ForceMode.Acceleration);

        if (dead == true)
            return;

        var movey = (SharedCamera.instance.transform.right) * move.x + (SharedCamera.instance.transform.forward) * move.y;
        var movement = movey * moveForce * Time.deltaTime;

        if (isGrounded == false)
            movement = movement * 0.4f; //less force when not on ground

        if (sucked == false)
            rb.AddForce(movement, ForceMode.Force);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (contacts.Count > 7)
        {
            contacts.RemoveAt(7);
            contacts.Insert(0, collision.GetContacts();
        }
        
        Vector3 boarderNorm = Vector3.zero;

        for(int i = 0; i < collision.contactCount && i < 10; i++)
        {
            if(contacts[i].otherCollider.gameObject.GetComponent<CamBoarder>() != null)
            {
                boarderNorm = contacts[i].normal;
            }
        }
        for (int i = 0; i < collision.contactCount && i < 10; i++)
        {
            if (Vector3.Dot(boarderNorm, contacts[i].normal) < -0.8f)
                Dead();
        }
    }

    void Jump()
    {
        if (dead == true)
            return;

        if (isGrounded == true && sucked == false) //checks if jumping is allowed
        {   
            //adds an instant force upwards
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; //stops player from jumping untill its hit floor again
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
        print("dead");
        dead = true;
        rb.constraints = RigidbodyConstraints.None;
        Camera.main.GetComponent<SharedCamera>().targets.Remove(transform); //removes it self from the cameras focus

        if (p1 == true) //bad yes, i dont care right now maybe later
            gameManager.p1Score -= 10;
        else
            gameManager.p2Score -= 10;

        var players = FindObjectsOfType<Player>();

        foreach(Player player in players) //checks if its the only player left and if so it can respawn
        {
            if (player != this)
                if(player.p1 == p1) //if there is other players (clones) then destory this one
                {
                    Destroy(gameObject);
                }
        }
    }
}
