using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class OfficeScreen : MonoBehaviour {
	private Renderer r;
	private Material screenOn;
	private Material monitorOffMaterial;

	void Start () {
		r = gameObject.GetComponent<Renderer>();
		screenOn = Resources.Load("OfficeScreenOn", typeof(Material)) as Material;
	}

	void OnDestroy() {
		Resources.UnloadAsset (screenOn);
		Resources.UnloadUnusedAssets ();
	}

	public void powerMonitorOn() {
		Material[] mats = r.materials;
		monitorOffMaterial = mats [0]; // cache this mat
		mats [0] = screenOn;
		r.materials = mats;
	}

	public void powerMonitorOff() {
		Material[] mats = r.materials;
		mats [0] = monitorOffMaterial;
		r.materials = mats;
	}
}