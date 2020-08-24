using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Media;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Stats")]
    public bool paused = true;


    public int p1Score;
    public int p2Score;

    public Material p1Material;
    public Material p2Material;

    [Header("Unity Things")]
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    public GameObject mainScreen;
    public GameObject pauseScreen;
    public GameObject loseScreen;
    
    private MainControls controls; //refrence to Unitys input system

    [Header("AudioClips")]
    private AudioSource audioPlayer;
    public AudioClip loseTrack;

    void Awake() //because it needs to SET UP INPUTS before OnEnable
    {
        //sets up input events
        controls = new MainControls();
        audioPlayer = Camera.main.GetComponent<AudioSource>();

        controls.Shared.Pause.performed += funny => Pause();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    void Update()
    {
        p1ScoreText.text = ("P1: " + p1Score);
        p2ScoreText.text = ("P1: " + p2Score);
    }

    private void Pause()
    {
        pauseScreen.SetActive(!paused);

        if (paused == true) //this feels dumb
        {
            Time.timeScale = 1; //time starts
            paused = false;
        }
        else
        {
            Time.timeScale = 0; //time stops
            paused = true;
        }

        var players = FindObjectsOfType<Player>();


        foreach(Player player in players)
        {
            player.Pause(paused);
        }
    }

    public void GameOver(bool win)
    {
        mainScreen.SetActive(false);
        if(win == false)
        {
            loseScreen.SetActive(true);
            audioPlayer.clip = loseTrack;
            audioPlayer.Play();
        }
    }

    public void ReLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
