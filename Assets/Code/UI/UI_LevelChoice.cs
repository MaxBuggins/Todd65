using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_LevelChoice : MonoBehaviour
{
    [Header("Level Charchteristics")]
    public LevelType levelType;

    [Header("Unity Things")]
    public TMP_Text levelTitleUI;
    public TMP_Text levelDifficultyUI;
    public Image levelSprite;


    void Start()
    {
        levelTitleUI.text = levelType.levelName;

        levelDifficultyUI.text = levelType.difficilty.ToString();

        switch (levelType.difficilty) //proberly a bad system of doing it but at least its a switch
        {
            case (Difficulty.Easy):
                {
                    levelDifficultyUI.color = Color.green;
                    break;
                }
            case (Difficulty.Medium):
                {
                    levelDifficultyUI.color = Color.yellow;
                    break;
                }
            case (Difficulty.Hard):
                {
                    levelDifficultyUI.color = Color.red;
                    break;
                }
        }

        levelSprite.sprite = levelType.levelPreview;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelType.level);
    }
}
