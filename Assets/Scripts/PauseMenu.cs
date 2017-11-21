using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{

    public bool isPaused;
    public bool muted = false;

    public GameObject pauseMenuCanvas;
    public GameObject endMenuCanvas;

    // Update is called once per frame
    void Update()
    {
    }

    public void Resume()
    {
        isPaused = false;
        if (!isPaused)
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            if (!muted)
            {
                GameObject.Find("BGM1").GetComponent<AudioSource>().UnPause();
            }
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("mainMenu");
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
        Time.timeScale = 1f;
    }

    public void BtnPause()
    {
        isPaused = true;
        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            if (muted)
            {
                GameObject.Find("BGM1").GetComponent<AudioSource>().Pause();
            }
        }
    }

    public void BtnEnd()
    {
        isPaused = true;
        if (isPaused)
        {
            endMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ButtonMute()
    {
        muted = !muted;
        if (muted)
        {
            GameObject.Find("BGM1").GetComponent<AudioSource>().Pause();
        }
        else
        {
            GameObject.Find("BGM1").GetComponent<AudioSource>().UnPause();
        }
    }

}



