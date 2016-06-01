using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	[HideInInspector] public bool jump = false;

	public float maxSpeed = 5f;
	public float moveForce = 365f;
	public float speedInc = 0.5f;
	public float jumpForce = 275f;
	public Transform groundCheck;

	private bool grounded;
	private float prevH = 0;
	private Rigidbody2D rb2d; 
	private Animator anim;

	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		grounded = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		Debug.Log (grounded);

		if (Input.GetButtonDown ("Jump") && grounded) {
			jump = true;
		}
	}

	void FixedUpdate() 
	{
		
		float h = Input.GetAxis ("Horizontal");

		animateWalking (h);

		if (h * rb2d.velocity.x < maxSpeed) {
			rb2d.velocity = new Vector2(rb2d.velocity.x + h*speedInc,rb2d.velocity.y);		}

		if (h == 0) {
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
		}

		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed) {
			rb2d.velocity =  new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		}

		// animation things

		if (jump) {
			// animation things go here
			rb2d.AddForce (new Vector2 (0f, jumpForce));
			jump = false;
		}
	}

	void animateWalking(float h)
	{
		if (h == 0) {
			anim.SetBool ("walkRight", false);
			anim.SetBool ("walkLeft", false);
			return;
		}
		if (Mathf.Sign (prevH) != Mathf.Sign (h)) {
			anim.SetTrigger ("turn");
			prevH = h;
			return;
		} else if (h > 0) {
			anim.SetBool ("walkleft", false);
			anim.SetBool ("walkRight", true);
			prevH = h;
			return;
		} else if (h < 0) {
			anim.SetBool ("walkRight", false);
			anim.SetBool ("walkLeft", true);
			prevH = h;
		}
	}

}
