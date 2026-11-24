using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunnerGameManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private RunnerPlayerController player;
    [SerializeField] private RunnerWorldController world;
    [SerializeField] private RunnerObstacleSpawner spawner;

    [Header("UI References")]
    [SerializeField] private GameObject startTitle;
    [SerializeField] private GameObject startInstructions;
    [SerializeField] private GameObject howToPlayText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private Button restartButton;

    private Vector3 playerStartPosition;
    private float score;
    private bool gameStarted;
    private bool gameOver;

    private void Awake()
    {
        playerStartPosition = player.transform.position;
    }

    private void Start()
    {
        gameStarted = false;
        gameOver = false;
        score = 0f;

        world.StopWorld();
        spawner.StopSpawning();
        player.DisablePlayer();

        startTitle.SetActive(true);
        startInstructions.SetActive(true);
        howToPlayText.SetActive(true);
        gameOverPanel.SetActive(false);

        UpdateScore();

        restartButton.onClick.AddListener(RestartGame);
    }

    private void Update()
    {
        if (!gameStarted && !gameOver &&
            Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (gameStarted && !gameOver)
        {
            score += Time.deltaTime * 10f;
            UpdateScore();
        }
    }

    private void StartGame()
    {
        gameStarted = true;

        startTitle.SetActive(false);
        startInstructions.SetActive(false);
        howToPlayText.SetActive(false);
        gameOverPanel.SetActive(false);

        world.StartWorld();
        spawner.StartSpawning();
        player.EnablePlayer();
    }

    public void GameOver()
    {
        if (gameOver || !gameStarted)
            return;

        gameOver = true;
        gameStarted = false;

        world.StopWorld();
        spawner.StopSpawning();
        player.DisablePlayer();

        finalScoreText.text = "Score: " + Mathf.FloorToInt(score);
        gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
            Destroy(obstacle);

        score = 0f;
        gameOver = false;
        gameStarted = false;

        world.StopWorld();
        spawner.StopSpawning();

        player.ResetPlayer(playerStartPosition);

        startTitle.SetActive(true);
        startInstructions.SetActive(true);
        howToPlayText.SetActive(true);
        gameOverPanel.SetActive(false);

        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(score);
    }

    private void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);
    }
}