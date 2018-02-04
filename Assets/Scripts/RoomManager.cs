using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
	private const float KEYBOARD_MASS = 1.0f;

	private Screen screenScript;
	private Arm armScript;
	private BadGuyEmitter badGuyEmitterScript;
	private Hero heroScript;

	private Rigidbody keyboardRb;
	private Animator heroAnimator;
	private SpriteRenderer heroRenderer;
	private Transform humanTransform;

	public bool isComputerOn = false;
	public bool shouldBackgroundMove = false;
	public bool shouldEmit = false;
	public bool isHeroSlashing = false;
	public bool isGameOver = false;

	void Start () {
		screenScript = gameObject.GetComponentInChildren<Screen> ();
		armScript = gameObject.GetComponentInChildren<Arm> ();
		badGuyEmitterScript = gameObject.GetComponentInChildren<BadGuyEmitter> ();
		heroScript = gameObject.GetComponentInChildren<Hero> ();

		humanTransform = GameObject.Find ("Human").transform;
		heroAnimator = GameObject.Find ("Hero").GetComponent<Animator>();
		heroRenderer = GameObject.Find ("Hero").GetComponent<SpriteRenderer>();
		keyboardRb = GameObject.Find ("Keyboard").GetComponent<Rigidbody>();
	}

	void Update() {
		if (isGameOver) {
			return;
		}

		if (heroAnimator != null) {
			isHeroSlashing = heroAnimator.GetCurrentAnimatorStateInfo (0).IsName ("HeroSlash");
		}
	}

	// Calls:
	// - ArmScript on first key down
	public void bootComputer() {
		if (isGameOver) {
			return;
		}

		isComputerOn = true;
		screenScript.powerMonitorOn ();
		StartCoroutine(startGame());
	}

	// Calls:
	// - ArmScript on key down
	public void heroAttack() { 
		if (isGameOver) {
			return;
		}

		if (heroAnimator != null && !isHeroSlashing) {
			heroAnimator.SetTrigger ("Attack"); // run hero slash animation
		}
	}

	// Calls:
	// - BadGuy on collision
	public void killHero() {
		if (isGameOver) {
			return;
		}

		heroAnimator.Play("HeroDeath"); // Play hero death
		keyboardRb.mass = KEYBOARD_MASS; // Be able to throw keyboard when you die
		endGame();
	}

	// Calls:
	// - BadGuy on collision
	public void addPoint() {
		// TODO BUCK
	}

	private IEnumerator startGame() {
		yield return new WaitForSeconds(2);
		heroRenderer.enabled = true;
		shouldBackgroundMove = true;
		StartCoroutine(startEmitting());
	}

	private IEnumerator startEmitting() {
		yield return new WaitForSeconds(5);
		shouldEmit = true;
	}

	private void endGame() {
		isGameOver = true;
		shouldEmit = false;
		shouldBackgroundMove = false;

		StartCoroutine(goToWork ());
	}

	private IEnumerator goToWork() {
		yield return new WaitForSeconds(1);
	
		humanTransform.RotateAround (humanTransform.position, Vector3.up, 180);
	}
}