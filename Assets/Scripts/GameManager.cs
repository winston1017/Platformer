using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    [SerializeField]
    private float playerRangedDmg = 10;
    [SerializeField]
    private float playerMeleeDmg = 10;
    [SerializeField]
    private float enemyRangedDmg = 10;
    [SerializeField]
    private float enemyMeleeDmg = 10;

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
    private Text highScoreTxt;
    [SerializeField]
    private Text rangedTxt;
    [SerializeField]
    private Text meleeTxt;
    [SerializeField]
    private int collectedCoins;
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
            deathTxt.text = value.ToString();
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




    // Use this for initialization
    void Start()
    {
        killCount = 0;
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreTxt.text = highScore.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (killCount > highScore)
        {
            highScore = killCount;
            highScoreTxt.text = "" + killCount;

            PlayerPrefs.SetInt(highScoreKey, highScore);
            PlayerPrefs.Save();
        }
    }
}
