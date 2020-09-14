using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlayer : MonoBehaviour
{
    [Header("Player Stats")]
    public string playerName;
    public int score;

    [Header("Player Charchterisits")]
    public float moveDelay;

    [Header("Internal Variables")]
    public Route currentRoute;
    public int routePos;
    public int nextRoutePos;
    
    private bool moveing = false;

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


    }

    void OnTweenDone()
    {
        routePos++;
        if (routePos <= nextRoutePos)
        {
            Tween.Position(transform, transform.position, currentRoute.childPieces[routePos].position + Vector3.up, 1, 0,
                completeCallback: OnTweenDone);
        }
        else
            score += currentRoute.childPieces[routePos].GetComponent<BoardPiece>().points;
    }

    public void Move(Route newRoute, int newRoutePos)
    {
        currentRoute = newRoute;
        nextRoutePos = newRoutePos;
        OnTweenDone();
    }
}
