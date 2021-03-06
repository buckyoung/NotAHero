﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class RoomScreen : MonoBehaviour {
	private float speed = 0.15f;

	private Renderer r;
	private Material gameBackdrop;
	private Material monitorOffMaterial;
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

	void OnDestroy() {
		Resources.UnloadAsset (gameBackdrop);
		Resources.UnloadUnusedAssets ();
	}

	public void powerMonitorOn() {
		Material[] mats = r.materials;
		monitorOffMaterial = mats [0]; // cache this mat
		mats [0] = gameBackdrop;
		r.materials = mats;
	}

	public void powerMonitorOff() {
		Material[] mats = r.materials;
		mats [0] = monitorOffMaterial;
		r.materials = mats;
	}
}