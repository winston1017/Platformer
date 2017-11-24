using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;

public delegate void DeadEventHandler();

public class Player : Character
{
    public LayerMask groundLayer;

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

    public Vector2 startPos;

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
    public bool wasGrounded;
    public bool cameraDisabler;

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


    void Update()
    {


        //if (!TakingDamage && !IsDead)
        if (!IsDead)
        {
            if (transform.position.y <= -14f)
            {
                if (announceDrown == false)
                {
                    cameraDisabler = true;
                    MyRigidbody.velocity = new Vector2(0, 0);
                    GameManager.Instance.DeathCount++;
                    GameManager.Instance.CollectedCoins -= 10;
                    GameManager.Instance.KillCount -= 10;

                    if (GameManager.Instance.DeathCount < 3)
                    {
                        CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "You Fell :(" + "\n Respawning...", Color.cyan, true, 5f, true);
                    }
                    announceDrown = true;
                }
                time += Time.deltaTime;
                if (time >= 5)
                {
                    Death();
                }
            }

            if (this.GetComponent<Animator>().GetBool("land") == true && MyRigidbody.velocity.y != 0)
            {
                this.GetComponent<Animator>().SetBool("land", false);
            }

            if (!cameraDisabler)
            {
                OnGround = GroundCheck();
                
                if (btnJumping && GroundCheck() && Jump && btnJumping == true && MyRigidbody.velocity.y == 0)
                {
                    MyRigidbody.velocity = Vector2.up * jumpVelocity;
                }
                float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
                //float horizontal = Input.GetAxis("Horizontal");
                HandleMovement(horizontal);
                Flip(horizontal);
                HandleLayers();
                HandleInput();
                //wasGrounded = IsGrounded();
            }
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
    private void HandleJump()
    {
        
        if (Jump && btnJumping == true) // || MyRigidbody.velocity.y > -8.8
        {
            MyRigidbody.velocity = Vector2.up * jumpVelocity;
            //source.PlayOneShot(jumpSound, 0.13F);
        }
    }
    //new HandleMovement
    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        // causes unable to jump when tapping fast //if (Jump && OnGround && (MyRigidbody.velocity.y == 0)) // || MyRigidbody.velocity.y > -8.8
        //if (Jump && GroundCheck() && btnJumping == true) // || MyRigidbody.velocity.y > -8.8
        //{
        //    MyRigidbody.velocity = Vector2.up * jumpVelocity;
        //    //source.PlayOneShot(jumpSound, 0.13F);
        //}
        if (!Attack && !Slide && (GroundCheck() || airControl)) // add one for aircontrool and in air and slow it down
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        if (Attack && !Slide && (GroundCheck() || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed / 1.8f, MyRigidbody.velocity.y);
        }
        //if (Jump && GroundCheck() && (MyRigidbody.velocity.y < 0)) // || MyRigidbody.velocity.y > -8.8
        //{
        //    Debug.Log("tried to jump but velocity Y is < 0 = " + MyRigidbody.velocity.y);
        //}
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

    //private bool IsGrounded()
    //{
    //    if (MyRigidbody.velocity.y <= 0) // works when it is <= 0
    //    {
    //        foreach (Transform point in groundPoints)
    //        {
    //            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
    //            for (int i = 0; i < colliders.Length; i++)
    //            {
    //                if (colliders[i].gameObject != gameObject)
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    private bool GroundCheck()
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

    //private bool GroundCheck()
    //{

    //    Vector2 position = transform.position;
    //    Vector2 position2 = new Vector2(transform.position.x + 0.5f, transform.position.y);
    //    Vector2 position3 = new Vector2(transform.position.x - 0.5f, transform.position.y);
    //    Vector2 direction = Vector2.down;
    //    float distance = 3f;
    //    Debug.DrawRay(position, direction, Color.green);
    //    Debug.DrawRay(position2, direction, Color.green);
    //    Debug.DrawRay(position3, direction, Color.green);
    //    RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
    //    RaycastHit2D hit2 = Physics2D.Raycast(position, direction, distance, groundLayer);
    //    RaycastHit2D hit3 = Physics2D.Raycast(position, direction, distance, groundLayer);


    //    if (hit.collider != null || hit2.collider != null || hit3.collider != null)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

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
            else if (currentDmgSrc == "emd")
            {
                currentReceiveDamage = GameManager.Instance.EnemyMeleeDmg;
                source.PlayOneShot(takeDmgSound, 0.2F);
            }

            healthStat.CurrentVal -= currentReceiveDamage;
            CombatTextManager.Instance.CreateText(transform.position, Convert.ToString(currentReceiveDamage), Color.white, false, 0f, true);

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
                CombatTextManager.Instance.CreateAnnounceText(new Vector3(Camera.main.gameObject.transform.position.x, Camera.main.gameObject.transform.position.y + 1f, 0), "You Died x_x" + "\n Respawning...", Color.cyan, true, 5f, true);

                GameManager.Instance.DeathCount++;
                GameManager.Instance.CollectedCoins -= 10;
                GameManager.Instance.KillCount -= 10;
            }
            yield return null;
        }
    }

    public override void Death()
    {
        time = 0;
        announceDrown = false;
        cameraDisabler = false;
        MyRigidbody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
    }

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
        if (MyRigidbody.velocity.y == 0)
        {
            MyAnimator.SetTrigger("slide");
        }
    }

    public void BtnThrow()
    {
        MyAnimator.SetTrigger("throw");
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            GameManager.Instance.CollectedCoins++; //future: coinval
            Destroy(other.gameObject);
            CombatTextManager.Instance.CreateText(transform.position, "+1 G", Color.yellow, false, 0f, true);
            source.PlayOneShot(coinSound, 0.38F);
        }
        if (other.gameObject.tag == "Burger")
        {
            healthStat.CurrentVal += 20; //future: coinval
            Destroy(other.gameObject);
            CombatTextManager.Instance.CreateText(transform.position, "+20 HP", Color.green, false, 0f, true);
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
            CombatTextManager.Instance.CreateText(transform.position, "Melee ↑5", Color.blue, false, 0f, true);
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
            CombatTextManager.Instance.CreateText(transform.position, "Ranged ↑3", Color.blue, false, 0f, true);
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

