﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBadGuyEmitter : MonoBehaviour {
	private RoomManager roomManagerScript;

	void Start () {
		roomManagerScript = gameObject.GetComponentInParent<RoomManager> ();
	}

	void Update () {
		if (roomManagerScript.shouldEmit && !roomManagerScript.isGameOver) {
			roomManagerScript.shouldEmit = false;

			GameObject resource = (GameObject)Instantiate(
				Resources.Load("BadGuy"), 
				transform.position,
				Quaternion.identity
			);

			resource.transform.SetParent(this.transform);
			Destroy(resource, 5.5f);

			StartCoroutine(waitRandom(1, 4));
		}
	}

	private IEnumerator waitRandom(int min, int max) {
		float randNum = Random.value  * (max - min) + min;
		yield return new WaitForSeconds(randNum);
		roomManagerScript.shouldEmit = true;
	}
}
