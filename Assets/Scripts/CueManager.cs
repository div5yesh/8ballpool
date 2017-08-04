using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityRandom = UnityEngine.Random;

public class CueManager : NetworkBehaviour
{

	[SerializeField]
    private GameObject cue;

    public GameObject CuePrefab;

	int playerId;

	bool isInitialized = false;

	bool isActive = false;

	bool hasTurn = false;

    Quaternion ROTATION = Quaternion.Euler(0, 0, 1.76f);
    Vector3 ROTATION_AXIS = new Vector3(0, 1, 0);

    // Use this for initialization

     void Initialize()
    {
		cue = Instantiate (CuePrefab);
        cue.SetActive(false);
		cue.transform.SetParent (transform, true);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (hasTurn) {
			if (!isActive) {
//				Vector3 cueBall = GameObject.FindWithTag("CueBall").transform.position;
//				Vector3 position = new Vector3(cueBall.x + 0.775f, 0.82f, cueBall.z);
//				cue.transform.SetPositionAndRotation(position, ROTATION);
//				cue.transform.RotateAround(cueBall, ROTATION_AXIS, UnityRandom.Range(0, 360));
//				cue.SetActive(true);
//				isActive = true;
			}
		} else {
			if (isActive) {
//				cue.SetActive (false);
//				isActive = false;
			}
		}
    }

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		if (isInitialized) {
			return;
		}
		isInitialized = true;
		Initialize ();

		//if (hasAuthority) 
		{
			NetworkPlayer player = NetworkManager.Instance.GetPlayerById (playerId);
			if (player != null) {
				player.SetPlayerReady ();
			}
		}
	}

	public void SetPlayerId(int id)
    {
		playerId = id;
    }

    public void OnTurnEnd()
    {
        //if (hasAuthority)
        {
			hasTurn = false;
			cue.SetActive (false);
			isActive = false;
        }
    }
		
    public void OnTurnStart()
    {
        //if (hasAuthority)
        {
			hasTurn = true;
			Vector3 cueBall = GameObject.FindWithTag("CueBall").transform.position;
			Vector3 position = new Vector3(cueBall.x + 0.775f, 0.82f, cueBall.z);
			cue.transform.SetPositionAndRotation(position, ROTATION);
			cue.transform.RotateAround(cueBall, ROTATION_AXIS, UnityRandom.Range(0, 360));
			cue.SetActive(true);
			isActive = true;
        }
    }
}
