using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityRandom = UnityEngine.Random;

public class CueManager : NetworkBehaviour {

    private GameObject cue;

    public GameObject CuePrefab;

    Quaternion ROTATION = Quaternion.Euler(0, 0, 1.76f);
    Vector3 ROTATION_AXIS = new Vector3(0, 1, 0);

    // Use this for initialization
    void Start () {
        Vector3 cueBall = GameObject.FindWithTag("CueBall").transform.position;
        cue = Instantiate(CuePrefab, new Vector3(cueBall.x + 0.775f, 0.82f, cueBall.z), ROTATION);
        cue.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();

    //        Initialize();
    //}

    //public void Initialize()
    //{
 
    //}

    public void OnTurnEnd()
    {
        cue.SetActive(false);
    }

    public void OnTurnStart()
    {
        Vector3 cueBall = GameObject.FindWithTag("CueBall").transform.position;
        Vector3 position = new Vector3(cueBall.x + 0.775f, 0.82f, cueBall.z);
        cue.transform.SetPositionAndRotation(position, ROTATION);
        cue.transform.RotateAround(cueBall, ROTATION_AXIS, UnityRandom.Range(0, 360));
        cue.SetActive(true);
    }

    public void SetPlayerId(int playerId)
    {

    }
}
