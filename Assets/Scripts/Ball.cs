using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPG
{

	public enum BallType{
		NONE,
		CUE,
		SOLID,
		STRIPE
	}

	public class Ball : MonoBehaviour
	{

		//    //the minimum speed
		//    public float minSpeed = 0.5f;
		//
		//    //the current time the ball has to be slowed down
		//    protected float m_slowTime;
		//
		//    //the time the ball has to be slowed down before its considered stopped
		//    public float slowTime = 1;
		//
		//    //the balls last position
		//    protected Vector3 lastPosition = Vector3.zero;
		//
		//    //the balls currnet speed.
		//    protected float Speed = 0;
		//
		//    protected Rigidbody m_rigidbody;

		public BallType type;

		public virtual void Awake ()
		{
//        m_rigidbody = gameObject.GetComponent<Rigidbody>();
		}

		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
//        if (Speed < minSpeed)
//        {
//            m_slowTime += Time.deltaTime;
//            if (m_slowTime > slowTime)
//            {
//                m_rigidbody.angularVelocity = Vector3.zero;
//                m_rigidbody.velocity = Vector3.zero;
//                m_rigidbody.isKinematic = true;
//                m_slowTime = 0;
//            }
//
//        }
		}

		void FixedUpdate ()
		{
//        Speed = (transform.position - lastPosition).magnitude / Time.deltaTime * 3.6f;
//        lastPosition = transform.position;
		}

		void OnDestroy(){
			
		}
	}
}