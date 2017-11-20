using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;
    
    private Canvas healthCanvas;

    private bool coinDrop;
    private int monsterLevel = 1;
    private int currentReceiveDamage = 10;
    private int skyMoneyDropNum = 8;

    public GameObject Target { get; set; }

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
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
                return (Vector2.Distance(transform.position, Target.transform.position) <= chaseRangeHi) && (Vector2.Distance(transform.position, Target.transform.position) >= chaseRangeLo);
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
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
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
                ChangeDirection();
            }
        }

    }
    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                if (transform.position.y <= -14f)
                {
                    for (int i = 0; i < (skyMoneyDropNum + monsterLevel); i++)
                    {
                        GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(UnityEngine.Random.Range(-43,43), 18), Quaternion.identity);
                        Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    }
                    Death();
                }
                currentState.Execute();
            }
            LookAtTarget();
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

    public void Move()
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
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
        return facingRight ? Vector2.right : Vector2.left; //if fR is true, then V2.right
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage(string currentDmgSrc)
    {

        if (currentDmgSrc == "prd")
        {
            currentReceiveDamage= GameManager.Instance.PlayerRangedDmg;
        }
        if (currentDmgSrc == "pmd")
        {
            currentReceiveDamage = GameManager.Instance.PlayerMeleeDmg;
        }

        healthStat.CurrentVal -= currentReceiveDamage;

        if (!healthCanvas.isActiveAndEnabled)
        {
            healthCanvas.enabled = true;
        }

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("die");
            GameManager.Instance.KillCount += monsterLevel; 
            if (!coinDrop)
            {
                //Randomized position drop for higher level = higher coin count Mathf.CeilToInt(monsterLevel/2)
                for (int i = 0; i < monsterLevel; i++)
                {
                    GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x+ UnityEngine.Random.Range(0.0f, 1.5f), transform.position.y + UnityEngine.Random.Range(1.0f, 3.8f)), Quaternion.identity);
                    Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
                if (UnityEngine.Random.Range(1, 5) == 3)
                {
                    GameObject burger = (GameObject)Instantiate(GameManager.Instance.BurgerPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(0.0f, 1.5f), transform.position.y + UnityEngine.Random.Range(1.0f, 3.8f)), Quaternion.identity);
                    Physics2D.IgnoreCollision(burger.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
            }
            coinDrop = true;
            yield return null;
        }
    }
    
    public override void Death()
    {
        //Fix for dead body dropping coins
        coinDrop = false;
        MyAnimator.SetTrigger("idle");
        MyAnimator.ResetTrigger("die");
        healthStat.MaxVal += 10;
        healthStat.CurrentVal = healthStat.MaxVal;
        monsterLevel++;
        
        transform.position = startPos;
        healthCanvas.enabled = false;
        Target = null;
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

}
