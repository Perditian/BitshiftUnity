using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	public AudioSource normal;
	public AudioSource shifted;

	// Use this for initialization
	void Start () {
		normal.mute = false;
		shifted.mute = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.bitshifted) {
			normal.mute = true;
			shifted.mute = false;			
		} else {
			normal.mute = false;
			shifted.mute = true;
		}
	}
}
