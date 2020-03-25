using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isGamePaused = false;

    public void PauseKeyPressed() {
        if (!GameManager.Instance.GetPlayer().GetComponent<PlayerScript>().isDead()) {
            if (isGamePaused = !isGamePaused)
                Pause();
            else
                Resume();
        }
    }

    private void Pause() {
        Cursor.visible = true;
        gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void Resume() {
        Cursor.visible = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void MainMenu() {
        Cursor.visible = true;
        SceneManager.LoadScene("MenuScene");
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
