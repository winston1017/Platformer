using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string startLevel;
    string gameModeKey = "GameMode";

    public bool chosenGame;
    

    public void NewGame()
    {
        //SceneManager.LoadScene(1);
        PlayerPrefs.SetString(gameModeKey, "Story_Mode");
        PlayerPrefs.Save();
        SceneManager.LoadScene("StoryLevelOne");
    }
    public void OldGame()
    {
        //SceneManager.LoadScene(1);
        PlayerPrefs.SetString(gameModeKey, "Arcade_Mode");
        PlayerPrefs.Save();
        SceneManager.LoadScene("levelTest");

        //SceneManager.LoadScene("ArcadeMode");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
