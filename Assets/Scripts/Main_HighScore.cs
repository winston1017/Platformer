using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_HighScore : MonoBehaviour
{
    
    [SerializeField]
    private Text highScoreTxt;
    [SerializeField]
    private int totalSavedCollectedCoins;
    [SerializeField]
    private Text totalSavedCollectedCoinsTxt;
    

    private int highScore = 0;
    string highScoreKey = "HighScore";

    private int ArcadeHighScore = 0;
    string ArcadeHighScoreKey = "ArcadeScore";
    string savedCollectedCoinsKey = "CollectedCoins";


    public int TotalSavedCollectedCoins
    {
        get
        {
            return totalSavedCollectedCoins;
        }

        set
        {
            totalSavedCollectedCoinsTxt.text = value.ToString();
            totalSavedCollectedCoins = value;
        }
    }
    

    // Use this for initialization
    void Start()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreTxt.text = highScore.ToString();

        totalSavedCollectedCoins = PlayerPrefs.GetInt(savedCollectedCoinsKey, 0);
        totalSavedCollectedCoinsTxt.text = totalSavedCollectedCoins.ToString();
    }

    
}
