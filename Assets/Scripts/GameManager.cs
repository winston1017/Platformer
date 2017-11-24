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
    private float playerRangedDmg = 9;
    [SerializeField]
    private float playerMeleeDmg = 11;
    [SerializeField]
    private float enemyRangedDmg = 9;
    [SerializeField]
    private float enemyMeleeDmg = 12;
    [SerializeField]
    private int enemyLevel = 1;
    [SerializeField]
    private int enemyHealth = 30;
    [SerializeField]
    private int userSelectDeathCount = 3;
    [SerializeField]
    private string gameMode;

    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private GameObject burgerPrefab;
    [SerializeField]
    private GameObject zombieFemalePrefab;
    [SerializeField]
    private GameObject cratePrefab;
    [SerializeField]
    private GameObject bushPrefab;
    [SerializeField]
    private GameObject bush2Prefab;
    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private GameObject stoneShroomPrefab;
    [SerializeField]
    private GameObject tree1Prefab;
    [SerializeField]
    private GameObject tree2Prefab;
    [SerializeField]
    private GameObject tree3Prefab;


    [SerializeField]
    private GameObject playerNinjaPrefab;

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
    private Text storyHighScoreTxt;


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
    [SerializeField]
    private int deathCount;
    [SerializeField]
    private int killCount;
    private int rangedLevel = 1;
    private int meleeLevel = 1;
    private int meleeCost = 10;
    private int rangedCost = 10;

    private int highScore = 0;
    string highScoreKey = "HighScore";

    private int storyHighScore = 0;
    string storyHighScoreKey = "StoryScore";

    //private string gameChosen = "";
    string gameModeKey = "GameMode";

    private int arcadeKillsReq = 7;
    private int levelOneReqKills = 20;

    private int storyLevelUnlock = 0;
    private int announcedUnlock = 0;


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

    //public int EnemyLevel
    //{
    //    get
    //    {
    //        return enemyLevel;
    //    }

    //    set
    //    {
    //        enemyLevel = value;
    //    }
    //}

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

    public int StoryLevelUnlock
    {
        get
        {
            return storyLevelUnlock;
        }

        set
        {
            storyLevelUnlock = value;
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

    // Use this for initialization
    void Start()
    {
        
        gameMode = PlayerPrefs.GetString(gameModeKey, "Arcade_Mode");
        EnemyLevel = 1;
        EnemyHealth = 30;
        //Initialize values
        if (gameMode == "Arcade_Mode")
        {
            CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "ARCADE MODE", Color.yellow, true, 3f, true);
        }
        else if (gameMode == "Story_Mode")
        {
            CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "STORY MODE \n LEVEL ONE", Color.yellow, true, 3f, true);
        }
        //PlayerPrefs.SetInt(savedCollectedCoinsKey, 0); - resetting for test
        killCount = 0;
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreTxt.text = highScore.ToString();

        storyHighScore = PlayerPrefs.GetInt(storyHighScoreKey, 0);
        storyHighScoreTxt.text = storyHighScore.ToString();

        totalSavedCollectedCoins = PlayerPrefs.GetInt(savedCollectedCoinsKey, 0);
        totalSavedCollectedCoinsTxt.text = totalSavedCollectedCoins.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (deathCount == userSelectDeathCount)
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

        else if (gameMode == "Arcade_Mode")
        {
            if (killCount > highScore)
            {
                highScore = killCount;
                highScoreTxt.text = "" + killCount;

                PlayerPrefs.SetInt(highScoreKey, highScore);
                PlayerPrefs.Save();
            }

            if (numKills == arcadeKillsReq)
            {
                enemyLevel++;
                enemyHealth += 10;
                enemyRangedDmg++;
                enemyMeleeDmg++;
                CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "Enemies got stronger", Color.yellow, true, 3f, true);
                numKills = 0;
            }
        }
        else if (gameMode == "Story_Mode")
        {
            if (killCount > storyHighScore)
            {
                storyHighScore = killCount;
                storyHighScoreTxt.text = "" + killCount;

                PlayerPrefs.SetInt(storyHighScoreKey, storyHighScore);
                PlayerPrefs.Save();
            }

            if (numKills == levelOneReqKills && StoryLevelUnlock == 0)
            {
                //StoryLevelUnlock is now level1
                StoryLevelUnlock++;

                enemyHealth += 20;
                enemyLevel++;
                if (announcedUnlock == 0)
                {
                    announcedUnlock++;
                    Player.Instance.startPos = new Vector2(76.99f, 18f);
                    CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "LEVEL TWO OPEN ->", Color.yellow, true, 3f, true);

                    //DestroyLevel1Barrier
                    GameObject killBarrier = GameObject.Find("LevelBarrier1");
                    Destroy(killBarrier);

                    //Set min camera and max (maybe remove?)
                    CameraFollow.Instance.XMin = Player.Instance.transform.position.x;
                    CameraFollow.Instance.XMax = 267f;
                    //Put down a camera barreir
                    GameObject CameraBarrier = Instantiate(Resources.Load("CameraBarrier"), new Vector3(Player.Instance.transform.position.x - 15.5f, Player.Instance.transform.position.y, Player.Instance.transform.position.z), Quaternion.identity) as GameObject;

                    //Look for Level2Spawns
                    GameObject[] l2spawns;
                    l2spawns = GameObject.FindGameObjectsWithTag("Level2Spawn");
                    foreach (GameObject go in l2spawns)
                    {
                        GameObject EnemyL2 = (GameObject)Instantiate(GameManager.Instance.zombieFemalePrefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-2.6f, 2.0f)), go.transform.position.y, go.transform.position.z), Quaternion.identity);
                        EnemyL2.transform.localScale = new Vector3(EnemyL2.transform.localScale.x * 1.15f, EnemyL2.transform.localScale.y * 0.9f, EnemyL2.transform.localScale.z);
                        EnemyL2.transform.parent = go.transform.parent; //IMPORTANT: SET BACK TO ORIGINAL HIERARCHY SO ENEMIES CAN LOOK FOR "EDGES"

                        if (UnityEngine.Random.Range(1, 6) == 5)
                        {
                            GameObject Crate = (GameObject)Instantiate(GameManager.Instance.cratePrefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                        }
                        if (UnityEngine.Random.Range(1, 3) == 1)
                        {
                            int randomBushNum = UnityEngine.Random.Range(1, 4);
                            if (randomBushNum == 1)
                            {
                                GameObject Bush = (GameObject)Instantiate(GameManager.Instance.bushPrefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                                GameObject Bush1 = (GameObject)Instantiate(GameManager.Instance.bushPrefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                            else if (randomBushNum == 2)
                            {
                                GameObject Bush2 = (GameObject)Instantiate(GameManager.Instance.bush2Prefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                                GameObject Bush3 = (GameObject)Instantiate(GameManager.Instance.bush2Prefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                            else if (randomBushNum == 3)
                            {
                                GameObject Bush4 = (GameObject)Instantiate(GameManager.Instance.bushPrefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                                GameObject Bush5 = (GameObject)Instantiate(GameManager.Instance.bush2Prefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                        }
                        if (UnityEngine.Random.Range(1, 3) == 2)
                        {
                            int randomStoneNum = UnityEngine.Random.Range(1, 3);
                            if (randomStoneNum == 1)
                            {
                                GameObject StoneShroom = (GameObject)Instantiate(GameManager.Instance.stoneShroomPrefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                            else if (randomStoneNum == 2)

                            {
                                GameObject Stone = (GameObject)Instantiate(GameManager.Instance.stonePrefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                        }
                        if (UnityEngine.Random.Range(1, 3) == 1)
                        {
                            int randomTreeNum = UnityEngine.Random.Range(1, 4);
                            if (randomTreeNum == 1)
                            {
                                GameObject Tree = (GameObject)Instantiate(GameManager.Instance.tree1Prefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                            else if (randomTreeNum == 2)
                            {
                                GameObject Tree1 = (GameObject)Instantiate(GameManager.Instance.tree2Prefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                            else if (randomTreeNum == 3)
                            {
                                GameObject Tree2 = (GameObject)Instantiate(GameManager.Instance.tree3Prefab, new Vector3(go.transform.position.x + (UnityEngine.Random.Range(-4.6f, 4.0f)), go.transform.position.y + 1, go.transform.position.z), Quaternion.identity);
                            }
                        }

                        //GameObject EnemyL2 = Instantiate(zombieFemalePrefab, go.transform.position, go.transform.rotation) as GameObject;
                        Destroy(go);
                    }

                }
            }
        }
    }
}
