using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionExit(Collision collision)
	{
		GameObject ball = collision.collider.gameObject;
		ball.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
	}
}
