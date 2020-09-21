using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PlayType { Platform, Racer, }

[CreateAssetMenu(fileName = "PlayerType", menuName = "PlayerType", order = 1)]
public class PlayerType : ScriptableObject
{
    [Header("Player Charchteristics")]

    public float moveForce;
    public float jumpForce;
    public float gravityMultiplyer = 1;
}
