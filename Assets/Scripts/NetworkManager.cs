using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    public static NetworkManager Instance;

    int iActivePlayer = -1;

    public int ActivePlayer
    {
        get
        {
            return iActivePlayer;
        }
        set
        {
            iActivePlayer = value;
        }
    }

    float MaxTime = 30;

    float TurnTime;

	bool playersReady = false;

    List<NetworkPlayer> players;

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
        TurnTime = MaxTime;
        players = new List<NetworkPlayer>();
    }

    public NetworkPlayer GetActivePlayer()
    {
        //Debug.Log("getactiveplayer::"+ActivePlayer);
        if (players.Count != 2)
        {
            return null;
        }
        return players[ActivePlayer];
    }

	public NetworkPlayer GetPlayerById(int id){
		return players [id];
	}

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (playersReady)
        {
            if (TurnTime <= 0)
            {
                Debug.Log("update timer");
                TurnEnd();
                TurnTime = MaxTime;
            }
            else
            {
                if (TurnTime == MaxTime)
                {
                    TurnStart();
                }
                TurnTime -= Time.deltaTime;
                GameObject.FindWithTag("Timer").GetComponent<TextMesh>().text = Mathf.Round(TurnTime).ToString();
            }
        }
    }

    public void TurnEnd()
    {
		players[ActivePlayer].TurnEnd();
    }

    //Will always change turns
    int GetNextTurn()
    {
        return (++ActivePlayer) % players.Count;
    }

    //public override void OnClientConnect(NetworkConnection conn)
    //{
    //    //base.OnClientConnect(conn);
    //    ClientScene.AddPlayer(conn, 0);
    //}

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    // Intentionally not calling base here - we want to control the spawning of prefabs
    //    Debug.Log("OnServerAddPlayer");

    //    NetworkPlayer newPlayer = Instantiate(NetworkPlayerPrefab);
    //    DontDestroyOnLoad(newPlayer);
    //    NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId);
    //}

    public void RegisterNetworkPlayer(NetworkPlayer player)
    {
        players.Add(player);
		player.SetPlayerId (players.Count);
		player.becameReady += SetPlayersReady;
//		player.OnPlayerReady ();
    }

	public void SetPlayersReady(NetworkPlayer npl){
		if (players.Count == 2) {
			playersReady = true;
		}
	}

    public void TurnStart()
    {
        int turn = GetNextTurn();
		ActivePlayer = turn;
        players[turn].StartTurn();
    }
}
