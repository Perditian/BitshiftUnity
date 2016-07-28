using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public Collider2D target; 

	private float normal_speed = 20;
	private float shifted_speed = 5;
	private float created;
	private Rigidbody2D rb2d; 
	private Renderer sprite;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		sprite = GetComponent<Renderer> ();
		created = Time.time;
		Vector2 direction = target.transform.position + new Vector3(.185f, -.325f) - transform.position;
		rb2d.velocity = direction.normalized * 10;
	}
	
	void OnTriggerEnter2D(Collider2D hit) {
		if (hit.gameObject.tag == "Player") {
			Debug.Log("YOU DEAD");
		}
		Destroy(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (!sprite.isVisible && (Time.time - created >= .1)) {
			Destroy(this.gameObject);
				Debug.Log("bullet destroyed");
		}
	}

	void FixedUpdate () {
		if (GameManager.Instance.bitshifted) {
			rb2d.velocity = rb2d.velocity .normalized * shifted_speed;
		} else {
			rb2d.velocity = rb2d.velocity .normalized * normal_speed;
		}
	}
}
