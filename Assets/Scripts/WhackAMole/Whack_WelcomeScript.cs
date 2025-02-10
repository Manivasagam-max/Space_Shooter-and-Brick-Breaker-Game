using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Whack_WelcomeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void start(){
        SceneManager.LoadScene("Whack_Levels");
    }
    public void Back(){
        // SceneManager.LoadScene("Back");
        Application.Quit();
    }
    public void lvl_1(){
        SceneManager.LoadScene("Whack_Lvl1");
    }
     public void lvl_2(){
        SceneManager.LoadScene("Whack_Lvl2");
    }
     public void lvl_3(){
        SceneManager.LoadScene("Whack_Lvl3");
    }
     public void lvl_4(){
        SceneManager.LoadScene("Whack_Lvl4");
    }
}
