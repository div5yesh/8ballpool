using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPG
{
    public class TableManager : MonoBehaviour
    {
        public static TableManager Instance;

        public GameObject[] balls;

        public GameObject cueBall;

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
            cueBall = GameObject.FindGameObjectWithTag("CueBall");
        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdateActiveTableObjects()
        {
            balls = GameObject.FindGameObjectsWithTag("Ball");
        }

        public bool IsTableSleeping()
        {
            UpdateActiveTableObjects();

            bool tableSleeping = true;
            foreach (var ball in balls)
            {
                tableSleeping = tableSleeping && ball.GetComponent<Rigidbody>().IsSleeping();
            }

            tableSleeping = tableSleeping && cueBall.GetComponent<Rigidbody>().IsSleeping();

            return tableSleeping;
        }
    } 
}
