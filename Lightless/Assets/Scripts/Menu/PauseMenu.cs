using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isGamePaused = false;

    public void PauseKeyPressed() {
        if (isGamePaused = !isGamePaused)
            Pause();
        else
            Resume();
    }

    private void Pause() {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu() {
        SceneManager.LoadScene("MenuScene");
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
