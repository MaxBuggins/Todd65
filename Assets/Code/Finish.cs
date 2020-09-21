using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public int winPoints;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            //if (player.p1 == true)
            //    gameManager.p1Score += winPoints;
            //else
            //    gameManager.p2Score += winPoints;

            gameManager.GameOver(true);
            player.Dead(false);
        }
    }
}
