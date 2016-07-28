using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public Collider2D target; 
	private Rigidbody2D rb2d; 
	private Renderer sprite;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		sprite = GetComponent<Renderer> ();
		Vector2 direction = target.transform.position + new Vector3(.185f, -.325f) - transform.position;
		rb2d.velocity = direction.normalized * 10;
	}
	
	void OnTriggerEnter2D(Collider2D hit) {
		if (hit.gameObject.tag == "Player") {
			Debug.Log("YOU DEAD");
		}
	}

	// Update is called once per frame
	void Update () {
		if (!sprite.isVisible) {
			Destroy(this);
		}
	}
}
