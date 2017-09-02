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

        float power = 20;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RotateCueStick(float deg)
        {
            Vector3 cueBallTranform = cueBall.transform.position;
            transform.RotateAround(cueBallTranform, ROTATION_AXIS, deg);
        }

        public void Shoot(float power)
        {
            cueBall.GetComponent<Rigidbody>().AddForce((cueBall.transform.position - transform.position).normalized * power, ForceMode.Impulse);
            cueBall.GetComponent<Rigidbody>().AddTorque(Vector3.zero);
            NetworkManager.Instance.GameState = GameState.SHOOTING;
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
