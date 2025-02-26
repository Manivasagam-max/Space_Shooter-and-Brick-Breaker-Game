using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManagerW : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject NextLevelPanel;
    public bool isgameover;
    public int requiredscore;
    private HoleSelector hs;
    public MoleController[] moles;
    public float minWait = 1f;
    public float maxWait = 3f;
    private float Timer;
    public float gameDuration = 60f; 
    public TextMeshProUGUI TimerText;
    public float postDisappearWait=0.5f;
    private string filepath = Path.Combine(Application.dataPath, "Patient_Data", "Whack_Score.csv");

    private List<int> recentHoles = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        isgameover=false;
        
        hs=FindObjectOfType<HoleSelector>();
        StartCoroutine(SpawnMoles());
        Timer = gameDuration;
    }
    IEnumerator SpawnMoles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            // Pick a random mole without repeating recent ones
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, moles.Length);
            }
            while (recentHoles.Contains(randomIndex) && moles.Length > 2);

            recentHoles.Add(randomIndex);
            if (recentHoles.Count > 2) recentHoles.RemoveAt(0);

            // Activate the mole under the selected hole
            moles[randomIndex].StartMoleRoutine();
            yield return new WaitForSeconds(postDisappearWait);
        }
    }

    // Update is called once per frame
    void Update()
    {
         Timer -= Time.deltaTime;
                if (TimerText != null)
                {
                    TimerText.text = "Time: " + Mathf.CeilToInt(Timer) + "s";
                }
        if (Input.GetKeyDown(KeyCode.Escape)|| Timer<=0)
        {
            gameOverPanel.SetActive(true);
            isgameover=true;
            Timer=0;
            StopAllCoroutines();
        }
        if(hs.score>=requiredscore){
            NextLevelPanel.SetActive(true);
            isgameover=true;
        }
    }
    public void onclick_Back(){
        SceneManager.LoadScene("Whack_levels");
    }
    public void onclick_Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void onclic_2ndlevel(){
         if (File.Exists(filepath))
        {
            string data = "0,2";
            File.WriteAllText(filepath, data);
        }
        SceneManager.LoadScene("Whack_Lvl2");
    }
     public void onclic_3rdlevel(){
         if (File.Exists(filepath))
        {
            string data = "0,3";
            File.WriteAllText(filepath, data);
        }
        SceneManager.LoadScene("Whack_Lvl3");
    }
     public void onclic_4thlevel(){
         if (File.Exists(filepath))
        {
            string data = "0,4";
            File.WriteAllText(filepath, data);
        }
        SceneManager.LoadScene("Whack_Lvl4");
    }
}
