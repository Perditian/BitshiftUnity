using UnityEngine;
using System.Collections;

public class StaticAnimator : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		animateObject(GameManager.Instance.bitshifted);
	}

	void animateObject(bool shifted)
	{
		if (shifted)
			anim.SetBool ("shifted", true);
		else
			anim.SetBool ("shifted", false);
	}
}
