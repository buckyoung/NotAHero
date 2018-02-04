using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Screen : MonoBehaviour {
	private float speed = 0.15f;

	private Renderer r;
	private Material gameBackdrop;
	private RoomManager roomManagerScript;

	void Start () {
		roomManagerScript = gameObject.GetComponentInParent<RoomManager> ();

		r = gameObject.GetComponent<Renderer>();

		gameBackdrop = Resources.Load("GameBk", typeof(Material)) as Material;
	}

	void Update () {
		if (roomManagerScript.shouldBackgroundMove) {
			Vector2 offset = new Vector2 (Time.deltaTime * speed, 0);
			r.material.mainTextureOffset += offset;
		}
	}

	public void powerMonitorOn() {
		Material[] mats = r.materials;
		mats [0] = gameBackdrop;
		r.materials = mats;
	}
}