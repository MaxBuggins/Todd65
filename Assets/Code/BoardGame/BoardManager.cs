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

    void NewTurn()
    {
        playerTurn++;
        if (playerTurn > players.Length - 1)
        {
            StartMiniGame();
            playerTurn = 0;
        }

        mainUI.NewTurn(true);
        mainCam.target = players[playerTurn].transform;

    }

    public void ChoseRoute(MoveDirection moveDirection)
    {
        Route newRoute = players[playerTurn].currentRoute;
        foreach (Route.RouteInsert route in players[playerTurn].currentRoute.nextRoutes)
            if (route.moveDirection == moveDirection)
                players[playerTurn].Move(route.route, route.startPos);
    }

    public void Roll()
    {
        var roll = Random.Range(1, 6);

        if (players[playerTurn].currentRoute.childPieces.Length <= players[playerTurn].routePos)
        {
            mainUI.ChooseDirection();
            return;
        }
        players[playerTurn].Move(players[playerTurn].currentRoute, roll + players[playerTurn].routePos);
        NewTurn();
    }

    public void StartMiniGame()
    {

    }
}
