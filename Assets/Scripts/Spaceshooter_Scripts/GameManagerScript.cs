using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public GameObject GameOverPanel;
    public TextMeshProUGUI timerText;
    public bool Levelunlocked = false;
    public bool isGameOver = false; // Tracks the game over state
    public float gameDuration = 90f; // Game duration in seconds
    public GameObject newSpaceshipPanel;
    private int finalScore;
    private int currentLevel;
    private string path = Path.Combine(Application.dataPath, "Patient_Data", "ScoreManager.csv");
    private PlayerScore Playerscore;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Playerscore = FindObjectOfType<PlayerScore>();
        timer = gameDuration; // Initialize timer 
        GameOverPanel.SetActive(false);
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            string[] values = lines[0].Split(','); // Split the line by commas
            if (values.Length >= 2)
            {

                int.TryParse(values[1], out currentLevel); // Parse the second value as currentLevel
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape) || isGameOver)
        {
            // GameOverPanel.SetActive(true);
            GameOver();
        }
        else
        { // Countdown the timer
            timer -= Time.deltaTime;

            // Update the timer display
            if (timerText != null)
            {
                timerText.text = "Time:" + Mathf.CeilToInt(timer).ToString() + "s"; // Show remaining time
            }
        }

        // Check if the timer has reached zero
        if (timer <= 0)
        {
            timer = 0;
            GameOver();
        }




    }
    private void savedata()
    {

        //using the currentscore variable in playerscore to append the player score in csv file when game ends
        int currentscores = Playerscore.currentScore;
        string score = $"{currentscores},{currentLevel}";
        File.WriteAllText(path, score);


    }

    public void GameOver()
    {
        if (Levelunlocked == false)
        {

            GameOverPanel.SetActive(true);
            savedata();
        }

        timerText.text = "Time:0s";
        isGameOver = true; // Set game over state 


    }
    public void restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void Back_to_ChooseLevel()
    {
        SceneManager.LoadScene("space_shooter_level");
    }
    public void UnlockShipPanel()
    {
        if (newSpaceshipPanel != null)
        {
            newSpaceshipPanel.SetActive(true);
            Levelunlocked = true;
            GameOver();

        }

    }
    public void secondlevel()
    {

        finalScore = 0;
        if (currentLevel <= 2)
        {
            currentLevel = 2;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level2");
    }
    public void thirdlevel()
    {
        finalScore = 0;
        if (currentLevel <= 3)
        {
            currentLevel = 3;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level3");

    }
    public void fourthlevel()
    {
        finalScore = 0;
        if (currentLevel <= 4)
        {
            currentLevel = 4;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level4");

    }
    public void fifthlevel()
    {
        finalScore = 0;
        if (currentLevel <= 5)
        {
            currentLevel = 5;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level5");

    }
    public void sixthlevel()
    {
        finalScore = 0;
        if (currentLevel <= 6)
        {
            currentLevel = 6;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level6");

    }
    public void seventhlevel()
    {
        finalScore = 0;
        if (currentLevel <= 7)
        {
            currentLevel = 7;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level7");
    }
      public void eigthlevel()
    {
        finalScore = 0;
        if (currentLevel <= 8)
        {
            currentLevel = 8;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level8");
    }
    public void ninthlevel()
    {
        finalScore = 0;
        if (currentLevel <= 9)
        {
            currentLevel = 9;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level9");
    }
    public void tenthlevel() {
        finalScore = 0;
        if (currentLevel <= 10)
        {
            currentLevel = 10;
        }
        string data = $"{finalScore},{currentLevel}";
        File.WriteAllText(path, data);
        SceneManager.LoadScene("SpaceShooter_Level9");
    }

}
