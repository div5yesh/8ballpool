using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    [SerializeField]
    public NetworkPlayer NetworkPlayerPrefab;

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
        players = new List<NetworkPlayer>();
    }

    public NetworkPlayer GetActivePlayer()
    {
        Debug.Log("getactiveplayer::"+ActivePlayer);
        return players.Count == 2 ? players[ActivePlayer] : null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnEnd()
    {
        int turn = GetNextTurn();
        players[turn].RpcStartTurn();
        ActivePlayer = turn;
    }

    //Will always change turns
    int GetNextTurn()
    {
        return (ActivePlayer == 0) ? 1 : 0;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        //base.OnClientConnect(conn);
        ClientScene.AddPlayer(conn, 0);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // Intentionally not calling base here - we want to control the spawning of prefabs
        Debug.Log("OnServerAddPlayer");

        NetworkPlayer newPlayer = Instantiate(NetworkPlayerPrefab);
        DontDestroyOnLoad(newPlayer);
        NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId);
    }

    public void RegisterNetworkPlayer(NetworkPlayer player)
    {
        players.Add(player);

        if (players.Count == 2)
        {
            int turn = GetNextTurn();
            players[turn].RpcStartTurn();
            ActivePlayer = turn;
        }
        else
        {
            Debug.Log("waiting for players::" + players.Count);
        }
    }
}
