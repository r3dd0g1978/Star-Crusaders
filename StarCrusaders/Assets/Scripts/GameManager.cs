using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText, livesText;
    [SerializeField] Canvas pauseScreen;
    [SerializeField] Canvas gameOverScreen;
    [SerializeField] Canvas endOfLevelScreen;
    [SerializeField] int playerLives;

    int score;
    bool gameIsPaused = false;

    void OnEnable()
    {
        BossOne.OnBossDied += ProcessBossDeath;
        EnemyBasic.OnEnemyDied += UpdateScore;
        Crab.OnEnemyDied += UpdateScore;
        Player.OnPlayerHit += ProcessPlayerLife;
    }

    void OnDisable()
    {
        BossOne.OnBossDied -= ProcessBossDeath;
        EnemyBasic.OnEnemyDied -= UpdateScore;
        Crab.OnEnemyDied -= UpdateScore;
        Player.OnPlayerHit -= ProcessPlayerLife;
    }
        
    void Start()
    {
        score = 0;
        livesText.text = $"LIVES: {playerLives.ToString()}";
        scoreText.text = $"SCORE: {score.ToString()}";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void UpdateScore()
    {
        score += 100;
        scoreText.text = $"SCORE: {score.ToString()}";
    }

    string ProcessPlayerLife(string hitSFX)
    {
        playerLives--;
        livesText.text = $"LIVES: {playerLives.ToString()}";
        if (playerLives == 0)
        {
            ProcessPlayerDeath();
        }
        return hitSFX;
    }

    void ProcessPlayerDeath()
    {
        Player player = FindObjectOfType<Player>();
        Destroy(player.gameObject);
        DisplayGameOver();
    }

    void PauseGame()
    {
        if (!gameIsPaused)
        {
            pauseScreen.gameObject.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
        else
        {
            pauseScreen.gameObject.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
    }

    void DisplayGameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ProcessBossDeath()
    {
        UpdateScore();
        EndLevel();
    }

    void EndLevel()
    {
        endOfLevelScreen.gameObject.SetActive(true);
    }
}
