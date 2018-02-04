﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuy : MonoBehaviour {
	private float speed = -0.85f;
	private RoomManager roomManagerScript;

	void Start () {
		roomManagerScript = gameObject.GetComponentInParent<RoomManager> ();
	}

	void Update () {
		if (roomManagerScript.isGameOver) { // kill self if game ends
			Destroy (gameObject);
		}
		transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.gameObject.name != "Hero") { // only concerned with hero collisions
			return;
		}

		if (!roomManagerScript.isHeroSlashing) {
			roomManagerScript.killHero();
			return;
		}

		Destroy (gameObject); // kill self if hero slashed
	}
}
