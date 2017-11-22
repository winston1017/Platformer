using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{

    private IEnemyState currentState;

    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private float throwRange;
    [SerializeField]
    private float chaseRangeHi;
    [SerializeField]
    private float chaseRangeLo;

    private Vector3 startPos;
    private int monsterLevel = 1;

    //[SerializeField]
    //private Transform leftEdge;
    //[SerializeField]
    //private Transform rightEdge;

    private Canvas healthCanvas;

    //private bool coinDropped;
    private float currentReceiveDamage;
    private int skyMoneyDropNum = 8;

    private bool gotCrit;

    //Fall off respawn timer count
    private float time;
    private bool fellOff;
    private bool rained = false;

    public AudioClip swingSound;
    public AudioClip takeMeleeSound;
    public AudioClip takeRangedSound;

    public AudioClip coinRain;
    public AudioClip drownSound;

    private AudioSource source;

    public GameObject Target { get; set; }

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {

                return (Mathf.Abs(transform.position.x - Target.transform.position.x)) <= meleeRange;
                //return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    //Feature: To not let enemy throw when logically too close to throw objects - chase instead
    public bool InChaseRange
    {
        get
        {
            if (Target != null)
            {

                return (Mathf.Abs(transform.position.x - Target.transform.position.x) <= chaseRangeHi) && (Mathf.Abs(transform.position.x - Target.transform.position.x) <= chaseRangeLo);
                //return (Vector2.Distance(transform.position, Target.transform.position) <= chaseRangeHi) && (Vector2.Distance(transform.position, Target.transform.position) >= chaseRangeLo);
            }
            return false;
        }
    }

    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                // return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
                return (Mathf.Abs(transform.position.x - Target.transform.position.x)) <= throwRange;
            }
            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return healthStat.CurrentVal <= 0;
        }
    }

    public override void Start()
    {

        base.Start();
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
        healthCanvas = transform.GetComponentInChildren<Canvas>();
        startPos = transform.position;
        source = GetComponent<AudioSource>();
    }

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }

    //FUNCTION: Face the correct direction
    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDirection = Target.transform.position.x - transform.position.x;

            if (xDirection < 0 && facingRight || xDirection > 0 && !facingRight)
            {
                //FIX FOR SPINNING ENEMIES
                if (Vector2.Distance(transform.position, new Vector2(transform.position.x, Target.transform.position.y)) > 5)
                {
                    RemoveTarget();
                }
                ChangeDirection();
            }

        }
    }

    void Update()
    {
        if (GameManager.Instance.GameMode == "Story_Mode")
        {
            if (monsterLevel < GameManager.Instance.StoryLevelUnlock + 1)
            {
                Destroy(gameObject);
            }
        }
        if (!IsDead && this.tag != "EnemyDead")
        {
            if (!TakingDamage)
            {
                if (transform.position.y <= -14f) //every time it updates, enemy position is still -14f, so it rains coins like crazy
                {
                    //Double lock
                    if (fellOff == false)
                    {
                        source.PlayOneShot(drownSound, 0.5F);
                        fellOff = true;
                        RainingCollectibles(fellOff);
                    }

                    //Respawn Timer for falling off use
                    time += Time.deltaTime;
                    if (time >= 12)
                    {
                        Death();
                    }
                }
                currentState.Execute();
            }
            LookAtTarget();
            if (monsterLevel < GameManager.Instance.EnemyLevel)
            {
                monsterLevel = GameManager.Instance.EnemyLevel;

                healthStat.MaxVal = GameManager.Instance.EnemyHealth;
                healthStat.CurrentVal = GameManager.Instance.EnemyHealth;
            }
        }
    }

    void FixedUpdate()
    {
        //if (GameManager.Instance.GameMode == "Arcade_Mode")
        //{

        //}
        
    }

    void RainingCollectibles(bool fellOff)
    {
        if (fellOff && !rained)
        {
            rained = true;

            CombatTextManager.Instance.CreateBigText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "KNOCKED OFF", Color.red, true, 2f, true);
            source.PlayOneShot(coinRain, 0.5F);

            //Rain One burger
            //GameObject burger = (GameObject)Instantiate(GameManager.Instance.BurgerPrefab, new Vector3(UnityEngine.Random.Range(-43, 43), 18), Quaternion.identity);
            GameObject burger = (GameObject)Instantiate(GameManager.Instance.BurgerPrefab, new Vector3(Camera.main.gameObject.transform.position.x + UnityEngine.Random.Range(-16.5f,16.5f), 18), Quaternion.identity);
            Physics2D.IgnoreCollision(burger.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            //Increase score
            Debug.Log(monsterLevel);
            GameManager.Instance.KillCount += Mathf.CeilToInt(monsterLevel / 1.5f);
            //GameManager.Instance.EnemyMeleeDmg += (Mathf.FloorToInt(GameManager.Instance.KillCount / 250f));
            GameManager.Instance.NumKills++;
            //Rain coins
            for (int i = 0; i < (skyMoneyDropNum + Mathf.FloorToInt(monsterLevel / 1.5f)); i++)
            {
                //GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(UnityEngine.Random.Range(-43, 43), 18), Quaternion.identity);
                GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(Camera.main.gameObject.transform.position.x + UnityEngine.Random.Range(-16.5f, 16.5f), 18), Quaternion.identity);
                Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            //Unlock
            fellOff = !fellOff;
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }
    

//Test code for future usage for finding closest x
    //GameObject FindClosestEdge()
    //{
    //    GameObject[] gos;
    //    gos = GameObject.FindGameObjectsWithTag("Edge");
    //    GameObject closest = null;
    //    float distance = Mathf.Infinity;
    //    Vector3 position = transform.position;
    //    foreach (GameObject go in gos)
    //    {
    //        Vector3 diff = go.transform.position - position;
    //        float curDistance = diff.sqrMagnitude;
    //        if (curDistance < distance)
    //        {
    //            closest = go;
    //            distance = curDistance;
    //        }
    //    }
    //    return closest;
    //}


public void Move()
    {
        if (!Attack)
        {
            //Edge detection
            if ((GetDirection().x > 0 && transform.position.x < this.transform.parent.gameObject.transform.parent.gameObject.transform.Find("RightEdge").position.x) || (GetDirection().x < 0 && transform.position.x > this.transform.parent.gameObject.transform.parent.gameObject.transform.Find("LeftEdge").position.x))
            {
                MyAnimator.SetFloat("speed", 1);
                transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left; //if fR is true 1, then V2.right else -1 and left
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage(string currentDmgSrc)
    {
        if (this.tag != "EnemyDead")
        {
            if (currentDmgSrc == "prd")
            {
                currentReceiveDamage = GameManager.Instance.PlayerRangedDmg;
                source.PlayOneShot(takeRangedSound, 0.2F);
            }
            if (currentDmgSrc == "pmd")
            {
                currentReceiveDamage = GameManager.Instance.PlayerMeleeDmg;
                source.PlayOneShot(takeMeleeSound, 0.3F);
            }

            //Critical Hit %
            if (UnityEngine.Random.Range(1, 13) == 2)
            {
                currentReceiveDamage = Mathf.FloorToInt(currentReceiveDamage * 1.5f);
                gotCrit = true;
            }

            healthStat.CurrentVal -= currentReceiveDamage;

            if (Target == null)
            {
                ChangeDirection();
            }

            if (!healthCanvas.isActiveAndEnabled)
            {
                healthCanvas.enabled = true;
            }

            //Quick way for critical hit text, FUTURE: re-order to truncate code
            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                if (!gotCrit)
                {
                    CombatTextManager.Instance.CreateText(transform.position, Convert.ToString(currentReceiveDamage), Color.white, false, 0f, true);
                }
                else
                {
                    CombatTextManager.Instance.CreateText(transform.position, Convert.ToString(currentReceiveDamage), Color.red, true, 0f, true);
                }
            }
            else
            {
                MyAnimator.SetTrigger("die");
                Debug.Log(monsterLevel);
                GameManager.Instance.KillCount += (monsterLevel * 2 - 1);
                GameManager.Instance.NumKills++;
                //GameManager.Instance.EnemyMeleeDmg += (Mathf.FloorToInt(GameManager.Instance.KillCount / 250f));
                if (this.tag != "EnemyDead")
                {
                    if (!gotCrit)
                    {
                        CombatTextManager.Instance.CreateText(transform.position, Convert.ToString(currentReceiveDamage), Color.white, false, 0f, true);
                    }
                    else
                    {
                        CombatTextManager.Instance.CreateText(transform.position, Convert.ToString(currentReceiveDamage), Color.red, true, 0f, true);
                    }
                    //Randomized position drop for higher level = higher coin count Mathf.CeilToInt(monsterLevel/2)
                    for (int i = 0; i < Mathf.CeilToInt(monsterLevel / 1.8f); i++)
                    {
                        GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(0.0f, 1.5f), transform.position.y + UnityEngine.Random.Range(1.0f, 3.8f)), Quaternion.identity);
                        Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    }
                    if (UnityEngine.Random.Range(1, 6) == 3)
                    {
                        GameObject burger = (GameObject)Instantiate(GameManager.Instance.BurgerPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(0.0f, 1.5f), transform.position.y + UnityEngine.Random.Range(1.0f, 3.8f)), Quaternion.identity);
                        Physics2D.IgnoreCollision(burger.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    }
                    this.tag = "EnemyDead";
                }
                yield return null;
            }
            gotCrit = false;
        }
    }
    
    public override void Death()
    {
        fellOff = false;
        rained = false;
        time = 0;
        this.tag = "Enemy";
        MyAnimator.SetTrigger("idle");
        MyAnimator.ResetTrigger("die");
        healthStat.MaxVal = GameManager.Instance.EnemyHealth;
        healthStat.CurrentVal = healthStat.MaxVal;
        monsterLevel = GameManager.Instance.EnemyLevel;
        //for when not death
        //healthStat.CurrentVal += 10;
        healthCanvas.enabled = false;
        Target = null;

        transform.position = new Vector3(startPos.x + UnityEngine.Random.Range(2.5f, 6.5f), startPos.y, startPos.z);
    }

    //ChangeDirection in Enemy
    public override void ChangeDirection()
    {
        //Makes a reference to the enemys canvas
        Transform tmp = transform.Find("EnemyHPCanvas").transform;

        //Stores the position, so that we know where to move it after we have flipped the enemy
        Vector3 pos = tmp.position;

        ///Removes the canvas from the enemy, so that the health bar doesn't flip with it
        tmp.SetParent(null);

        base.ChangeDirection();

        //Puts the health bar back on the enemy.
        tmp.SetParent(transform);

        //Pits the health bar back in the correct position.
        tmp.position = pos;
    }


    public void PlaySwingAtk()
    {
        source.PlayOneShot(swingSound, 0.68F);
    }
    public void PlayThrowAtk()
    {
        source.PlayOneShot(swingSound, 0.28F);
    }
}
