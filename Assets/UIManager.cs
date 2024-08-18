using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public InputActionReference pause;
    public GameObject pauseMenu;

    private void Start()
    {
        title.text = SceneManager.GetActiveScene().name;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (pause.action.WasPressedThisFrame())
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
