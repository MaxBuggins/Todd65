using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Stats")]
    public int p1Score;
    public int p2Score;

    [Header("Unity Things")]
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;


    void Start()
    {       
    }

    void Update()
    {
        p1ScoreText.text = ("P1: " + p1Score);
        p2ScoreText.text = ("P1: " + p2Score);
    }
}
