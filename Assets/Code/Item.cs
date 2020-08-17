using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum PowerUp {none, lowGravity, clone}

    [Header("OnTrigger Info")]
    public PowerUp powerUp;
    public int points;


    [Header("Move Info")]
    public float moveAmount;
    public float moveSpeed;

    public float rotSpeed;

    private Vector3 orginPos;
    private Vector3 movePos;

    private GameManager gameManager;

    private void Start()
    {
        orginPos = transform.localPosition;
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {


        movePos = orginPos;
        movePos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * moveSpeed) * moveAmount;

        transform.Rotate(Vector3.one, rotSpeed, Space.Self);

        transform.localPosition = movePos; ; //move the item according to the sine wave
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
            if (other.GetComponent<Player>().p1 == true) //bad yes, i dont care right now maybe later
                gameManager.p1Score += points;
            else
                gameManager.p2Score += points;

            switch (powerUp)
            {
                case PowerUp.none:
                    {
                        break;
                    }
                case PowerUp.lowGravity:
                    {
                        break;
                    }
                

            }
            Destroy(gameObject);
        } 
    }
}
