using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pixelplacement;

public class UIBoardGame : MonoBehaviour
{

    [Header("Charchteristics")]
    public float playerTurnTextDelay;

    [Header("Internal Variables")]
    private float time;

    [Header("Unity Things")]
    public GameObject playerStatsUI;
    private BoardManager boardManager;

    public GameObject playerTurn;
    public GameObject chooseDirection;
    public TextMeshProUGUI[] playerScores;

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();

        int num = 0;
        foreach (BoardPlayer player in boardManager.players)
        {
            var ui = Instantiate(playerStatsUI, transform);

            ui.GetComponent<UIPlayerScore>().player = player;
            ui.transform.position -= Vector3.up * 50 * num;

            num++;
        }
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time < 0)
        {
            NewTurn(false);
        }

    }

    public void NewTurn(bool _new)
    {
        if (_new == true)
        {
            playerTurn.SetActive(true);
            time = playerTurnTextDelay;
        }
        else
            playerTurn.SetActive(false);
    }

    public void ChooseDirection()
    {
        chooseDirection.SetActive(true);
    }
}
