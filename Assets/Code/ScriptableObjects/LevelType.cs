using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty { Easy, Medium, Hard}

[CreateAssetMenu(fileName = "LevelType", menuName = "LevelType", order = 1)]
public class LevelType : ScriptableObject
{

    [Header("MiniGame Charchteristics")]
    public string levelName;
    public Difficulty difficilty;
    public enum TeamType { FreeForAll, Split, AllOnOne }
    public TeamType teamType;

    public enum ControlType { WASD, Mouse}
    public ControlType controlType;

    public string levelDiscription;

    public Sprite levelPreview;

    public string level;
}
