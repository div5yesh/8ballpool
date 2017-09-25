using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QueueBallRay : MonoBehaviour {
    void Update () {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
		
	}
}
