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
		public BallType type;

		public int ball;

		public virtual void Awake ()
		{
			
		}

		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{

		}

		void FixedUpdate ()
		{
			
		}

		void OnDestroy()
		{
			NetworkManager.Instance.UpdateScore (ball, type);
		}
	}
}