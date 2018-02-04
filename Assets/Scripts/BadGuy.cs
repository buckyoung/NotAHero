using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuy : MonoBehaviour {
	private float speed = -0.85f;
	private Animator heroAnimator;

	void Start() {
		GameObject hero = GameObject.Find ("Hero");

		if (hero != null) {
			heroAnimator = hero.GetComponent<Animator> ();
		} else {
			Destroy (gameObject); // kill self if no hero
		}
	}

	void Update () {
		transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.gameObject.name != "Hero") {
			return;
		}

		if (heroAnimator.GetCurrentAnimatorStateInfo (0).IsName ("HeroSlash")) {
			Destroy (gameObject);
		} else {
			heroAnimator.Play("HeroDeath");
		}
			
	}
}
