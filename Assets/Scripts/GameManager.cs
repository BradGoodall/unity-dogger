using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Manager
    public static GameManager instance;

    // Player and Camera Elements
    [Header("Player & Camera")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameCamera;

    // UI Elements
    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text pressSpace;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private Image creditsPanel;
    [SerializeField] private TMP_Text controlsText;
    [SerializeField] private Image controlsPanel;
    [SerializeField] private Image doggerLogo;

    // Audio Clips
    [Header("Audio Clips")]
    [SerializeField] private AudioClip sfxDie;
    [SerializeField] private AudioClip sfxGoal;
    [SerializeField] private AudioClip sfxPickup;

    // Pickup Object
    [Header("Pickup Object")]
    [SerializeField] private GameObject pickupObject;

    private PlayerController playerController;
    private Vector3 cameraOffset;
    private AudioSource audioSource;
    private List<GameObject> pickups;

    // Scoring Variables
    private int playerScore = 0;
    private int playerHighScore = 0;
    private int playerLives = 3;
    private float timeLeft = 61;
    private int goalsReached = 0;
    private int highestStep;
    private int currentStep;

    // UI Animation Tweener variables
    private float current = 0;
    private float target = 1;
    private float tweenSpeed = 3;

    // Game State Variables
    private bool gameStarted = false;
    private bool gameOver = false;

    void Awake()
    {
        // Create the Game Manager Instance
        instance = this;
    }

    void Start()
    {
        // Get our references and setup the pickups list
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        pickups = new List<GameObject>();

        // Toggle the UI elements for the menu
        scoreText.enabled = false;
        timerText.enabled = false;
        livesText.enabled = false;
        pressSpace.enabled = true;
        doggerLogo.enabled = true;
        highScoreText.enabled = true;
        creditsText.enabled = true;
        creditsPanel.enabled = true;
        controlsText.enabled = true;
        controlsPanel.enabled = true;
        player.SetActive(false); // Disable the player in the menu

        // Get and display the saved High Score
        playerHighScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + playerHighScore;
    }

    void Update()
    {
        // If the game is in the menu state
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartGame(); // Start the game

            if (Input.GetKeyDown(KeyCode.H)) // For resetting the High Score
            {
                PlayerPrefs.SetInt("HighScore", 0);
                PlaySound(1);
                highScoreText.text = "High Score Reset!";
            }

            // Animate the UI
            current = Mathf.MoveTowards(current, target, tweenSpeed * Time.deltaTime);
            pressSpace.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1.2f), current);
            if (current == target)
                target = target == 0 ? 1 : 0;

        }

        // If the game is in the playing state
        if (gameStarted && gameOver==false)
        {
            // Countdown the timer
            if (!gameOver)
                timeLeft -= Time.deltaTime;

            // Spawn bones
            if (pickups.Count < 3)
                pickups.Add((GameObject)Instantiate(pickupObject, new Vector3(Random.Range(-12, 12), Random.Range(-7, 4), 0), Quaternion.identity));

            // Update the camera position to follow the player
            cameraOffset = new Vector3(0, player.transform.position.y + 4, -10);
            gameCamera.transform.position = cameraOffset;

            // Check for highest step (Scoring points when the player moves foward
            currentStep = (int)player.transform.position.y;
            if (currentStep > highestStep)
            {
                highestStep = currentStep;
                AddScore(10);
            }

            // Check for wins/losses
            if (playerLives <= 0 || goalsReached == 4 || (int)timeLeft == 0)
            {
                if (goalsReached == 4)
                {
                    AddScore((int)timeLeft * 100);
                    AddScore(1000);
                    EndGame("Congratulations, You Win!\nPress <SPACE> to Restart");
                }
                if (playerLives <= 0)
                {
                    EndGame("You ran out of lives!\nPress <SPACE> to Restart");
                }
                if ((int)timeLeft == 0)
                {
                    timerText.text = "Time Out!";
                    EndGame("You ran out of time!\nPress <SPACE> to Restart");
                }
            }

            // Update the UI texts
            scoreText.text = "Score: " + playerScore + "\nGoals: " + goalsReached + " / 4";
            livesText.text = "Lives: " + playerLives;
            timerText.text = "Time Left: " + (int)timeLeft;
        }

        // Reset the game when it's finished
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
            ResetGame();

        // Quit the game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }

    public void KillPlayer()
    {
        PlaySound(1);
        ResetPlayerPosition();
        playerLives--;
    }

    public void ResetPlayerPosition()
    {
        // Sets the player back to the starting position
        player.transform.position = new Vector3(0, -9, 0);
        currentStep = (int)player.transform.position.y;
        highestStep = currentStep;
    }

    private void ResetTimer()
    {
        // Resets the timer
        timeLeft += 20;
    }

    public void AddGoal()
    {
        // Increment the goal counter
        PlaySound(0);
        goalsReached++;
        if (goalsReached != 4)
            ResetTimer();
    }

    private void EndGame(string message)
    {
        if (!gameOver)
        {
            if (playerScore > playerHighScore) // If you beat the high score
            {
                PlayerPrefs.SetInt("HighScore", playerScore);
                highScoreText.text = "New High Score! " + PlayerPrefs.GetInt("HighScore", 0);
            }
            else // If you didn't beat the high score
            {
                highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
            }
                
        }

        // Show the correct UI elements
        pressSpace.enabled = true;
        pressSpace.text = message;
        highScoreText.enabled = true;
        gameOver = true;
    }

    public void PlaySound(int clip)
    {
        // Plays a sound clip based on the given int
        switch (clip)
        {
            case 0:
                audioSource.clip = sfxGoal;
                break;
            case 1:
                audioSource.clip = sfxDie;
                break;
            case 2:
                audioSource.clip = sfxPickup;
                break;
            default:
                audioSource.clip = null;
                break;
        }

        audioSource.Play();
    }

    public void RemovePickup(GameObject pickup)
    {
        // Remove a pickup from the list
        pickups.Remove(pickup);
    }

    private void StartGame()
    {
        // Toggle the correct UI elements
        scoreText.enabled = true;
        timerText.enabled = true;
        livesText.enabled = true;
        pressSpace.enabled = false;
        doggerLogo.enabled = false;
        highScoreText.enabled = false;
        creditsText.enabled = false;
        creditsPanel.enabled = false;
        controlsText.enabled = false;
        controlsPanel.enabled = false;
        player.SetActive(true); // Enable the player
        ResetPlayerPosition();
        PlaySound(2); // Play a sound

        gameStarted = true;
    }

    private void ResetGame()
    {
        // Resets the scene
        SceneManager.LoadScene(0);
    }

    public bool isGameOver()
    { 
        return gameOver; 
    }
}
