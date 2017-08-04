using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkPlayer : NetworkBehaviour
{

    [SerializeField]
    public GameObject CueManagerPrefab;

	public event Action<NetworkPlayer> becameReady;

    public CueManager cueManager;
    private int playerId;

    [SerializeField]
    public GameObject TurnTimer;

	[Client]
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);

        base.OnStartClient();
        Debug.Log("Client Network Player start");

        NetworkManager.Instance.RegisterNetworkPlayer(this);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
		OnPlayerReady ();
    }

    public void OnPlayerReady()
    {
		if (hasAuthority)
        {
            CmdClientReadyInScene();
        }
    }

	public void SetPlayerId(int id){
		playerId = id;
	}

	public void SetPlayerReady(){
		if (hasAuthority) {
			CmdSetPlayerReady ();
		}
	}

	[Command]
	public void CmdSetPlayerReady(){
		if (becameReady != null) {
			becameReady (this);
		}
	}

	public void TurnEnd()
	{
		RpcTurnEnd (NetworkManager.Instance.ActivePlayer);
	}

    [ClientRpc]
	void RpcTurnEnd(int id)
    {
		NetworkManager.Instance.GetPlayerById (id).cueManager.OnTurnEnd ();
        Debug.Log("turn end" + netId);
    }

    public void StartTurn()
    {
		RpcStartTurn (NetworkManager.Instance.ActivePlayer);
    }

	[ClientRpc]
	void RpcStartTurn(int id)
	{
		NetworkManager.Instance.GetPlayerById (id).cueManager.OnTurnStart ();
		Debug.Log("turn start" + netId);
	}

	[Command]
    private void CmdClientReadyInScene()
    {
        Debug.Log("CmdClientReadyInScene"+connectionToClient.connectionId);
        GameObject cueManagerObject = Instantiate(CueManagerPrefab);
        NetworkServer.SpawnWithClientAuthority(cueManagerObject, connectionToClient);
        cueManager = cueManagerObject.GetComponent<CueManager>();
		cueManager.SetPlayerId (playerId);
    }
}
