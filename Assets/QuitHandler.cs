using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitHandler : MonoBehaviour
{
    public CanvasGroup uiCanvasGroup;
    public CanvasGroup confirmQuitCanvasGroup;

    public GameObject endMenuCanvas;

    // Use this for initialization
    private void Awake()
    {
        //disable the quit confirmation panel
        DoConfirmQuitNo();
    }

    /// <summary>
    /// Called if clicked on No (confirmation)
    /// </summary>
    public void DoConfirmQuitNo()
    {
        Debug.Log("Back to the game");
        Time.timeScale = 1f;
        //enable the normal ui
        uiCanvasGroup.alpha = 1;
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;

        //disable the confirmation quit ui
        confirmQuitCanvasGroup.alpha = 0;
        confirmQuitCanvasGroup.interactable = false;
        confirmQuitCanvasGroup.blocksRaycasts = false;

    }

    /// <summary>
    /// Called if clicked on Yes (confirmation)
    /// </summary>
    public void DoConfirmQuitYes()
    {
        //enable the normal ui
        uiCanvasGroup.alpha = 1;
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;

        //disable the confirmation quit ui
        confirmQuitCanvasGroup.alpha = 0;
        confirmQuitCanvasGroup.interactable = false;
        confirmQuitCanvasGroup.blocksRaycasts = false;

        Debug.Log("Ok go game over screen");
        //Application.Quit();
        //UnityEngine.SceneManagement.SceneManager.LoadScene("mainMenu");

        endMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Called if clicked on Quit
    /// </summary>
    public void DoQuit()
    {
        Debug.Log("Check form quit confirmation");
        Time.timeScale = 0f;
        //reduce the visibility of normal UI, and disable all interraction
        uiCanvasGroup.alpha = 0.5f;
        uiCanvasGroup.interactable = false;
        uiCanvasGroup.blocksRaycasts = false;

        //enable interraction with confirmation gui and make visible
        confirmQuitCanvasGroup.alpha = 1;
        confirmQuitCanvasGroup.interactable = true;
        confirmQuitCanvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Called if clicked on new game (example)
    /// </summary>
    public void DoNewGame()
    {
        Debug.Log("Launch a new game");
    }

}
