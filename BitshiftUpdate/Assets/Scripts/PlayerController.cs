using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	[HideInInspector] public bool jump = false;



	public float maxSpeed = 3f;
	public float speedInc = 0.5f;
	public float jumpForce = 120f;
	public float jumpRatio = 0.65f; //used for holding down to jump higher
	public Transform groundCheck;

	private bool grounded;
	private float prevJumpForce;
	private Rigidbody2D rb2d; 
	private Animator anim;

	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator> ();
		anim.SetBool ("facingRight", true);
		rb2d = GetComponent<Rigidbody2D> ();
		grounded = true;
		prevJumpForce = jumpForce;
	}
	
	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		if (grounded == true) {
			prevJumpForce = jumpForce;
		}


		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
			// need to make holding jump increase height 
		}
	}
		
	void FixedUpdate() 
	{

		//find out which direction the player is attempting to move
		float h = Input.GetAxis ("Horizontal");


		animateWalking (h);

		// increase speed to maximum
		if (h * rb2d.velocity.x < maxSpeed) {
			rb2d.velocity = new Vector2(rb2d.velocity.x + h*speedInc,rb2d.velocity.y);		}

		//stop walking quickly rather than skid stop
		if (h == 0) {
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
		}
			
		// bound speed
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed) {
			rb2d.velocity =  new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		}

		Jump ();
	}
		
	void Jump()
	{
		if(Input.GetButton("Jump") && jump == false){
			rb2d.AddForce(new Vector2(0f, prevJumpForce));
			prevJumpForce = prevJumpForce * jumpRatio;
		}
		if (jump) {
			// jump animation things go here
			rb2d.AddForce (new Vector2 (0f, prevJumpForce));
			prevJumpForce = prevJumpForce * jumpRatio;
			jump = false;
		}
	}

	void animateWalking(float h)
	{
		// no input
		if (h == 0) {
			anim.SetBool ("walking", false);
			return;
		}

		// right input
		if (h > 0) {
			anim.SetBool ("facingRight", true);
			anim.SetBool ("walking", true);
			return;
		}
		// left input
		if (h < 0) {
			anim.SetBool ("facingRight", false);
			anim.SetBool ("walking", true);
			return;
		}
	}

}
