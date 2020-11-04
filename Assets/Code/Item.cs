using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum PowerUp { none, agility, clone }

    [Header("Itenm Info")]
    public PowerUp powerUp;
    public int points;

    public bool opisitePickUp;
    public Player closestPlayer;


    [Header("Move Info")]
    public float moveAmount;
    public float moveSpeed;

    public float rotSpeed;

    private Vector3 orginPos;
    private Vector3 movePos;

    private GameManager gameManager;
    private Renderer render;

    private void Start()
    {
        orginPos = transform.localPosition;
        gameManager = FindObjectOfType<GameManager>();
        render = GetComponent<Renderer>();
    }
    void Update()
    {
        if (moveAmount == 0)
            return;
        movePos = orginPos;
        movePos.y += Mathf.Sin(Time.deltaTime * Mathf.PI * moveSpeed) * moveAmount;

        transform.Rotate(Vector3.one, rotSpeed, Space.Self);

        transform.localPosition = movePos; ; //move the item according to the sine wave

        if (opisitePickUp == true)
            RelativeMaterial();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            switch (powerUp)
            {
                case PowerUp.clone:
                    {
                        Instantiate(other.gameObject);
                        Destroy(gameObject);
                        break;
                    }
            }
        }

        if (other.tag == "Player")
        {
            bool targetP1;
            if (opisitePickUp == false) //if the touching player gets effected
                targetP1 = other.GetComponent<Player>().p1;
            else
                targetP1 = !other.GetComponent<Player>().p1;

            if (targetP1 == true) //bad yes, i dont care right now maybe later
                gameManager.p1Score += points;
            else
                gameManager.p2Score += points;

            switch (powerUp)
            {
                case PowerUp.none:
                    {
                        break;
                    }
                case PowerUp.agility:
                    {
                        var players = FindObjectsOfType<Player>();
                        foreach(Player player in players)
                        {
                            if(player.p1 == targetP1)
                            {
                                player.moveForce *= 1.25f;
                                player.jumpForce *= 1.25f;
                            }
                        }
                        break;
                    }


            }
            Destroy(gameObject);
        }
    }

    void RelativeMaterial() //changes the colour of the material to reflect which player is closest
    {
        float closestMag = Mathf.Infinity; //need to start somewhere and infiity looks cool
        var players = FindObjectsOfType<Player>();

        foreach (Player player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position); //gets the distance
            if(distance < closestMag) //compars the distance and if its smaller then its the new closestMag
                closestPlayer = player;
        }

        if(closestPlayer.p1 == true)  //not so cool and eligant but im lazy   
            render.material = gameManager.p1Material;
        else
            render.material = gameManager.p2Material;

    }
}
