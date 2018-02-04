using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
	private Animator animator;

	void Start () {
		animator = GetComponent<Animator> ();
	}

	void Update () {
		if (animator.GetCurrentAnimatorStateInfo (0).IsName("HeroDeath")) {
			transform.position += new Vector3 (0, -0.003f, 0);// fall towards ground on death
			Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
		}
	}
}
