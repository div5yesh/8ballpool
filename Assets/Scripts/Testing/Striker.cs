using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Striker : MonoBehaviour {
    public float force;
    
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (!Input.GetKeyDown(KeyCode.S))
#else
        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began)
#endif
            return;
        GetComponent<Rigidbody>().AddForce(transform.forward * force);
	}
}