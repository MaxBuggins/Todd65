using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player State")]
    public bool p1 = true;

    [Header("Unity Things")]
    public GameObject player;
    private MainControls controls;

    public Material redMaterial;
    public Material blueMaterial;

    void Awake()
    {
        //sets up input events
        controls = new MainControls();

        if (p1 == true) //checks if this script is for player1 if so takes player 1 inputs
        {
            GetComponent<Renderer>().material = redMaterial;
            controls.Player1.Jump.performed += funnier => Spawn();
        }

        else //else it must be player 2 and takes in player2's input
        {
            GetComponent<Renderer>().material = blueMaterial;
            controls.Player2.Jump.performed += funnier => Spawn();
        }
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider != null) //highlights selected terrain piece
        {
            transform.position = new Vector3(hit.point.x, hit.point.y);
        }
    }

    void Spawn()
    {
        var play = Instantiate(player, transform.position, player.transform.rotation);
        play.GetComponent<Player>().p1 = p1;
    }
}
