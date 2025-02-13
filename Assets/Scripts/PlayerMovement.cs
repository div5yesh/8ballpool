﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets;

public class PlayerMovement : NetworkBehaviour
{
    public float magnitude = 2f;
    public float timer = 30f;

    GameObject cueBall;
    float startTime;
    bool shooting = false;
    Vector3 initialPosition;

    Vector3 ROTATION_AXIS = new Vector3(0, 1, 0);

    // Use this for initialization
    void Start()
    {
        cueBall = GameObject.FindWithTag("CueBall");
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer)
        //{
        //    return;
        //}

        var rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var power = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        float force = Vector3.Distance(transform.position, cueBall.transform.position);

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            shooting = false;
            transform.RotateAround(cueBall.transform.position, ROTATION_AXIS, rotation);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            shooting = false;
        }

        if (force <= 1.25)
        {
            transform.Translate(Math.Abs(power), 0, 0);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            startTime = Time.time;
            shooting = true;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            initialPosition = transform.position;
        }

            RecoilCue(force);
        //if (cueBall.GetComponent<Rigidbody>().IsSleeping())
        {
            //SpawnManager.Instance.isPlayerTurn = SpawnManager.Instance.isPlayerTurn == 0 ? 1 : 0;
        }
    }

    void RecoilCue(float force)
    {
        if (shooting)
        {
            float fracJourney = force / 1.25f;
            Vector3 newPosition = Vector3.Lerp(transform.position, initialPosition, fracJourney);
            transform.position = newPosition;
        }
    }

    [Server]
    private void OnTriggerEnter(Collider collider)
    {
        if (shooting)
        {
            if (collider.gameObject.tag == "CueBall")
            {
                cueBall.GetComponent<Rigidbody>().AddForce((cueBall.transform.position - transform.position) * 500);
                //NetworkServer.UnSpawn();
                shooting = false;
            }
        }
    }
}