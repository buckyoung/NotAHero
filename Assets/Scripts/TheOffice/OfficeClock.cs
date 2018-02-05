using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeClock : MonoBehaviour {
	private const int LENGTH_OF_HOUR = 7;

	private TextMesh timeText;
	private OfficeManager officeManagerScript;

	private string[] times = {"09:00", "10:00", "11:00", "12:00", "01:00", "02:00", "03:00", "04:00", "05:00"};

	private int count = 0;

	void Start () {
		timeText = gameObject.GetComponentInChildren<TextMesh> ();
		officeManagerScript = gameObject.GetComponentInParent<OfficeManager> ();
		timeText.text = "";
	}

	public void powerOn() {
		timeText.text = times [count++];
		StartCoroutine(incrementHour());
	}

	private IEnumerator incrementHour() {
		yield return new WaitForSeconds(LENGTH_OF_HOUR);

		timeText.text = times [count++];

		if (count == times.Length) {
			officeManagerScript.endDay ();
			yield return null;
		} else {
			StartCoroutine (incrementHour ());
		}
	}
}
