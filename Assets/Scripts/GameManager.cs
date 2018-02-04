using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private const float ROTATE_SPEED = 20.0f;

	private bool shouldHumanRotate = false;

	private bool isHomeLoaded = true;
	private bool isAtHome = true;
	private bool isGoingHome = false;

	private bool isOfficeLoaded = false;
	private bool isAtOffice = false;
	private bool isGoingToOffice = false;

	private Transform humanTransform;

	private HumanArmManager[] humanArmManagerScripts;
	private RoomManager roomManagerScript;

	// UI
	public int day = 1;

	public int todaysRoomScore = 0;
	public int highestRoomScore = 0;

	public int todaysOfficeScore = 0;
	public int highestOfficeScore = 0;



	void Start () {
		humanTransform = GameObject.Find ("Human").transform;

		humanArmManagerScripts = GetComponentsInChildren<HumanArmManager> ();

		roomManagerScript = gameObject.GetComponentInChildren<RoomManager> ();

		GameObject resource = (GameObject)Instantiate(
			Resources.Load("TheRoom"), 
			transform.position,
			Quaternion.identity
		);
	}

	void Update() {
		if (shouldHumanRotate) {
			humanTransform.RotateAround (humanTransform.position, Vector3.up, Time.deltaTime*ROTATE_SPEED);

			if (isGoingToOffice && humanTransform.eulerAngles.y > 178) {
				humanTransform.eulerAngles = new Vector3 (0, 180, 0);
				shouldHumanRotate = false;

				isGoingToOffice = false;
				isAtOffice = true;
			} else if (isGoingHome && humanTransform.eulerAngles.y > 358) {
				humanTransform.eulerAngles = new Vector3 (0, 0, 0);
				shouldHumanRotate = false;

				isGoingHome = false;
				isAtHome = true;
			}

			if (isGoingToOffice && !isOfficeLoaded) {
				loadOffice ();
			}

			if (isGoingHome && !isHomeLoaded) {
				loadHome ();
			}

			if (isAtHome && isOfficeLoaded) {
				unloadOffice ();
			}

			if (isAtOffice && isHomeLoaded) {
				unloadHome ();
			}
		}
	}

	// Calls:
	// - RoomManager after game ends
	public void goToWork() {
		shouldHumanRotate = true;
		isGoingToOffice = true;
		isAtHome = false;
	}

	public void goHome() {
		shouldHumanRotate = true;
		isGoingHome = true;
		isAtOffice = false;
	}

	// Calls:
	// - InputManager during input
	public void onKeyDown(string input) {
		foreach (var humanArmManagerScript in humanArmManagerScripts) { // send event to each arm
			humanArmManagerScript.onKeyDown (input);	
		}

		if (isAtHome) {
			roomManagerScript.onKeyDown ();
		}
	}

	// Calls:
	// - RoomManager on end game
	public void setRoomScore(int score) {
		todaysRoomScore = score;

		if (score > highestRoomScore) {
			highestRoomScore = score;
		}
	}

	private void loadOffice() {
		// TODO BUCK RESUOURCES DOT LOAD PREFAB
		isOfficeLoaded = true;
	}

	private void loadHome() {
		// TODO BUCK RESUOURCES DOT LOAD PREFAB
		isHomeLoaded = true;
	}

	private void unloadOffice() {
		// TODO BUCK RESUOURCES DESTROY PREFAB
		isOfficeLoaded = true;
	}

	private void unloadHome() {
		// TODO BUCK RESUOURCES DESTROY PREFAV
		isHomeLoaded = true;
	}
}
