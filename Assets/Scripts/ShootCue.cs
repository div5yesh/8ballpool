using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class ShootCue : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 pos = eventData.pointerCurrentRaycast.worldPosition;
        int x = Mathf.FloorToInt(pos.x);
        int z = Mathf.FloorToInt(pos.z);
        Debug.Log("Shoot:" + x + "," + z);
    }

    public bool playerTurn = false;
    //private NetworkStartPosition spawnPoint;
    // Use this for initialization
    //void Start()
    //{
    //    if (isLocalPlayer)
    //    {
    //        Vector3 cuePosition = GameObject.FindWithTag("cueBall").transform.position;
    //        SpawnPlayer(cuePosition);
    //    }
    //}

    //[ClientRpc]
    //void SpawnPlayer(Vector3 cuePosition)
    //{
    //    if (isLocalPlayer)
    //    {
    //        transform.position = new Vector3(cuePosition.x, cuePosition.y, cuePosition.z);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

    }

    //public override void OnStartLocalPlayer()
    //{
    //    //GetComponent<MeshRenderer>().material.color = Color.blue;

    //}
}
