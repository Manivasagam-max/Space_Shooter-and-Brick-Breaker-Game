using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManage4S : MonoBehaviour
{
    public BallScript bscript;

    public int lives;
    public int score;
    public Text livesText;
    public Text scoreText;
    public GameObject gameOverPanel;
    public int numberOfBricks;
    public int playerSideBricks;
    public int enemySideBricks;
    public bool gameOver;
    public GameObject NextLevelPanel;

    void Start()
    {
        lives = 3; // Reset lives to 3
        score = 0; // Reset score to 0
        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score;
        if (FindObjectOfType<BrickType>() != null)
        {
            playerSideBricks = CountBricksWithId(1);
            enemySideBricks = CountBricksWithId(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOver();
        }
        if (FindObjectOfType<BrickType>() != null)
        {
            playerSideBricks = CountBricksWithId(1);
            enemySideBricks = CountBricksWithId(2);
            if (playerSideBricks <= 1 || enemySideBricks <= 1)
            {
                GameOver();
            }
        }
        else
        {
            numberOfBricks = GameObject.FindGameObjectsWithTag("Brick").Length;
            if (score >= 20)
            {
                nextLevelPanel();
            }
        }
    }
    void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        if (gameOverPanel.activeSelf)
        {
            bscript.ballrb.velocity = Vector2.zero;
            bscript.inPlay = false;
        }
    }
    public void UpdateLives(int changeInLives)
    {
        lives += changeInLives;

        if (lives <= 0)
        {
            lives = 0;
            GameOver();
        }

        livesText.text = "Lives: " + lives;
    }
    public void UpdateScore(int points)
    {

        score += points;

        scoreText.text = "Score:" + score;
    }
    public void PlayAgain()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void UpdateNumberOfBricks()
    {

        numberOfBricks--;
    }
    private int CountBricksWithId(int brickId)
    {
        int count = 0;

        // Count bricks with the given BrickId
        BrickType[] bricks = FindObjectsOfType<BrickType>();
        foreach (BrickType brick in bricks)
        {
            if (brick.BrickId == brickId)
            {
                count++;
            }
        }

        return count;
    }
    public void Back()
    {
        SceneManager.LoadScene("BB_Level");

    }
    void nextLevelPanel()
    {
        gameOver = true;
        NextLevelPanel.SetActive(true);
        if (NextLevelPanel.activeSelf)
        {
            bscript.ballrb.velocity = Vector2.zero;
            bscript.inPlay = false;
        }

    }
    public void go_to_FifthLevel()
    {
        SceneManager.LoadScene("Main_4");
    }
}
