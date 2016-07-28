using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public Collider2D target; 
	public float normal_speed;
	public float shifted_speed;
	private Rigidbody2D rb2d; 
	private Renderer sprite;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		sprite = GetComponent<Renderer> ();
		Vector2 direction = target.transform.position + new Vector3(.185f, -.325f) - transform.position;
		if (GameManager.Instance.bitshifted) {
			rb2d.velocity = direction.normalized * shifted_speed;
		} else {
			rb2d.velocity = direction.normalized * normal_speed;
		}
	}
	
	void OnTriggerEnter2D(Collider2D hit) {
		if (hit.gameObject.tag == "Player") {
			Debug.Log("YOU DEAD");
		}
		Destroy(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (!sprite.isVisible) {
			Destroy(this.gameObject);
		}
	}

	void FixedUdate () {
		if (GameManager.Instance.bitshifted) {
			rb2d.velocity = rb2d.velocity.normalized * shifted_speed;
		} else {
			rb2d.velocity = rb2d.velocity.normalized * normal_speed;
		}
	}
} 
