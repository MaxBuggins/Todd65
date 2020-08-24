using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty { Easy, Medium, Hard, TeamWork }

[CreateAssetMenu(fileName = "LevelType", menuName = "LevelType", order = 1)]
public class LevelType : ScriptableObject
{

    [Header("Level Charchteristics")]
    public string levelName;
    public Difficulty difficilty;
    public Sprite levelPreview;

    public Object level;
}
