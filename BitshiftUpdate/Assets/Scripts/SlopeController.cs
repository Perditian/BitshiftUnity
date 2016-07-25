using UnityEngine;
using System.Collections;

public class SlopeController : MonoBehaviour {

	private Animator anim;
	public Collider norm;
	public Collider shift;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		
	}

	void FixedUpdate () {
		// if (GameManager.Instance.bitshifted) {
		// 	norm.enabled = false;
		// 	shift.enabled = true;
		// } else {
		// 	norm.enabled = true;
		// 	shift.enabled = false;
		// }
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
