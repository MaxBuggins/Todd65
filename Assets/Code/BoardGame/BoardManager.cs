using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection {left, right, forward, back} //for what route is being picked might be a bad way to do it
public class BoardManager : MonoBehaviour
{
    [Header("Internal Variables")]
    public BoardPlayer[] players; //array because it must be fixed and in the same order allways
    public int playerTurn = 0;

    [Header("Unity Things")]
    private UIBoardGame mainUI;
    private BoardCamera mainCam;
    
    void Awake()
    {
        mainUI = FindObjectOfType<UIBoardGame>();
        mainCam = FindObjectOfType<BoardCamera>();

        players = FindObjectsOfType<BoardPlayer>();

        NewTurn();
    }

    
    void Update()
    {
        
    }

    public void NewTurn()
    {
        playerTurn++;
        if (playerTurn > players.Length - 1)
        {
            StartMiniGame();
            playerTurn = 0;
        }

        mainUI.NewTurn(true, players[playerTurn]);
        mainCam.target = players[playerTurn].transform;
        players[playerTurn].playerState = PlayState.Turn;
    }

    public void Roll()
    {
        players[playerTurn].Roll();
    }
    public void ChooseRoute(bool left)
    {
        if(left == true)
            players[playerTurn].ChangeRoute(MoveDirection.left);
        else
            players[playerTurn].ChangeRoute(MoveDirection.right);
    }

    public void StartMiniGame()
    {

    }
}
