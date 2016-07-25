using UnityEngine;
using System.Collections;

public class TempCamera : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > -17.6)
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);	}
}
