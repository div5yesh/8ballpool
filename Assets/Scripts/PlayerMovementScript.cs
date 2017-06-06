using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    Rigidbody rigidbody;

    public float force = 5;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0, y);
        rigidbody.AddForce(movement*force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "")
        {

        }
    }
}