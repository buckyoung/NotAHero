using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private const float ROTATE_SPEED = 15.0f;

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
	private OfficeManager officeManagerScript;

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

	public Text[] theRoomScores;
	public Text[] officeScores;
	public Text[] dayTexts;

	void Start () {
		loadHome ();

		humanTransform = GameObject.Find ("Human").transform;
		humanArmManagerScripts = GetComponentsInChildren<HumanArmManager> ();
		humanTransform.eulerAngles = new Vector3 (0, 0, 0);

		setRoomScoreTexts ("");
		setOfficeScoreTexts ("");
		setDayTexts ("");
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
			} else if (isGoingHome && (humanTransform.eulerAngles.y > 359.8 || humanTransform.eulerAngles.y < 100)) {
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
		} else if (isAtOffice) {
			officeManagerScript.onKeyDown (input);
		}
	}

	// Calls:
	// - RoomManager on end game
	public void setOfficeScore(int score) {
		todaysOfficeScore = score;
		totalOfficeScore += score;

		if (score > highestOfficeScore) {
			highestOfficeScore = score;
		}

		if (day == 1) { 
			setOfficeScoreTexts ("Last Output: " + todaysOfficeScore);
			return;
		}

		setOfficeScoreTexts ("Last Output: " + todaysOfficeScore + "\n" + "Most Output: " + highestOfficeScore + "\n" + "Total Output: " + totalOfficeScore);
	}

	// Calls:
	// - OfficeManager on day end
	public void setRoomScore(int score) {
		todaysRoomScore = score;
		totalRoomScore += score;

		if (score > highestRoomScore) {
			highestRoomScore = score;
		}

		if (day == 0) { 
			setRoomScoreTexts ("High Score: " + highestRoomScore);
			return;
		}

		setRoomScoreTexts ("Last Score: " + todaysRoomScore + "\n" + "High Score: " + highestRoomScore + "\n" + "Total Score: " + totalRoomScore);
	}

	public void incrementDay() {
		day++;
		setDayTexts ("Day " + day.ToString ());
	}

	private void loadOffice() {
		officeInstanceObject = (GameObject)Instantiate(
			Resources.Load("TheOffice"), 
			new Vector3(0,0,-15),
			Quaternion.Euler(new Vector3(0,180,0))
		);

		officeInstanceObject.transform.parent = transform;
		officeManagerScript = officeInstanceObject.GetComponent<OfficeManager> ();

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

	private void setRoomScoreTexts(string value) {
		foreach (Text item in theRoomScores) {
			item.text = value;	
		}
	}

	private void setOfficeScoreTexts(string value) {
		foreach (Text item in officeScores) {
			item.text = value;	
		}
	}

	private void setDayTexts(string value) {
		foreach (Text item in dayTexts) {
			item.text = value;	
		}
	}
}
