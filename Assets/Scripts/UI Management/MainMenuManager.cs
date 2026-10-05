using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Transform mainMenu, levelSelect;

    public void Play()
    {
        LevelLoader.LoadScene(2);
    }

    public void LevelEditor()
    {
        LevelLoader.LoadScene(3);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        mainMenu.gameObject.SetActive(false);
        levelSelect.gameObject.SetActive(true);
    }

    public void Return()
    {
        mainMenu.gameObject.SetActive(true);
        levelSelect.gameObject.SetActive(false);
    }
}
