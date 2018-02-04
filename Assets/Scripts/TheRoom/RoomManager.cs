using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
	private const float KEYBOARD_MASS = 1.0f;
	private const int WAIT_TO_START_GAME = 2;
	private const int WAIT_BEFORE_EMIT = 5;
	private const int WAIT_AFTER_GAME_OVER = 7;

	private RoomScreen screenScript;
	private RoomHero heroScript;

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

		keyboardRb.mass = KEYBOARD_MASS; // Be able to throw keyboard when you die
		endGame();
	}

	// Calls:
	// - RoomHero on collision
	public void addPoint() {
		score++;
	}

	private void bootComputer() {
		isComputerOn = true;
		screenScript.powerMonitorOn ();
		StartCoroutine(startGame());
	}

	private IEnumerator startGame() {
		yield return new WaitForSeconds(WAIT_TO_START_GAME);
		heroScript.onStartGame ();
		shouldBackgroundMove = true;
		isGameRunning = true;
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

		StartCoroutine(goToWork ());
	}

	private IEnumerator goToWork() {
		yield return new WaitForSeconds(WAIT_AFTER_GAME_OVER);
		gameManagerScript.goToWork ();
	}
}