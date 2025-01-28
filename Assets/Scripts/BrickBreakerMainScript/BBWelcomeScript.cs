using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class BBWelcomeScript : MonoBehaviour
{
    public void onclick_start()
    {
        SceneManager.LoadScene("BB_Level");
    }
    public void onclick_Back()
    {
        // SceneManager.LoadScene("#anything"); can add additional scene or just quit the application
        Application.Quit();
    }
    public void onClick_lvl_1()
    {
        SceneManager.LoadScene("Main_0");
    }
    public void onClick_lvl_2()
    {
        SceneManager.LoadScene("Main_1");
    }
    public void onClick_lvl_3()
    {
        SceneManager.LoadScene("Main_2");
    }
    public void onClick_lvl_4()
    {
        SceneManager.LoadScene("Main_3");
    }
    public void onClick_lvl_5()
    {
        SceneManager.LoadScene("Main_4");
    }
    public void onclick_back()
    {
        SceneManager.LoadScene("BB_Welcome");
    }
}
