using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {

	private Animator anim;

	void Start () 
	{
		anim = GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		animateClouds (GameManager.Instance.bitshifted);
	
	}

	void animateClouds(bool shifted)
	{
		anim.SetBool("shifted", shifted);
	}
}
