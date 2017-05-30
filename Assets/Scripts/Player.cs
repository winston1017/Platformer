using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private static Player instance;

	public static Player Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<Player> ();
			}
			return instance;
		}
	}

	private Animator myAnimator;


	[SerializeField]
	private float movementSpeed;
	//private bool attack;
	//private bool slide;
	private bool facingRight;

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;

	[SerializeField]
	private LayerMask whatIsGround;

	//private bool isGrounded;
	//private bool jump;
	//private bool jumpAttack;

	[SerializeField]
	private bool airControl;

	[SerializeField]
	private float jumpForce;

	public Rigidbody2D MyRigidbody{
		get;
		set;
	}

	//Replaced old vars with props
	public bool Attack {
		get;
		set;
	}
	public bool Slide {
		get;
		set;
	}
	public bool Jump {
		get;
		set;
	}
	public bool OnGround {
		get;
		set;
	}
	// Use this for initialization
	void Start () {
		facingRight = true;
		MyRigidbody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> (); 

	}

	void Update(){
		HandleInput ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		float horizontal = Input.GetAxis ("Horizontal");

		OnGround = IsGrounded();

		HandleMovement(horizontal);

		flip (horizontal);

		//HandleAttacks ();

		HandleLayers ();

		//ResetValues ();
	}

	/*private void HandleMovement(float horizontal)
	{
		if (myRigidbody.velocity.y < 0) {
			myAnimator.SetBool ("land", true);
		}
		if (!myAnimator.GetBool("slide") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || airControl))
		{
			myRigidbody.velocity = new Vector2 (horizontal*movementSpeed, myRigidbody.velocity.y); //x=-1, y=0;
		}
		if (isGrounded && jump) {
			isGrounded = false;
			myRigidbody.AddForce (new Vector2 (0, jumpForce));
			myAnimator.SetTrigger ("jump");
		}
		if (slide && !this.myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Slide")) {
			myAnimator.SetBool ("slide", true);
		} else if (!this.myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Slide")) {
			myAnimator.SetBool ("slide", false);
		}
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
	}
	private void HandleAttacks()
	{
		if (attack && isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
			myAnimator.SetTrigger ("attack");
			myRigidbody.velocity = Vector2.zero;
		}

		if (jumpAttack && !isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack")) {
			myAnimator.SetBool ("jumpAttack", true);
		}
		if (!jumpAttack && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttack"))
		{
			myAnimator.SetBool ("jumpAttack", false);
		}
	}*/

	//new HandleMovement
	private void HandleMovement (float horizontal)
	{
		if (MyRigidbody.velocity.y < 0) {
			myAnimator.SetBool ("land", true);
		}
		if (!Attack && !Slide && (OnGround || airControl)) {
			MyRigidbody.velocity = new Vector2 (horizontal * movementSpeed, MyRigidbody.velocity.y);
		}
		if (Jump && MyRigidbody.velocity.y == 0) {
			MyRigidbody.AddForce(new Vector2(0, jumpForce));
		}

		myAnimator.SetFloat ("speed", Mathf.Abs (horizontal));
	}
	private void HandleInput()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			//jump = true;
			myAnimator.SetTrigger("jump");
		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) 
		{
			//attack = true;
			//jumpAttack = true;
			myAnimator.SetTrigger("attack");
		}
		if (Input.GetKeyDown (KeyCode.LeftControl)) 
		{
			//slide = true;
			myAnimator.SetTrigger("slide");
		}
		if (Input.GetKeyDown (KeyCode.F)) 
		{
			myAnimator.SetTrigger("throw");
		}

	}

	private void flip(float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;

			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	/*private void ResetValues()
	{
		attack = false;
		slide = false;
		jump = false;
		jumpAttack = false;
	}*/

	private bool IsGrounded()
	{
		if (MyRigidbody.velocity.y <= 0) {
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);

				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject)
					{
						//myAnimator.ResetTrigger ("jump");
						//myAnimator.SetBool ("land", false);
						return true;
					}
				}
			}
		}
		return false;
	}

	private void HandleLayers()
	{
		if (!OnGround) {
			myAnimator.SetLayerWeight (1, 1);
		}
		else{
			myAnimator.SetLayerWeight (1, 0);
		}
	}
}
