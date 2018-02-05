using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private const float ROTATE_SPEED = 10.0f;

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

	private GameObject roomInstanceObject;
	private GameObject officeInstanceObject;

	// UI
	public int day = 0;

	public int todaysRoomScore = 0;
	public int highestRoomScore = 0;
	public int totalRoomScore = 0;

	public int todaysOfficeScore = 0;
	public int highestOfficeScore = 0;
	public int totalOfficeScore = 0;

	public Text theRoomText;

	void Start () {
		loadHome ();

		humanTransform = GameObject.Find ("Human").transform;
		humanArmManagerScripts = GetComponentsInChildren<HumanArmManager> ();
		humanTransform.eulerAngles = new Vector3 (0, 0, 0);

		theRoomText.text = "";

	}

	void Update() {
		if (shouldHumanRotate) {
			humanTransform.RotateAround (humanTransform.position, Vector3.up, Time.deltaTime*ROTATE_SPEED);

			if (isGoingToOffice && humanTransform.eulerAngles.y > 179.8) {
				humanTransform.eulerAngles = new Vector3 (0, 180, 0);
				shouldHumanRotate = false;

				isGoingToOffice = false;
				isAtOffice = true;
				incrementDay ();
			} else if (isGoingHome && humanTransform.eulerAngles.y > 359.8) {
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
	// - RoomManager 5 seconds after game ends
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
		totalRoomScore += score;

		if (score > highestRoomScore) {
			highestRoomScore = score;
		}

		if (day == 0) { 
			theRoomText.text = "High Score: " + highestRoomScore;
			return;
		}

		theRoomText.text = "Last Score: " + todaysRoomScore + "\n" + "High Score: " + highestRoomScore + "\n" + "Total Score: " + totalRoomScore;
	}

	public void incrementDay() {
		day++;
		// TODO UPDATE LABEL
	}

	private void loadOffice() {
		officeInstanceObject = (GameObject)Instantiate(
			Resources.Load("TheOffice"), 
			new Vector3(0,0,-15),
			Quaternion.Euler(new Vector3(0,180,0))
		);

		officeInstanceObject.transform.parent = transform;
//		roomManagerScript = officeInstanceObject.GetComponent<RoomManager> ();

		isOfficeLoaded = true;
	}

	private void loadHome() {
		roomInstanceObject = (GameObject)Instantiate(
			Resources.Load("TheRoom"), 
			new Vector3(0,0,3),
			Quaternion.identity
		);

		roomInstanceObject.transform.parent = transform;
		roomManagerScript = roomInstanceObject.GetComponent<RoomManager> ();

		isHomeLoaded = true;
	}

	private void unloadOffice() {
		GameObject.Destroy (officeInstanceObject);
		isOfficeLoaded = false;
	}

	private void unloadHome() {
		GameObject.Destroy (roomInstanceObject);
		isHomeLoaded = false;
	}
}
