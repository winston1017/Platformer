using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected Transform knifePos;

    [SerializeField]
    protected float movementSpeed;
    
    [SerializeField]
    protected GameObject knifePrefab;

    [SerializeField]
    protected int health;

    [SerializeField]
    private EdgeCollider2D SwordCollider;

    [SerializeField]
    private List<string> damageSources;

    protected bool facingRight;

    public abstract bool IsDead { get; }    

    public bool Attack { get; set;}

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    // Use this for initialization
    public virtual void Start()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract IEnumerator TakeDamage();

    public void ChangeDirection()
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
        SwordCollider.enabled = !SwordCollider.enabled;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains (other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
