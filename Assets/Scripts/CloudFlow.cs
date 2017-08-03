using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFlow : MonoBehaviour {

	public float distancePerSec = 0.3f;

	Vector3 movement;

	void Start() {
		movement = transform.position;
	}

	void FixedUpdate () {
		movement.Set (movement.x + distancePerSec * Time.deltaTime, movement.y, movement.z);
		transform.position = movement;
	}
}
