using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    protected Animator myAnimator;

    [SerializeField]
    protected Transform knifePos;

    [SerializeField]
    protected float movementSpeed;
    
    [SerializeField]
    protected GameObject knifePrefab;

    protected bool facingRight;

    public bool Attack { get; set;}

    // Use this for initialization
    public virtual void Start()
    {
        facingRight = true;
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowKnife(int value)
    {
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
}
