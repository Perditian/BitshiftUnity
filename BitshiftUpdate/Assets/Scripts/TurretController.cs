﻿using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {


	public Collider2D target;
	public LayerMask mask;
	public GameObject bullet; 

	private Renderer sprite;
	public float time = 0f;
	public bool tracking = false;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<Renderer> ();
		tracking = false;
	}

	// Update is called once per frame
	void Update () {
		if (sprite.isVisible) {
			Debug.Log("hi! You gonna die now 2");

			// calculate position to begin raycast from outside of hitbox
			Vector3 CastEnd = target.transform.position + new Vector3(.185f, -.325f, 0f);
			Vector3 Offset = CastEnd - transform.position; 
			Vector3 CastStart = transform.position + Offset.normalized * (.4f * Mathf.Sqrt(2));

			// check for collisions
			if (CastEnd.x * transform.localScale.x > CastStart.x * transform.localScale.x) {
				RaycastHit2D hit = Physics2D.Raycast(CastStart, (CastEnd- CastStart), Mathf.Infinity, mask);
				Debug.DrawLine(CastStart, hit.point, Color.red, .1f);

				// firing logic
				if (hit.collider.gameObject.tag == "Player") {
					if (tracking) {
						if ((Time.time - time) >= 2) {
							Debug.Log("bang");
							GameObject b = (GameObject)Instantiate(bullet, CastStart, Quaternion.identity);
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
}