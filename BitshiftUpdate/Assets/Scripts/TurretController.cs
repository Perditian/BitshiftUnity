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
	private LineRenderer line;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<Renderer> ();
		anim = GetComponent<Animator> ();
		line = GetComponent<LineRenderer> ();
		line.sortingLayerName = "Foreground";
		tracking = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 CastEnd = target.transform.position + new Vector3(.185f, -.325f, 0f);

		// if turret is visible and player in front of turret
		if (sprite.isVisible && CastEnd.x * transform.localScale.x > transform.position.x * transform.localScale.x) {
			if (GameManager.Instance.bitshifted) {
				shifted_fire(CastEnd);
			} else {
				normal_fire(CastEnd);
			}
		} else {
			tracking = false;
			line.enabled = false;
		}	
	}

	void normal_fire(Vector3 CastEnd) {
		// calculate position to begin raycast from outside of hitbox
		Vector3 Offset = CastEnd - transform.position; 
		Vector3 CastStart = transform.position + Offset.normalized * (.4f * Mathf.Sqrt(2));

		angle = 0;

		// check for hit
		RaycastHit2D hit = Physics2D.Raycast(CastStart, (CastEnd- CastStart), Mathf.Infinity, mask);

		// Firing Logic
		// if hit a player rather than an obstacle
		if (hit.collider.gameObject.tag == "Player") {
			// calculate angle of player for animation
			angle = Mathf.Atan((transform.position - CastStart).y/(transform.position - CastStart).x);

			// if already tracking player
			if (tracking) {
				// FIRE ZE BULLET
				if ((Time.time - time) >= normal_delay) {
					Debug.Log("bang");
					GameObject b = (GameObject)Instantiate(bullet, transform.position + Offset.normalized * (.45f * Mathf.Sqrt(2)), Quaternion.identity);
					b.GetComponent<BulletController>().target = target;
					tracking = false;
				// DRAW ZE LINE
				} else {
					Vector3[] array = new Vector3[2];
					array[0] = transform.position + new Vector3(0f,0f,-.5f);
					array[1] = transform.position + (CastEnd - transform.position) * (Time.time - time) / normal_delay + new Vector3(0f,0f,-.5f);
					line.SetPositions(array);
					line.enabled = true;
				}
			// begin tracking cycle
			} else {
				tracking = true;
				time = Time.time;
			}
		} else {
			tracking = false;
		}	
	}

	void shifted_fire(Vector3 CastEnd) {
		// calculate position to begin raycast from outside of hitbox
		Vector3 Offset = CastEnd - transform.position; 

		// set angle to return to to 0
		angle = 0;

		// disable line drawing
		line.enabled = false;

		// Firing Logic
		// fire regardless of obstacles. If already tracking:
		if (tracking) {
			// FIRE ZE BULLET (never draw ze line)
			if ((Time.time - time) >= shifted_delay) {
				Debug.Log("bang");
				GameObject b = (GameObject)Instantiate(bullet, transform.position + Offset.normalized * (.45f * Mathf.Sqrt(2)), Quaternion.identity);
				b.GetComponent<BulletController>().target = target;
				tracking = false;
			}
		// begin tracking cycle
		} else {
			tracking = true;
			time = Time.time;
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
		Debug.Log(angle / Mathf.PI * 180);
	}
}