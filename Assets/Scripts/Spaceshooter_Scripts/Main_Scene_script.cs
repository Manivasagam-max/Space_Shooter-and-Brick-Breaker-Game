using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Scene_script : MonoBehaviour
{

    public void onClick_gotoLevel()
    {
        SceneManager.LoadScene("space_shooter_level");
    }
    public void onclick_gotochoose()
    {
        SceneManager.LoadScene("Main");
    }
}
