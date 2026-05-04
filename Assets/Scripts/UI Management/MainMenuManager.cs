using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        LevelLoader.LoadScene(3);
    }

    public void LevelSelect()
    {
        LevelLoader.LoadScene(2);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
