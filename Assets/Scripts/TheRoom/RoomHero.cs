using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class RoomHero : MonoBehaviour {
	public bool isHeroSlashing = false;

	private Animator animator;
	private SpriteRenderer r;
	private RoomManager roomManagerScript;

	void Start () {
		roomManagerScript = gameObject.GetComponentInParent<RoomManager> ();
		animator = GetComponent<Animator> ();
		r = GetComponent<SpriteRenderer>();
	}

	void Update () {
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("HeroDeath")) { // Need to know when the animation is actually playing, so we get the right time to destroy self
			transform.position += new Vector3 (0, -0.003f, 0); // Fall towards ground on death
			Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length); // Destroy self on animation end
			return;
		}

		isHeroSlashing = animator.GetCurrentAnimatorStateInfo (0).IsName ("HeroSlash");
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.gameObject.tag != "Enemy") { // only concerned with enemy collisions
			return;
		}

		if (!isHeroSlashing) {
			animator.Play("HeroDeath"); // Play hero death animation
			roomManagerScript.killHero();
			return;
		}

		Destroy (collision.gameObject); // kill enemy if hero slashed
		roomManagerScript.addPoint (); // add point if hero slashed
	}

	// Calls:
	// - RoomManager
	public void onHeroAttack() {
		if (!isHeroSlashing) {
			animator.SetTrigger ("Attack");
		}
	}

	// Calls:
	// - RoomManager 
	public void onStartGame() {
		r.enabled = true;
	}
}
