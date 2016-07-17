using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static bool shifted = false;

	// [HideInInspector] 
	public bool jump = false;

	public float maxSpeed = 3f;
	public float speedInc = 0.5f;
	public float jumpForce = 120f;
	public float jumpRatio = 0.65f; //used for holding down to jump higher
	public Transform groundCheckL;
	public Transform groundCheckR;
	public Sprite WorldTracker;

	public bool grounded = false;
	private bool pounded = false;
	private float prevJumpForce;
	private Rigidbody2D rb2d; 
	private Animator anim;

	// Use this for initialization
	void Awake () 
	{
		shifted = false;

		anim = GetComponent<Animator> ();
		anim.SetBool ("facingRight", true);
		rb2d = GetComponent<Rigidbody2D> ();
		grounded = false;
		pounded = false;
		prevJumpForce = jumpForce;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// check if on ground
		bool oldground = grounded;
		grounded = Physics2D.Linecast(groundCheckL.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground"));
		if (grounded) {
			pounded = false;
			jump = false;
			if (!oldground)
				rb2d.velocity = new Vector2(rb2d.velocity.x , 0);
			prevJumpForce = jumpForce;
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
		if (h == 0 && Mathf.Abs(rb2d.velocity.x) < maxSpeed) {
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
		}
			
		// bound speed
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed) {
			rb2d.velocity =  new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		}

		Jump ();

		if (Input.GetButtonDown("Shift")) {
			Debug.Log(shifted);
			shifted = !shifted;
		}
	}

	void OnCollisionEnter2D (Collision2D hit)
	{
	}

	void OnCollisionExit2D (Collision2D hit)
	{
	}
		
	void Jump()
	{
		// initial jump
		if(Input.GetAxis("Vertical") > 0 && grounded && jump == false){
			Debug.Log(prevJumpForce);
			rb2d.AddForce(new Vector2(0f, 350));
			prevJumpForce = prevJumpForce * jumpRatio;
			jump = true;
		}
		// // continue jump
		// else if (Input.GetAxis("Vertical") > 0 && jump) {
		// 	// jump animation things go here
		// 	rb2d.AddForce (new Vector2 (0f, prevJumpForce));
		// 	prevJumpForce = prevJumpForce * jumpRatio;
		// }
		// ground pound
		else if (Input.GetKeyDown("down") && !grounded && !pounded) {
			jump = false;
			pounded = true;
			Debug.Log("groundpounding");
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);		
			rb2d.AddForce (new Vector2 (0f, -400));
		}
		
		// decay jump
		if (rb2d.velocity.y > 0 && !grounded && Input.GetAxis("Vertical") <= 0){
			jump = false;
			Debug.Log("decaying jump");
			rb2d.AddForce (new Vector2 (0f, rb2d.velocity.y * rb2d.velocity.y * -7.5f));
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
