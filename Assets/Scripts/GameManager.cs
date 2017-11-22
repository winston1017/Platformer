using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public float numKills = 0; //use for increasing enemy dmg

    public GameObject endMenuCanvas;
    [SerializeField]
    private float playerRangedDmg;
    [SerializeField]
    private float playerMeleeDmg;
    [SerializeField]
    private float enemyRangedDmg;
    [SerializeField]
    private float enemyMeleeDmg;
    [SerializeField]
    private int enemyLevel;
    [SerializeField]
    private int enemyHealth;

    [SerializeField]
    private string gameMode;

    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private GameObject burgerPrefab;

    [SerializeField]
    private Text coinTxt;
    [SerializeField]
    private Text deathTxt;
    [SerializeField]
    private Text killTxt;
    [SerializeField]
    private Text gameOverKillTxt;

    [SerializeField]
    private Text highScoreTxt;
    [SerializeField]
    private Text rangedTxt;
    [SerializeField]
    private Text meleeTxt;
    [SerializeField]
    private int collectedCoins;

    [SerializeField]
    private int gameOverSavedCollectedCoins;
    [SerializeField]
    private Text gameOverSavedCollectedCoinsTxt;

    [SerializeField]
    private int totalSavedCollectedCoins;
    [SerializeField]
    private Text totalSavedCollectedCoinsTxt;

    [SerializeField]
    private Text meleeCostTxt;
    [SerializeField]
    private Text rangedCostTxt;

    private int deathCount;
    private int killCount;
    private int rangedLevel = 1;
    private int meleeLevel = 1;
    private int meleeCost = 10;
    private int rangedCost = 10;

    private int highScore = 0;
    string highScoreKey = "HighScore";

    private int ArcadeHighScore = 0;
    string ArcadeHighScoreKey = "ArcadeScore";

    public bool SelectedGameMode;

    //private string gameChosen = "";
    string gameModeKey = "GameMode";

    private int arcadeKillsReq = 5;
    private int levelOneReqKills = 3;
    //private int levelTwoReqKills = 2;

    //This is to show how much player has collected
    //private int savedCollectedCoins = 0;
    //Grab collected amount
    string savedCollectedCoinsKey = "CollectedCoins";

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    public GameObject CoinPrefab
    {
        get
        {
            return coinPrefab;
        }

    }
    public GameObject BurgerPrefab
    {
        get
        {
            return burgerPrefab;
        }

    }

    public int CollectedCoins
    {
        get
        {
            return collectedCoins;
        }

        set
        {
            coinTxt.text = value.ToString();
            collectedCoins = value;
        }
    }

    public int DeathCount
    {
        get
        {
            return deathCount;
        }

        set
        {
            deathTxt.text = value.ToString() + " / 3";
            deathCount = value;
        }
    }

    public int KillCount
    {
        get
        {
            return killCount;
        }

        set
        {
            gameOverKillTxt.text = value.ToString(); //gameover score
            killTxt.text = value.ToString();
            killCount = value;
        }
    }

    public float PlayerRangedDmg
    {
        get
        {
            return playerRangedDmg;
        }

        set
        {
            playerRangedDmg = value;
        }
    }

    public float PlayerMeleeDmg
    {
        get
        {
            return playerMeleeDmg;
        }

        set
        {
            playerMeleeDmg = value;
        }
    }

    public float EnemyRangedDmg
    {
        get
        {
            return enemyRangedDmg;
        }

        set
        {
            enemyRangedDmg = value;
        }
    }

    public float EnemyMeleeDmg
    {
        get
        {
            return enemyMeleeDmg;
        }

        set
        {
            enemyMeleeDmg = value;
        }
    }

    public int RangedLevel
    {
        get
        {
            return rangedLevel;
        }

        set
        {
            rangedTxt.text = value.ToString();
            rangedLevel = value;
        }
    }

    public int MeleeLevel
    {
        get
        {
            return meleeLevel;
        }

        set
        {
            meleeTxt.text = value.ToString();
            meleeLevel = value;
        }
    }

    public int MeleeCost
    {
        get
        {
            return meleeCost;
        }

        set
        {
            meleeCostTxt.text = value.ToString();
            meleeCost = value;
        }
    }

    public int RangedCost
    {
        get
        {
            return rangedCost;
        }

        set
        {
            rangedCostTxt.text = value.ToString();
            rangedCost = value;
        }
    }

    public float NumKills
    {
        get
        {
            return numKills;
        }

        set
        {
            numKills = value;
        }
    }

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

    public int GameOverSavedCollectedCoins
    {
        get
        {
            return gameOverSavedCollectedCoins;
        }

        set
        {
            gameOverSavedCollectedCoinsTxt.text = value.ToString();
            gameOverSavedCollectedCoins = value;
        }
    }

    public int EnemyHealth
    {
        get
        {
            return enemyHealth;
        }

        set
        {
            enemyHealth = value;
        }
    }

    public int EnemyLevel
    {
        get
        {
            return enemyLevel;
        }

        set
        {
            enemyLevel = value;
        }
    }

    public string GameMode
    {
        get
        {
            return gameMode;
        }

        set
        {
            gameMode = value;
        }
    }

    public bool SelectedGameMode1
    {
        get
        {
            return SelectedGameMode;
        }

        set
        {
            SelectedGameMode = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        gameMode = PlayerPrefs.GetString(gameModeKey, "Arcade_Mode");

        //Initialize values
        EnemyLevel = 1;
        playerRangedDmg = 10;
        playerMeleeDmg = 10;
        enemyRangedDmg = 10;
        enemyMeleeDmg = 10;
        enemyHealth = 30;
        Debug.Log("inside gm start" + SelectedGameMode);
        if (SelectedGameMode)
        {
            Debug.Log("selected");
            if (gameMode == "Arcade_Mode")
            {
                CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "ARCADE MODE", Color.yellow, true, 3f, true);
            }
            else if (gameMode == "Story_Mode")
            {
                CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "STORY MODE \n LEVEL ONE", Color.yellow, true, 3f, true);
            }
        }
        //PlayerPrefs.SetInt(savedCollectedCoinsKey, 0); - resetting for test
        killCount = 0;
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreTxt.text = highScore.ToString();

        totalSavedCollectedCoins = PlayerPrefs.GetInt(savedCollectedCoinsKey, 0);
        totalSavedCollectedCoinsTxt.text = totalSavedCollectedCoins.ToString();
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (deathCount >= 3)
        {
            endMenuCanvas.SetActive(true);
            Time.timeScale = 0f;

            //calculate new coins
            gameOverSavedCollectedCoins = Mathf.CeilToInt(killCount * 0.088f);

            if (gameOverSavedCollectedCoins > 0)
            {
                //display new coins
                gameOverSavedCollectedCoinsTxt.text = gameOverSavedCollectedCoins.ToString() + " added! ";

                //add new coins to existing coins
                totalSavedCollectedCoins += gameOverSavedCollectedCoins;
                totalSavedCollectedCoinsTxt.text = totalSavedCollectedCoins.ToString() + "!";
            }
            else
            {
                gameOverSavedCollectedCoinsTxt.text = "0 :(";
                totalSavedCollectedCoinsTxt.text = totalSavedCollectedCoins.ToString() + "!";
            }
            //save all coins
            PlayerPrefs.SetInt(savedCollectedCoinsKey, totalSavedCollectedCoins);
            PlayerPrefs.Save();
        }

        if (gameMode == "Arcade_Mode")
        {
            if (killCount > highScore)
            {
                ArcadeHighScore = killCount;
                highScoreTxt.text = "" + killCount;

                PlayerPrefs.SetInt(ArcadeHighScoreKey, ArcadeHighScore);
                PlayerPrefs.Save();
            }

            if (numKills > arcadeKillsReq)
            {
                enemyHealth += 10;
                enemyLevel++;
                enemyRangedDmg++;
                enemyMeleeDmg++;
                CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "Enemies got stronger", Color.yellow, true, 3f, true);
                numKills = 0;
            }
        }
        if (gameMode == "Story_Mode")
        {
            if (killCount > highScore)
            {
                highScore = killCount;
                highScoreTxt.text = "" + killCount;

                PlayerPrefs.SetInt(highScoreKey, highScore);
                PlayerPrefs.Save();
            }

            if (numKills == levelOneReqKills)
            {
                enemyHealth = 60;
                enemyLevel = 10;
                CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "LEVEL TWO OPEN ->", Color.yellow, true, 3f, true);
            }
        }
    }
}
