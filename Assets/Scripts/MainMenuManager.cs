using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   
    public void ButtonExitClick()
    {
        Application.Quit();
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
