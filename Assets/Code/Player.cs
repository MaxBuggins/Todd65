using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;


public enum Rotation { x, y, z } //stupid dumb

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
    public float moveForce;
    public float jumpForce;
    public float gravityMultiplyer = 1;
    public float jumpDelay = 0.1f;
    public float maxVelocityXZ;

    public GameRotationChange currentZone;

    [Header("Unity Things")]
    public GameObject BrokenBody;
    public GameObject playerSpawner;
    private MainControls controls; //refrence to Unitys input system

    private Renderer render;
    public Material redMaterial;
    public Material blueMaterial;

    private Rigidbody rb;
    private Vector3 pausedVelocity; //for when the game is paused
    private Vector3 pausedAngularVelocity; //for when the game is paused

    private GameManager gameManager;
    private RaycastHit hit;

    private Collider camBoarder;

    void Awake() //needs start because applying controls need to happen first but p1 state needs to be before start
    {
        //sets up componet refrences
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        render = GetComponent<Renderer>();
        if (FindObjectOfType<CamBoarder>() != null)
            camBoarder = FindObjectOfType<CamBoarder>().gameObject.GetComponent<Collider>();

        Camera.main.GetComponent<SharedCamera>().targets.Add(this.transform); //gets the camera to focus on it

        //sets up input events
        controls = new MainControls();
    }

    void Start() //need start because dumb
    {

        //applys player type variables
        moveForce = playerType.moveForce * rb.mass;
        jumpForce = playerType.jumpForce * rb.mass;
        gravityMultiplyer = playerType.gravityMultiplyer;

        if (p1 == true) //checks if this script is for player1 if so takes player 1 inputs
        {
            render.material = redMaterial;
            controls.Player1.Jump.performed += ctx => Jump();
            controls.Player1.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => move = Vector2.zero;
        }
        else //else it must be player 2 and takes in player2's input
        {
            render.material = blueMaterial;
            controls.Player2.Jump.performed += ctx => Jump();
            controls.Player2.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => move = Vector2.zero;
        }

        if (rotation == Rotation.x)
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
        if (gameManager.paused == true)
            return;

        time -= Time.deltaTime; //time is decressed depending on how much time has passed since last update


        if (time <= 0) //dosen't check untill jump delay has completed
            if (Physics.SphereCast(new Ray(transform.position, Vector3.down), 0.3f, out hit, 0.75f))
            {
                if (hit.collider.transform == transform) //its self doesn't count
                    isGrounded = false;
                else
                    isGrounded = true;

                if (hit.collider.gameObject.GetComponent<Bouncer>()) //bounce pads cannot be jumped off
                    isGrounded = false;
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
        else
            rb.AddForce(Physics.gravity * 1.25f, ForceMode.Acceleration);

        if (sucked == false)
            rb.AddForce(movement, ForceMode.Force);


        CrushCheck(); //check if the player has been crushed
    }

    Dictionary<Collider, List<ContactPoint>> contacts = new Dictionary<Collider, List<ContactPoint>>();
    private void OnCollisionStay(Collision collision)
    {
        contacts[collision.collider] = new List<ContactPoint>();
        contacts[collision.collider].AddRange(collision.contacts);
    }

    private void OnCollisionExit(Collision collision)
    {
        contacts.Remove(collision.collider);
    }

    private void CrushCheck()
    {
        // check if the crusher is crushing us
        if (camBoarder && contacts.ContainsKey(camBoarder))
        {
            Vector3 crusherNormal = contacts[camBoarder][0].normal;

            foreach (KeyValuePair<Collider, List<ContactPoint>> pair in contacts)
            {
                foreach (ContactPoint cp in pair.Value)
                {
                    if (Vector3.Dot(crusherNormal, cp.normal) < -0.8f)
                    {
                        print("dead");
                        Dead(true);
                    }
                }
            }
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
    public void changeOritation(Rotation rot, float lockPos) //this is kinda cut stuff but yeah
    {
        rotation = rot;
        switch (rotation)
        {
            case (Rotation.x):
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

    public void Dead(bool lose)
    {
        dead = true;
        rb.constraints = RigidbodyConstraints.None;
        Camera.main.GetComponent<SharedCamera>().targets.Remove(transform); //removes it self from the cameras focus

        var players = FindObjectsOfType<Player>();
        var brokenBody = Instantiate(BrokenBody, transform.position, transform.rotation);

        if (p1 == true) //so the broken body is the same colour
            foreach (Renderer render in brokenBody.GetComponentsInChildren<Renderer>())
                render.material = redMaterial;
        else
            foreach (Renderer render in brokenBody.GetComponentsInChildren<Renderer>())
                render.material = blueMaterial;

        if (lose == true)
        {
            if (p1 == true) //bad yes, i dont care right now maybe later
                gameManager.p1Score -= 10;
            else
                gameManager.p2Score -= 10;

            if (players.Length <= 1) //only player left
            {
                gameManager.GameOver(false); //tells the gameManager that the game is over with win set to false
                Destroy(gameObject);
            }

            bool clone = false;
            foreach (Player player in players) //checks if its the only player left and if so it can respawn
            {
                if (player != this) //doesn't check it self
                    if (player.p1 == p1) //only checks those that are the same p1 value
                    {
                        clone = true;
                        Destroy(gameObject); //if there is anthor player on the same 'team' then this is just a clone and then you can destory
                    }
            }
            if (clone == false)
                Spawn();
        }
        else
            Destroy(gameObject);
    }

    public void Spawn()
    {
        var spawner = Instantiate(playerSpawner);
        spawner.GetComponent<PlayerSpawner>().p1 = p1;
        Destroy(gameObject);
    }

    public void Pause(bool paused)
    {
        if (paused == true)
        {
            pausedVelocity = rb.velocity;
            pausedAngularVelocity = rb.angularVelocity;
            controls.Disable();
        }

        rb.isKinematic = paused;

        if (paused == false)
        {
            rb.velocity = pausedVelocity;
            rb.angularVelocity = pausedAngularVelocity;
            controls.Enable();
        }
    }
}
