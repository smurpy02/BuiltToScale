using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static World world;

    public void Play()
    {
        SceneTransition.TransitionScene(3);
    }

    public void LevelSelect()
    {
        SceneTransition.TransitionScene(2);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
