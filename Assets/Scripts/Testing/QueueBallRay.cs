using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QueueBallRay : MonoBehaviour
{
	void Update ()
	{
		Debug.DrawRay (transform.position, transform.forward * 2, Color.red);
	}

	void DrawGuideLine ()
	{
		LineRenderer lr = GetComponent<LineRenderer> ();
		lr.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
		lr.SetPosition (0, Vector3.zero);
		lr.SetPosition (1, new Vector3 (1, 0, 1));
	}
}
