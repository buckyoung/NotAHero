using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuyEmitter : MonoBehaviour {
	private bool canEmit = true;

	void Update () {
		if (canEmit) {
			canEmit = false;

			GameObject resource = (GameObject)Instantiate(
				Resources.Load("BadGuy"), 
				transform.position,
				Quaternion.identity
			);

			resource.transform.SetParent(this.transform);
			Destroy(resource, 5.5f);

			StartCoroutine(waitRandom(2, 6));
		}
	}

	private IEnumerator waitRandom(int min, int max) {
		float randNum = Random.value  * (max - min) + min;
		yield return new WaitForSeconds(randNum);
		canEmit = true;
	}
}
