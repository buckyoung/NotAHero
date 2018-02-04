using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	private GameManager gameManagerScript;

	void Start () {
		gameManagerScript = GetComponent<GameManager> ();
	}

	void Update () {
		if (Input.anyKeyDown) {
			string characters = Input.inputString.ToLower();

			if (characters.Length < 1) {
				return;
			}

			gameManagerScript.onKeyDown (characters);
		}
	}
}

