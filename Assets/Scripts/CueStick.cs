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

		LineRenderer lr;

        // Use this for initialization
        void Start()
        {
			lr = GetComponent<LineRenderer> ();
			lr.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void RotateCueStick(float deg)
        {
            Vector3 cueBallTranform = cueBall.transform.position;
            transform.RotateAround(cueBallTranform, ROTATION_AXIS, deg);
			CollisionRayCast ();
        }

		void CollisionRayCast ()
		{
			RaycastHit hit;

			Vector3 dir = cueBall.transform.position - transform.position;
			dir = new Vector3 (dir.x, 0f, dir.z);

//			Debug.DrawRay(cueBall.transform.position, dir);

			Ray ray = new Ray (cueBall.transform.position, dir);

			if (Physics.SphereCast(ray, 0.028575f, out hit)) 
			{
				if (hit.collider.tag == "Ball" || hit.collider.tag == "Wall") 
				{
					if (lr) {
						lr.SetPosition (0, cueBall.transform.position);
						lr.SetPosition (1, hit.point);
						lr.enabled = true;
					}
				}
//				Debug.Log (hit.collider);
			}
		}

		public void MoveCueBall(Vector3 v)
		{
			if (!cueBall)
			{
				cueBall = GameObject.FindGameObjectWithTag("CueBall");  
			}
			{
				cueBall.transform.position.Set (v.x, cueBall.transform.position.y, v.z);
			}
		}

        public void Shoot(float power)
        {
            if (NetworkManager.Instance.GameState != GameState.SHOOTING)
            {
                cueBall.GetComponent<Rigidbody>().AddForce((cueBall.transform.position - transform.position).normalized * power);
                cueBall.GetComponent<Rigidbody>().AddTorque(Vector3.zero);
                NetworkManager.Instance.GameState = GameState.SHOOTING;
				lr.enabled = false;
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
			CollisionRayCast ();
        }

        public void Unspawn()
        {
            gameObject.SetActive(false);
        }
    } 
}
