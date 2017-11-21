using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;

public delegate void DeadEventHandler();

public class Player : Character
{

    private static Player instance;
    public event DeadEventHandler Dead;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float immortalTime;

    private float direction;
    private bool move;
    private float currentReceiveDamage;

    //private float btnHorizontal;

    private Vector2 startPos;

    public AudioClip swingSound;
    public AudioClip throwSound;
    public AudioClip takeDmgSound;
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip eatSound;
    private AudioSource source;

    public Rigidbody2D MyRigidbody { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if (healthStat.CurrentVal <= 0)
            {
                OnDead();
            }
            return healthStat.CurrentVal <= 0;
        }
    }

    //Improved Jump
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 1.55f;
    public bool btnJumping;
    public float time;
    public bool announceDrown;

    [Range(1, 10)]
    public float jumpVelocity;

    public override void Start()
    {
        time = 0;
        announceDrown = false;
        base.Start();
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidbody = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    //void Update()
    //{
    //    //if (!TakingDamage && !IsDead)
    //    if (!IsDead)
    //    {
    //        if (transform.position.y <= -14f)
    //        {
    //            Death();
    //        }
    //        HandleInput();
    //    }
    //}

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (!IsDead)
    //    {
    //        if (this.GetComponent<Animator>().GetBool("land") == true && MyRigidbody.velocity.y < 0)
    //        {
    //            this.GetComponent<Animator>().SetBool("land", false);
    //        }
    //        OnGround = IsGrounded();
    //        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
    //        HandleMovement(horizontal);
    //        Flip(horizontal);
    //        HandleLayers();
    //    }
    //}

    void Update()
    {
        //if (!TakingDamage && !IsDead)
        if (!IsDead)
        {
            if (transform.position.y <= -14f)
            {
                if (announceDrown == false)
                {
                    CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "You Fell :(" +"\n Respawning...", Color.cyan, true);
                    announceDrown = true;
                }
                time += Time.deltaTime;
                if (time >= 5)
                {
                    Death();
                }
            }
            if (this.GetComponent<Animator>().GetBool("land") == true && MyRigidbody.velocity.y < 0)
            {
                this.GetComponent<Animator>().SetBool("land", false);
                //Debug.Log("land false");

                if (OnGround)
                {
                    //Debug.Log("-> on ground so set velocity y to 0");
                    MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, 0);
                }
            }
            OnGround = IsGrounded();
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            HandleMovement(horizontal);
            Flip(horizontal);
            HandleLayers();
            HandleInput();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    //new HandleMovement
    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        if (!Attack && !Slide && (OnGround || airControl)) // add one for aircontrool and in air and slow it down
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        if (Attack && !Slide && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed / 1.8f, MyRigidbody.velocity.y);
        }
        if (Jump && OnGround && (MyRigidbody.velocity.y == 0)) // || MyRigidbody.velocity.y > -8.8
        {
            MyRigidbody.velocity = Vector2.up * jumpVelocity;
            source.PlayOneShot(jumpSound, 0.13F);
        }
        if (Jump && OnGround && (MyRigidbody.velocity.y < 0)) // || MyRigidbody.velocity.y > -8.8
        {
            Debug.Log("tried to jump but velocity Y is < 0");
        }
        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MyAnimator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && MyRigidbody.velocity.x != 0 && MyRigidbody.velocity.y == 0)
        {
            MyAnimator.SetTrigger("slide");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            MyAnimator.SetTrigger("throw");
        }


    }
    //Call changedirection by movement
    private void Flip(float horizontal)
    {
        if (!Slide)
        {
            if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
            {
                ChangeDirection();
            }
        }
    }

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0) // works when it is <= 0
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void ThrowKnife(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            base.ThrowKnife(value);
        }
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            //flash spriteRenderer while immortal to indicate immortality
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.15f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.15f);
        }
    }
    public override IEnumerator TakeDamage(string currentDmgSrc)
    {

        if (!immortal)
        {
            if (currentDmgSrc == "erd")
            {
                currentReceiveDamage = GameManager.Instance.EnemyRangedDmg;
                source.PlayOneShot(takeDmgSound, 0.2F);
            }
            if (currentDmgSrc == "emd")
            {
                currentReceiveDamage = GameManager.Instance.EnemyMeleeDmg;
                source.PlayOneShot(takeDmgSound, 0.2F);
            }

            healthStat.CurrentVal -= currentReceiveDamage;
            CombatTextManager.Instance.CreateText(transform.position, Convert.ToString(currentReceiveDamage), Color.white, false);

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;

                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
                CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "You Died x_x" + "\n Respawning...", Color.cyan, true);
            }
            yield return null;
        }
    }

    public override void Death()
    {
        time = 0;
        announceDrown = false;

        MyRigidbody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;

        GameManager.Instance.DeathCount++;
        GameManager.Instance.CollectedCoins -= 10;
        GameManager.Instance.KillCount -= 10;
    }

    //public void BtnJump()
    //{
    //    MyAnimator.SetTrigger("jump");
    //    Debug.Log("btnjump pressed, bool true");
    //}

    public void BtnJumpEnter()
    {
        MyAnimator.SetTrigger("jump");
        btnJumping = true;
    }

    public void BtnJumpExit()
    {
        btnJumping = false;
    }

    public void BtnAttack()
    {
        MyAnimator.SetTrigger("attack");
    }

    public void BtnSlide()
    {
        //if (MyRigidbody.velocity.x != 0 && MyRigidbody.velocity.y == 0)
        if (MyRigidbody.velocity.y == 0)
        {
            MyAnimator.SetTrigger("slide");
        }
    }

    public void BtnThrow()
    {
        MyAnimator.SetTrigger("throw");
    }

    //public void BtnMove(float direction)
    //{
    //    this.direction = direction;
    //    this.move = true;
    //}

    //public void BtnStopMove()
    //{
    //    this.direction = 0;
    //    this.btnHorizontal = 0;
    //    this.move = false;
    //}


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            GameManager.Instance.CollectedCoins++; //future: coinval
            Destroy(other.gameObject);
            CombatTextManager.Instance.CreateText(transform.position, "+1 G", Color.yellow, false);
            source.PlayOneShot(coinSound, 0.38F);
        }
        if (other.gameObject.tag == "Burger")
        {
            healthStat.CurrentVal += 20; //future: coinval
            Destroy(other.gameObject);
            CombatTextManager.Instance.CreateText(transform.position, "+20 HP", Color.green, false);
            source.PlayOneShot(eatSound, 0.2F);
        }
    }

    public void BtnUpgradePlayerMelee()
    {
        if (GameManager.Instance.CollectedCoins >= GameManager.Instance.MeleeCost)
        {
            GameManager.Instance.PlayerMeleeDmg += 5;
            GameManager.Instance.CollectedCoins -= GameManager.Instance.MeleeCost;
            GameManager.Instance.MeleeLevel++;
            GameManager.Instance.MeleeCost = Mathf.FloorToInt(GameManager.Instance.MeleeCost * 1.23f);
            CombatTextManager.Instance.CreateText(transform.position, "Melee ↑5", Color.blue, false);
        }
    }
    public void BtnUpgradePlayerRanged()
    {
        if (GameManager.Instance.CollectedCoins >= GameManager.Instance.RangedCost)
        {
            GameManager.Instance.PlayerRangedDmg += 3;
            GameManager.Instance.CollectedCoins -= GameManager.Instance.RangedCost;
            GameManager.Instance.RangedLevel++;
            GameManager.Instance.RangedCost = Mathf.FloorToInt(GameManager.Instance.RangedCost * 1.23f);
            CombatTextManager.Instance.CreateText(transform.position, "Ranged ↑3", Color.blue, false);
        }
    }

    public void PlaySwingAtk()
    {
        source.PlayOneShot(swingSound, 0.68F);
    }
    public void PlayThrowAtk()
    {
        source.PlayOneShot(throwSound, 0.38F);
    }
}

