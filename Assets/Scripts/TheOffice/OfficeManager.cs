using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour {
	private const float NEW_KEYBOARD_MASS = 1.0f;
	private const int WAIT_AFTER_DAY_ENDS = 2;

	private OfficeScreen screenScript;
	private OfficeClock clockScript;
	private TextMesh scoreText;
	private TextMesh outputText;
	private Light[] roomLights;
	private GameManager gameManagerScript;
	private Rigidbody keyboardRb;

	public bool isComputerOn = false;
	public bool isDayOver = false;

	private int score = 0;

	void Start () {
		screenScript = gameObject.GetComponentInChildren<OfficeScreen> ();
		clockScript = gameObject.GetComponentInChildren<OfficeClock> ();
		scoreText = GameObject.Find ("OfficeScreenScore").GetComponent<TextMesh>();
		outputText = GameObject.Find ("OfficeScreenOutput").GetComponent<TextMesh>();

		roomLights = gameObject.GetComponentsInChildren<Light> ();

		scoreText.text = "";
		outputText.text = "";

		gameManagerScript = gameObject.GetComponentInParent<GameManager> ();

		keyboardRb = GameObject.Find ("OfficeKeyboard").GetComponent<Rigidbody>();
		turnOffLights ();
	}

	// Calls:
	// - GameManager on key down
	public void onKeyDown(string input) { 
		if (isDayOver) {
			return;
		}

		if (!isComputerOn) {
			bootComputer ();
			return;
		}

		addPoints (input.Length);
		displayInput (input);
	}

	private void displayInput(string input) {
		if (input.Length > 1) {
			char c = input[0];
			outputText.text = c + "...";
			return;
		}

		outputText.text = input;
	}

	// Calls:
	// - OfficeClock on end of day
	public void endDay() {
		isDayOver = true;
		gameManagerScript.setOfficeScore (score);
		keyboardRb.mass = NEW_KEYBOARD_MASS; // Be able to throw keyboard when day is over
		turnOffLights ();
		scoreText.text = "";
		outputText.text = "";
		screenScript.powerMonitorOff ();
		StartCoroutine(goHome ());
	}

	private void addPoints(int amount) {
		score += amount;
		scoreText.text = score.ToString();
	}

	private void bootComputer() {
		isComputerOn = true;
		turnOnLights ();
		scoreText.text = "0";
		screenScript.powerMonitorOn ();
		clockScript.powerOn ();
	}

	private IEnumerator goHome () {
		yield return new WaitForSeconds(WAIT_AFTER_DAY_ENDS);
		gameManagerScript.goHome ();
	}

	private void turnOffLights() {
		foreach (Light light in roomLights) {
			light.enabled = false;
		}
	}

	private void turnOnLights() {
		foreach (Light light in roomLights) {
			light.enabled = true;
		}
	}
}