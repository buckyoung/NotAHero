using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBadGuy : MonoBehaviour {
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
}
