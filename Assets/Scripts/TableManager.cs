using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CPG
{
    public class TableManager : MonoBehaviour
    {
        public static TableManager Instance;

        public GameObject[] balls;

        Rigidbody cueBall;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdateActiveTableObjects()
        {
			cueBall = GameObject.FindGameObjectWithTag("CueBall").GetComponent<Rigidbody>();
            balls = GameObject.FindGameObjectsWithTag("Ball");
        }

        public bool IsTableSleeping()
        {
            UpdateActiveTableObjects();

            bool tableSleeping = true;
            foreach (var ball in balls)
            {
                if(ball.GetComponent<Rigidbody>().velocity.magnitude < 0.02)
                {
                    ball.GetComponent<Rigidbody>().Sleep();
                }
                tableSleeping = tableSleeping && ball.GetComponent<Rigidbody>().IsSleeping();
            }

            Debug.Log(cueBall.velocity.magnitude + tableSleeping.ToString());
            if (cueBall.velocity.magnitude > 0.02)
            {
                tableSleeping = false;
            }
            else {
                cueBall.Sleep();
            }

            return tableSleeping;
        }
    } 
}
