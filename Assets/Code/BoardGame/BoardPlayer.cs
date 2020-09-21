using JetBrains.Annotations;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayState { Idle, Turn, Moveing, Choice }
public class BoardPlayer : MonoBehaviour
{
    [Header("Player Stats")]
    public string playerName;
    public int score;

    [Header("Player Charchterisits")]
    public float moveDelay;

    [Header("Internal Variables")]
    public PlayState playerState = PlayState.Idle;

    public Route currentRoute;
    public int routePos;
    public int nextRoutePos;

    [Header("Unity Things")]
    private BoardManager boardManager;
    private UIBoardGame mainUI;


    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        mainUI = FindObjectOfType<UIBoardGame>();
        transform.position = currentRoute.childPieces[routePos].position;
        transform.position += Vector3.up;
    }

    void Update()
    {
        switch (playerState)
        {
            case PlayState.Idle:
                {
                    break;
                }
            case PlayState.Turn:
                {
                    break;
                }
            case PlayState.Moveing:
                {
                    break;
                }
        }

    }

    public void Roll()
    {
        var roll = Random.Range(1, 6);

        nextRoutePos = routePos + roll;

        OnTweenDone();
    }
    void OnTweenDone()
    {
        if (routePos < nextRoutePos)
        {
            routePos++;
            if (routePos < currentRoute.childPieces.Length)
            {
                Tween.Position(transform, transform.position, currentRoute.childPieces[routePos].position + Vector3.up, 0.5f, 0,
                    completeCallback: OnTweenDone);
            }
            else
            {
                ChangeRoute();
            }
        }
        else
        {
            score += currentRoute.childPieces[routePos].GetComponent<BoardPiece>().points;
            foreach(BoardPlayer player in boardManager.players)
            {
                if (player.routePos == routePos && player != this)
                {
                    Tween.Position(transform, transform.position, currentRoute.childPieces[routePos].position + Vector3.one - Vector3.right, 0.5f, 0);
                    boardManager.NewTurn();
                    return;
                }
            }
            Tween.Position(transform, transform.position, currentRoute.childPieces[routePos].position + Vector3.one, 0.5f, 0);
            boardManager.NewTurn();
        }
    }

    public void ChangeRoute()
    {
        if(currentRoute.nextRoutes.Length == 1)
        {
            routePos = currentRoute.nextRoutes[0].startPos;
            nextRoutePos -= currentRoute.childPieces.Length;
            currentRoute = currentRoute.nextRoutes[0].route;
            Tween.Position(transform, transform.position, currentRoute.childPieces[routePos].position + Vector3.up, 0.5f, 0,
                completeCallback: OnTweenDone);
        }
        else
        {
            mainUI.ChooseDirection(currentRoute);
        }
    }

    public void ChangeRoute(MoveDirection moveDirection)
    {
        int count = 0;
        foreach (Route.NextRoutes route in currentRoute.nextRoutes)
            if (route.moveDirection == moveDirection)
            {
                routePos = currentRoute.nextRoutes[count].startPos;
                nextRoutePos -= currentRoute.childPieces.Length;
                currentRoute = route.route;
                Tween.Position(transform, transform.position, currentRoute.childPieces[routePos].position + Vector3.up, 0.5f, 0,
                    completeCallback: OnTweenDone);

                count ++;
            }
    }
}
