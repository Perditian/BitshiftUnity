using UnityEngine;
using System.Collections;

public class BoulderController : MonoBehaviour {

	public Collider2D norm;
	public Collider2D shift;

	private Rigidbody2D rb2d; 
	private Vector2 direction = Vector2.zero;
	private float   onSlope = 0;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		direction = Vector2.zero;
		onSlope = 0;
	}
	
	void OnCollisionEnter2D(Collision2D hit)
	{
		Debug.Log("colliding");

	    // If the object we hit is the enemy
	    if (hit.gameObject.tag == "Player")
	    {
			Debug.Log("PUSH!");
	        // Calculate Angle Between the collision point and the player
	        direction = hit.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
	        // We then get the opposite (-Vector3) and normalize it
	        direction = -direction.normalized;
	    } else if (hit.gameObject.tag == "Slope") {
			onSlope -= Mathf.Sign(hit.transform.localScale.x);
		}
	}

	void OnCollisionExit2D (Collision2D hit)
	{
		if (hit.gameObject.tag == "Player") {
			direction = Vector2.zero;
		} else if (hit.gameObject.tag == "Slope") {
			onSlope += Mathf.Sign(hit.transform.localScale.x);
		}  
	}

	// Update is called once per frame
	void FixedUpdate () {
		// applypushing force to aid in pushing up slopes
		if (onSlope != 0 && Mathf.Sign(onSlope) != Mathf.Sign(direction.x) && direction != Vector2.zero) {
			rb2d.AddForce(new Vector2(Mathf.Sign(direction.x) * 24, 24));
			Debug.Log(direction);		
		}

		if (GameManager.Instance.bitshifted) {
			rb2d.isKinematic = true;
			transform.rotation = Quaternion.identity;
			norm.enabled = false;
			shift.enabled = true;
		} else {
			rb2d.isKinematic = false;
			norm.enabled = true;
			shift.enabled = false;
		}
	}
}
