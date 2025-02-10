using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManagerW : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject NextLevelPanel;
    public bool isgameover;
    public int nextlevelScore;
    private HoleSelector hs;
    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        isgameover=false;
        hs=FindObjectOfType<HoleSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameOverPanel.SetActive(true);
            isgameover=true;
        }
        if(hs.score>=nextlevelScore){
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
        SceneManager.LoadScene("Whack_Lvl2");
    }
     public void onclic_3rdlevel(){
        SceneManager.LoadScene("Whack_Lvl3");
    }
     public void onclic_4thlevel(){
        SceneManager.LoadScene("Whack_Lvl4");
    }
}
