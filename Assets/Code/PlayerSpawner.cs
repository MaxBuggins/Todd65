using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player State")]
    public bool p1 = true;

    public float spawnCooldown = 1f;

    public float moveSensitivity = 0.25f;
    private Vector2 move;
    private Vector2 localPos;

    [Header("Unity Things")]
    public GameObject player;
    private Camera cam;

    public MainControls controls;

    public Material redMaterial;
    public Material blueMaterial;

    private RaycastHit hit;

    void Awake()
    {
        if (FindObjectOfType<GameManager>().gameOver == true)
            Destroy(gameObject);


        //sets up input events
        else //because controls must NOT be set up for a big fat ERROR am i right or am i right?
            controls = new MainControls();
        cam = Camera.main;
    }

    void Start()
    {
        if (p1 == true) //checks if this script is for player1 if so takes player 1 inputs
        {
            GetComponent<Renderer>().material = redMaterial;
            controls.Player1.Jump.performed += funnier => Spawn();
            controls.Player1.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => move = Vector2.zero;
        }

        else //else it must be player 2 and takes in player2's input
        {
            GetComponent<Renderer>().material = blueMaterial;
            controls.Player2.Jump.performed += funnier => Spawn();
            controls.Player2.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => move = Vector2.zero;
        }

        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    void Update()
    {
        spawnCooldown -= Time.deltaTime;
        if (spawnCooldown > 0)
            return;

        localPos += move * moveSensitivity;
        localPos.x = Mathf.Clamp(localPos.x, -cam.orthographicSize, cam.orthographicSize);
        localPos.y = Mathf.Clamp(localPos.y, -cam.orthographicSize / 2, cam.orthographicSize / 2);

        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
        transform.position += new Vector3(localPos.x, localPos.y, 0);
    }

    void Spawn()
    {
        if (Physics.CheckSphere(transform.position, 1f))
            return;

        var play = Instantiate(player, transform.position, player.transform.rotation);
        //play.GetComponent<Player>().p1 = p1;
        controls.Disable();
        Destroy(gameObject);
    }
}
