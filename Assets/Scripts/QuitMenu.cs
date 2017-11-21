using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class QuitMenu : MonoBehaviour
{

    public bool isPaused;

    public GameObject pauseMenuCanvas;

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

    public void BtnQuit()
    {
        isPaused = true;
        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    

}



