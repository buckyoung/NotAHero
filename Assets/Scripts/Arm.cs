using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Arm : MonoBehaviour {
	public enum ArmToInt {left = 0, right = 1}
	public ArmToInt respondsTo;

	public Vector3 forceVector = new Vector3(1, 0, 0);
	private int forceScaling = 600;

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (Input.anyKeyDown) {
			if (Input.inputString.GetHashCode() % 2 == (int)respondsTo) {
				var magnitude = Input.inputString.Length * forceScaling;
				rb.AddRelativeForce(forceVector * magnitude);
			}
		}
	}
}

