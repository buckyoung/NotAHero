using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Arm : MonoBehaviour {
	public enum ArmEnum {left = 0, right = 1}
	public ArmEnum respondsTo;

	public Vector3 forceVector = new Vector3(1, 0, 0);

	private int forceScaling = 600;
	private Rigidbody rb;

	private Hashtable handHash = new Hashtable();
	private int[] leftHandChars = new int[] {'1','2','3','4','5','6','`','a','b','c','d','e','f','g','q','r','s','t','v','w','x','z', ' '};
	private int[] rightHandChars = new int[] {',','-','.','/','0','7','8','9',';','=','[',']','h','i','j','k','l','m','n','o','p','u','y'};
	private Animator heroAnimator;
	private Hashtable actionMap = new Hashtable();

	void Start () {
		rb = GetComponent<Rigidbody>();

		foreach (var character in leftHandChars) {
			handHash.Add (character, ArmEnum.left);	
		}

		foreach (var character in rightHandChars) {
			handHash.Add (character, ArmEnum.right);	
		}

		heroAnimator = GameObject.Find ("Hero").GetComponent<Animator> ();

		// Coordinate player actions to handedness
		actionMap.Add(ArmEnum.left, "Attack");
		actionMap.Add(ArmEnum.right, "Attack");
	}

	void Update () {
		if (Input.anyKeyDown) {
			string characters = Input.inputString.ToLower();

			if (characters.Length < 1) {
				return;
			}

			int character = characters[0].GetHashCode(); // use the handedness depending on the first character pressed

			if (handHash.ContainsKey(character) && (ArmEnum)handHash[character] == respondsTo) {
				var magnitude = Input.inputString.Length * forceScaling;
				rb.AddRelativeForce(forceVector * magnitude); // add force that scales with how many keys were hit

				heroAnimator.SetTrigger ((string)actionMap [respondsTo]);
			}
		}
	}
}

