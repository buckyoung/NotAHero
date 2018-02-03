using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Screen : MonoBehaviour {
	public float speed;

	private Renderer r;

	void Start () {
		r = gameObject.GetComponent<Renderer>();
	}

	void Update () {
		Vector2 offset = new Vector2(Time.deltaTime*speed, 0);
		r.material.mainTextureOffset += offset;
	}
}