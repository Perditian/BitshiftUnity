using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {


	public Collider2D target;
	public LayerMask mask;
	public GameObject bullet; 

	private Renderer sprite;
	public float time = 0f;
	public bool tracking = false;

	private float shifted_delay = 1;
	private float normal_delay  = 2;

	private float angle;
	private Animator anim;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<Renderer> ();
		anim = GetComponent<Animator> ();
		tracking = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (sprite.isVisible) {
			Debug.Log("hi! You gonna die now 2");

			// calculate position to begin raycast from outside of hitbox
			Vector3 CastEnd = target.transform.position + new Vector3(.185f, -.325f, 0f);
			Vector3 Offset = CastEnd - transform.position; 
			Vector3 CastStart = transform.position + Offset.normalized * (.4f * Mathf.Sqrt(2));

			angle = 0;

			// check for collisions
			if (CastEnd.x * transform.localScale.x > CastStart.x * transform.localScale.x) {
				RaycastHit2D hit = Physics2D.Raycast(CastStart, (CastEnd- CastStart), Mathf.Infinity, mask);
				Debug.DrawLine(CastStart, hit.point, Color.red, .1f);

				angle = Mathf.Tan((CastEnd- CastStart).y/(CastEnd- CastStart).x);

				// firing logic
				if (hit.collider.gameObject.tag == "Player") {
					if (tracking) {
						float delay = normal_delay;
						if (GameManager.Instance.bitshifted) {
							delay = shifted_delay;
						}
						if ((Time.time - time) >= delay) {
							Debug.Log("bang");
							GameObject b = (GameObject)Instantiate(bullet, transform.position + Offset.normalized * (.45f * Mathf.Sqrt(2)), Quaternion.identity);
							b.GetComponent<BulletController>().target = target;
							tracking = false;
						}
					} else {
						tracking = true;
						time = Time.time;
					}
				}
			} else {
				tracking = false;
			}
		} else {
			tracking = false;
		}	
	}

	void Update () {
		animateObject(GameManager.Instance.bitshifted, angle);
	}

	void animateObject(bool shifted, float angle)
	{
		if (shifted)
			anim.SetBool ("shifted", true);
		else
			anim.SetBool ("shifted", false);

		anim.SetFloat ("Angle", angle / Mathf.PI * 180);
		// Debug.Log(angle / Mathf.PI * 180);
	}
}