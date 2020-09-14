using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayerScore : MonoBehaviour
{
    public BoardPlayer player;

    [Header("Unity Things")]
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScore;


    void Start()
    {
        playerName.text = player.playerName;
        playerScore.text = "" + player.score;
    }

    void FixedUpdate()
    {
        playerScore.text = "" + player.score;
    }
}
