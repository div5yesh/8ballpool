using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace CPG
{
    public class CueStick : MonoBehaviour
    {
        Quaternion ROTATION = Quaternion.Euler(0, 0, 1.76f);
        Vector3 ROTATION_AXIS = new Vector3(0, 1, 0);

        GameObject cueBall;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
			CollisionRayCast ();
        }

        public void RotateCueStick(float deg)
        {
            Vector3 cueBallTranform = cueBall.transform.position;
            transform.RotateAround(cueBallTranform, ROTATION_AXIS, deg);
        }

		void CollisionRayCast ()
		{
			RaycastHit hit;

			Vector3 dir = cueBall.transform.position - transform.position;
			dir = new Vector3 (dir.x, 0.2f, dir.z);
			// Cast a sphere wrapping character controller 10 meters forward
			// to see if it is about to hit anything.
			Debug.DrawRay(cueBall.transform.position, dir);

			LineRenderer lr = GetComponent<LineRenderer> ();
			lr.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
			lr.SetPosition (0, cueBall.transform.position);
			lr.SetPosition (1, dir * 100);

			if (Physics.Raycast(cueBall.transform.position, dir, out hit)) {
//				if (hit.collider.gameObject.tag == "Ball") {
					
//				}
			}
		}

        public void Shoot(float power)
        {
            if (NetworkManager.Instance.GameState != GameState.SHOOTING)
            {
                cueBall.GetComponent<Rigidbody>().AddForce((cueBall.transform.position - transform.position).normalized * power);
                cueBall.GetComponent<Rigidbody>().AddTorque(Vector3.zero);
                NetworkManager.Instance.GameState = GameState.SHOOTING; 
            }
        }

        public void Spawn()
        {
            if (!cueBall)
            {
                cueBall = GameObject.FindGameObjectWithTag("CueBall");  
            }

            Vector3 cueBallTranform = cueBall.transform.position;
            Vector3 position = new Vector3(cueBallTranform.x + 0.775f, 0.82f, cueBallTranform.z);
            transform.SetPositionAndRotation(position, ROTATION);
            transform.RotateAround(cueBallTranform, ROTATION_AXIS, UnityRandom.Range(0, 360));
            gameObject.SetActive(true);
        }

        public void Unspawn()
        {
            gameObject.SetActive(false);
        }
    } 
}
