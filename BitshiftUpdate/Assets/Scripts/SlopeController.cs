using UnityEngine;
using System.Collections;

public class SlopeController : MonoBehaviour {

	private Animator anim;

	public Collider2D norm_c;
	public Collider2D shift_c;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate () {
		if (GameManager.Instance.bitshifted) {
			norm_c.enabled = false;
			shift_c.enabled = true;
		} else {
			norm_c.enabled = true;
			shift_c.enabled = false;
		}
	}	
	
	// Update is called once per frame
	void Update () {
		animateSlopes(GameManager.Instance.bitshifted);
	}

	void animateSlopes(bool shifted)
	{
		if (shifted)
			anim.SetBool ("shifted", true);
		else
			anim.SetBool ("shifted", false);
	}
}
