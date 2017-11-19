using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    [SerializeField]
    private int playerRangedDmg = 10;
    [SerializeField]
    private int playerMeleeDmg = 10;
    [SerializeField]
    private int enemyRangedDmg = 10;
    [SerializeField]
    private int enemyMeleeDmg = 10;

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
    private Text rangedTxt;
    [SerializeField]
    private Text meleeTxt;
    [SerializeField]
    private int collectedCoins;
    private int deathCount;
    private int killCount;

    private int rangedLevel = 1;
    private int meleeLevel = 1;

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

    public int PlayerRangedDmg
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

    public int PlayerMeleeDmg
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

    public int EnemyRangedDmg
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

    public int EnemyMeleeDmg
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




    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
