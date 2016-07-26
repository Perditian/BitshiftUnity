using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

	public static bool shifted = false;

	// [HideInInspector] 
	private bool attemptingJump = false;

	public float maxSpeed = 3f;
	public float speedInc = 0.5f;
	public float jumpForce = 120f;
	public float jumpRatio = 0.65f; //used for holding down to jump higher
	public Transform groundCheckL;
	public Transform groundCheckR;

	private bool grounded = false;
	private bool pounded = false;
	public float  onSlope = 0;
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
		onSlope = 0;
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
			attemptingJump = false;
			if (!oldground) {
				rb2d.velocity = new Vector2 (rb2d.velocity.x, 0);
			}
			prevJumpForce = jumpForce;
		}
	}
		
	void FixedUpdate() 
	{

		//find out which direction the player is attempting to move
		float horizontalInput = Input.GetAxisRaw ("Horizontal");
		Debug.Log(horizontalInput + " " + Mathf.Sign(onSlope));

		animateWalking (horizontalInput);

		// accelerate if on a slope.
		// TO DO: Change to utilize local gravity.
		if (onSlope != 0 && horizontalInput == Mathf.Sign(onSlope) && !GameManager.Instance.bitshifted) {
			Debug.Log("slide!");
			rb2d.AddForce(new Vector2 (9.81f / 2 * horizontalInput, -9.81f / Mathf.Sqrt(2) ));
		}

		// V key for fun and profit
		if (Input.GetKeyDown(KeyCode.V)) {
			Debug.Log("CHEATING :D");
			rb2d.velocity = new Vector2(rb2d.velocity.x + 6, rb2d.velocity.y);
		}

		//stop walking quickly rather than skid stop
		if (horizontalInput != Mathf.Sign(rb2d.velocity.x) && Mathf.Abs(rb2d.velocity.x) <= maxSpeed) {
			if (rb2d.velocity.x != 0) {
				rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
			}
		}

		// force based movement system if input detected
		if (horizontalInput != 0) {
			// increase speed to maximum
			if (Mathf.Abs(rb2d.velocity.x) <= maxSpeed) {
				rb2d.AddForce(new Vector2 (5 * Mathf.Sign(horizontalInput) * (maxSpeed - Mathf.Abs(rb2d.velocity.x)), 0f));		
			}
			// decrease speed to maximum 
			else if (Mathf.Sign(horizontalInput) != Mathf.Sign(rb2d.velocity.x)) {
				Debug.Log("stop sliding");
				rb2d.AddForce(new Vector2 (3 * Mathf.Sign(horizontalInput) * (Mathf.Abs(rb2d.velocity.x) - maxSpeed + 1), 0f));
			}
		}

		Jump ();
 
		if (Input.GetButtonDown("Shift")) {
			Debug.Log(shifted);
			GameManager.Instance.bitshifted = !GameManager.Instance.bitshifted;
		}
	}

	void OnCollisionEnter2D (Collision2D hit)
	{
		if (hit.gameObject.tag == "Slope") {
			onSlope -= Mathf.Sign(hit.transform.localScale.x);
		}
	}

	void OnCollisionExit2D (Collision2D hit)
	{
		if (hit.gameObject.tag == "Slope") {
			onSlope += Mathf.Sign(hit.transform.localScale.x);
		}
	}
		
	void Jump()
	{
		// initial jump
		if(Input.GetAxis("Vertical") > 0 && grounded && attemptingJump == false){
			Debug.Log(prevJumpForce);
			rb2d.velocity = new Vector2(rb2d.velocity.x, 6);
			prevJumpForce = prevJumpForce * jumpRatio;
			attemptingJump = true;
		}
		// // continue jump
		// else if (Input.GetAxis("Vertical") > 0 && jump) {
		// 	// jump animation things go here
		// 	rb2d.AddForce (new Vector2 (0f, prevJumpForce));
		// 	prevJumpForce = prevJumpForce * jumpRatio;
		// }
		// ground pound
		else if (Input.GetKeyDown("down") && !grounded && !pounded) {
			attemptingJump = false;
			pounded = true;
			Debug.Log("groundpounding");
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);		
			rb2d.AddForce (new Vector2 (0f, -400));
		}
		
		// decay jump
		if (rb2d.velocity.y > 0 && !grounded && Input.GetAxis("Vertical") <= 0){
			attemptingJump = false;
			Debug.Log("decaying jump");
			rb2d.AddForce (new Vector2 (0f, rb2d.velocity.y * rb2d.velocity.y * -7.5f));
		}
			
	}

	void animateWalking(float horizontalInput)
	{
		// no input
		if (horizontalInput == 0) {
			anim.SetBool ("walking", false);
			return;
		}

		// right input
		if (horizontalInput > 0) {
			anim.SetBool ("facingRight", true);
			anim.SetBool ("walking", true);
			return;
		}
		// left input
		if (horizontalInput < 0) {
			anim.SetBool ("facingRight", false);
			anim.SetBool ("walking", true);
			return;
		}
	}

}
