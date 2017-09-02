using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityRandom = UnityEngine.Random;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public bool isTurn = false;

	    Quaternion ROTATION = Quaternion.Euler(0, 0, 1.76f);
	    Vector3 ROTATION_AXIS = new Vector3(0, 1, 0);

    //    [SerializeField]
    //    public GameObject CueManagerPrefab;
    //
    //	public event Action<NetworkPlayer> becameReady;
    //
    //    public CueManager cueManager;
    //    private int playerId;
    //
    //    [SerializeField]
    //    public GameObject TurnTimer;

    [SyncVar(hook = "UpdateTimeDisplay")]
    public float time = 15;

	void Start(){
        GetComponentInChildren<Canvas>().enabled = false;
		GetComponent<MeshRenderer> ().enabled = false;
	}
//
	[Client]
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);

        base.OnStartClient();
        Debug.Log("Client Network Player start");

        NetworkManager.Instance.RegisterNetworkPlayer(this);
    }

	public override void OnNetworkDestroy ()
	{
		base.OnNetworkDestroy ();
		NetworkManager.Instance.DeregisterNetworkPlayer(this);
	}

//
//    public override void OnStartLocalPlayer()
//    {
//        base.OnStartLocalPlayer();
//		OnPlayerReady ();
//    }
//
//    public void OnPlayerReady()
//    {
//		if (hasAuthority)
//        {
//            CmdClientReadyInScene();
//        }
//    }
//
//	public void SetPlayerId(int id){
//		playerId = id;
//	}
//
	public void SetPlayerReady(){
		if (hasAuthority) {
//			CmdSetPlayerReady ();
		}
	}

    private void Update()
    {
        if (isTurn)
        {
            time -= Time.deltaTime; 
        }
    }

    
    public void UpdateTimeDisplay(float curtime)
    {
        GameObject.FindWithTag("Timer").GetComponent<TextMesh>().text = Mathf.Round(curtime).ToString(); 
    }

    //
    //	[Command]
    //	public void CmdSetPlayerReady(){
    //		if (becameReady != null) {
    //			becameReady (this);
    //		}
    //	}
    //
    public void TurnEnd()
	{
        isTurn = false;
        GetComponent<MeshRenderer> ().enabled = false;
		GetComponent<PlayerMovement> ().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
		UnSpawnCue ();
	}
    //
    public void RpcTurnEnd()
    {
        isTurn = false;
        time = 0;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
        UnSpawnCue();
        Debug.Log("turn end");
    }
    //
    public void TurnStart()
    {
		SpawnCue ();
		GetComponent<MeshRenderer> ().enabled = true;
		GetComponent<PlayerMovement> ().enabled = true;
        GetComponentInChildren<Canvas>().enabled = true;
        isTurn = true;
    }
    //
    public void RpcTurnStart()
    {
        SpawnCue();
        time = 15;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponentInChildren<Canvas>().enabled = true;
        Debug.Log("turn start");
        isTurn = true;

    }
    //
    void SpawnCue(){

		Vector3 cueBall = GameObject.FindWithTag("CueBall").transform.position;
		Vector3 position = new Vector3(cueBall.x + 0.775f, 0.82f, cueBall.z);
		transform.SetPositionAndRotation(position, ROTATION);
		transform.RotateAround(cueBall, ROTATION_AXIS, UnityRandom.Range(0, 360));
	}

	void UnSpawnCue(){
		transform.position = Vector3.zero;
	}

//	[Command]
//    private void CmdClientReadyInScene()
//    {
//        Debug.Log("CmdClientReadyInScene"+connectionToClient.connectionId);
//        GameObject cueManagerObject = Instantiate(CueManagerPrefab);
//        NetworkServer.SpawnWithClientAuthority(cueManagerObject, connectionToClient);
//        cueManager = cueManagerObject.GetComponent<CueManager>();
//		cueManager.SetPlayerId (playerId);
//    }
}
