using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PauseMenu : MonoBehaviour {

    //public string mainMenu;
    public bool isPaused;

    public GameObject pauseMenuCanvas;
    
	// Update is called once per frame
	void Update () {
		if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;

        }
        else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
	}

    public void Resume()
    {
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

    public void BtnPause()
    {
        isPaused = true;
    }
}
