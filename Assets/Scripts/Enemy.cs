using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

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

    public override void Start () {
        base.Start();
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());

        healthCanvas = transform.GetComponentInChildren<Canvas>();
	}
	
    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }

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
	void Update () {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
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
            if ((GetDirection().x >0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
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

    public override IEnumerator TakeDamage()
    {
        if (!healthCanvas.isActiveAndEnabled)
        {
            healthCanvas.enabled = true;
        }

        healthStat.CurrentVal -= 10;

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            
            GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity);
            Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }

    public override void Death()
    {
        
        //Destroy(gameObject);
        MyAnimator.SetTrigger("idle");
        MyAnimator.ResetTrigger("die");
        healthStat.CurrentVal = healthStat.MaxVal;

        //specify startpos and also delay respawn
        transform.position = startPos;

        healthCanvas.enabled = false;

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

        ///Changes the enemys direction
        base.ChangeDirection();

        //Puts the health bar back on the enemy.
        tmp.SetParent(transform);

        //Pits the health bar back in the correct position.
        tmp.position = pos;
    }

}
