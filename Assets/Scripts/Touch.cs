using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets;

public class Touch : MonoBehaviour {

	Particle particle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < Input.touchCount; i++) {
			if (Input.GetTouch (i).phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (i).position);
				if (Physics.Raycast (ray)) {
					// Create a particle if hit
					Debug.Log("input"+transform.ToString());
				}
			}
		}
	}
}
