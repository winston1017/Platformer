using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    [SerializeField]
    protected Stat healthStat;

    [SerializeField]
    protected Transform knifePos;

    [SerializeField]
    protected float movementSpeed;
    
    [SerializeField]
    protected GameObject knifePrefab;

    [SerializeField]
    private EdgeCollider2D swordCollider;

    [SerializeField]
    private List<string> damageSources;

    //TEST
    //Initializing damage values
    protected string playerRangedDmgStr = "prd";
    protected string playerMeleeDmgStr = "pmd";
    protected string enemyRangedDmgStr = "erd";
    protected string enemyMeleeDmgStr = "emd";


    protected bool facingRight;

    public abstract bool IsDead { get; }    

    public bool Attack { get; set;}

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    // Use this for initialization
    public virtual void Start()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
        healthStat.Initialize();
}

    // Update is called once per frame
    void Update()
    {

    }

    public abstract IEnumerator TakeDamage(string currentDmgSrc);

    public abstract void Death();

    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowKnife(int value)
    {
        //NOTE: currently gameobj rotates by this command
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(0, 0, -90));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(0, 0, 90));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains (other.tag))
        {
            if (other.tag == "Knife")
            { 
                StartCoroutine(TakeDamage(playerRangedDmgStr));
            }
            if (other.tag == "Sword")
            {
                StartCoroutine(TakeDamage(playerMeleeDmgStr));
            }
            if (other.tag == "EnemyMelee")
            {
                StartCoroutine(TakeDamage(enemyMeleeDmgStr));
            }
            if (other.tag == "EnemyRanged")
            {
                StartCoroutine(TakeDamage(enemyRangedDmgStr));
            }
        }
    }
}
