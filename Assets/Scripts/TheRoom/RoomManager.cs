using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
	private const float NEW_KEYBOARD_MASS = 1.0f;
	private const int WAIT_TO_POWER_MONITOR = 0;
	private const int WAIT_TO_START_GAME = 2;
	private const int WAIT_BEFORE_EMIT = 4;
	private const int WAIT_AFTER_GAME_OVER = 4;
	private const int WAIT_AFTER_POWER_DOWN = 1;

	private RoomScreen screenScript;
	private RoomHero heroScript;

	private TextMesh scoreText;

	private Light[] roomLights;

	private GameManager gameManagerScript;

	private Rigidbody keyboardRb;

	public bool isComputerOn = false;
	public bool shouldBackgroundMove = false;
	public bool shouldEmit = false;
	public bool isGameOver = false;
	public bool isGameRunning = false;

	private int score = 0;

	void Start () {
		screenScript = gameObject.GetComponentInChildren<RoomScreen> ();
		heroScript = gameObject.GetComponentInChildren<RoomHero> ();
		scoreText = gameObject.GetComponentInChildren<TextMesh> ();

		roomLights = gameObject.GetComponentsInChildren<Light> ();

		scoreText.text = "";

		gameManagerScript = gameObject.GetComponentInParent<GameManager> ();

		keyboardRb = GameObject.Find ("Keyboard").GetComponent<Rigidbody>();
	}

	void Update() {
		if (isGameOver) {
			return;
		}
	}

	// Calls:
	// - GameManager on key down
	public void onKeyDown() { 
		if (isGameOver) {
			return;
		}

		if (!isComputerOn) {
			bootComputer ();
		}

		if (isGameRunning) {
			heroScript.onHeroAttack ();
		}
	}

	// Calls:
	// - RoomHero on collision
	public void killHero() {
		if (isGameOver) {
			return;
		}

		keyboardRb.mass = NEW_KEYBOARD_MASS; // Be able to throw keyboard when you die
		endGame();
	}

	// Calls:
	// - RoomHero on collision
	public void addPoint() {
		score++;
		scoreText.text = score.ToString();
	}

	private void bootComputer() {
		isComputerOn = true;
		turnOnLights ();
		StartCoroutine(powerMonitorOn ());
	}

	private IEnumerator powerMonitorOn() {
		yield return new WaitForSeconds(WAIT_TO_POWER_MONITOR);
		screenScript.powerMonitorOn ();
		StartCoroutine(startGame());
	}

	private IEnumerator startGame() {
		yield return new WaitForSeconds(WAIT_TO_START_GAME);
		heroScript.onStartGame ();
		shouldBackgroundMove = true;
		isGameRunning = true;
		scoreText.text = score.ToString();
		StartCoroutine(startEmitting());
	}

	private IEnumerator startEmitting() {
		yield return new WaitForSeconds(WAIT_BEFORE_EMIT);
		shouldEmit = true;
	}

	private void endGame() {
		isGameOver = true;
		shouldEmit = false;
		shouldBackgroundMove = false;

		gameManagerScript.setRoomScore (score);

		StartCoroutine(powerDown ());
	}

	private IEnumerator powerDown () {
		yield return new WaitForSeconds(WAIT_AFTER_GAME_OVER);
		turnOffLights ();
		scoreText.text = "";
		screenScript.powerMonitorOff ();
		StartCoroutine(goToWork ());
	}

	private IEnumerator goToWork() {
		yield return new WaitForSeconds(WAIT_AFTER_POWER_DOWN);
		gameManagerScript.goToWork ();
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