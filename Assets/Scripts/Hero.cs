using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
	private Animator animator;
	private RoomManager roomManagerScript;

	void Start () {
		roomManagerScript = gameObject.GetComponentInParent<RoomManager> ();

		animator = GetComponent<Animator> ();
	}

	void Update () {
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("HeroDeath")) { // Need to know when the animation is actually playing, so we get the right time to destroy self
			transform.position += new Vector3 (0, -0.003f, 0); // Fall towards ground on death
			Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length); // Destroy self on animation end
		}
	}
}
