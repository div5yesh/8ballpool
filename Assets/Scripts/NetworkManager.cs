using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : UnityEngine.Networking.NetworkManager
{
    public static NetworkManager Instance;

    public Rigidbody[] balls;
//
    int iActivePlayer = 0;
    //
    public bool Shooting = false;

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
//
    float MaxTime = 15;

    float TurnTime;
//
//	bool playersReady = false;
//
    List<NetworkPlayer> players;
//
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
//    // Use this for initialization
    void Start()
    {
        TurnTime = 0;
        players = new List<NetworkPlayer>();
    }
//
//    public NetworkPlayer GetActivePlayer()
//    {
//        //Debug.Log("getactiveplayer::"+ActivePlayer);
//        if (players.Count != 2)
//        {
//            return null;
//        }
//        return players[ActivePlayer];
//    }
//
//	public NetworkPlayer GetPlayerById(int id){
//		return players [id];
//	}
//
//    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    //public delegate void OnBallStop();
    //public static event OnBallStop onBallStop;
    //

    public bool gameon = false;
    private void UpdateTimer()
    {
        //if (playersReady)
		if(players.Count == 2 && !gameon)
        {
            gameon = true;
            AlterTurns();
    //        if (TurnTime <= 0)
    //        {
    //            Debug.Log("update timer");
    //            //TurnEnd();
				//AlterTurns();
    //        }
    //        else
    //        {
    //            if (TurnTime == MaxTime)
    //            {
				//	//Debug.Log ("turn");
    //                //TurnStart();
    //            }
    //            if (!Shooting)
    //            {
    //                TurnTime -= Time.deltaTime;
    //                players[ActivePlayer].time = TurnTime;
    //                GameObject.FindWithTag("Timer").GetComponent<TextMesh>().text = Mathf.Round(TurnTime).ToString(); 
    //            }
    //        }
        }

        CheckForTurnEnd();
    }

    public void CheckForTurnEnd()
    {
        if (Shooting)
        {
            bool tableSleeping = true;
            foreach (var ball in balls)
            {
                tableSleeping = tableSleeping && ball.IsSleeping();
            }

            if (tableSleeping)
            {
                AlterTurns();
            } 
        }
    }
//
//    public void TurnEnd()
//    {
//		players[ActivePlayer].TurnEnd();
//    }
//
	void AlterTurns(){

        if (players.Count > 0)
        {
            Shooting = false;
            players[ActivePlayer].TurnEnd();
            int next = GetNextTurn();
            ActivePlayer = next;
            players[ActivePlayer].TurnStart();
            TurnTime = MaxTime; 
        }
    }

    //    //Will always change turns
    int GetNextTurn()
    {
        return (ActivePlayer+1) % players.Count;
    }
//
//    //public override void OnClientConnect(NetworkConnection conn)
//    //{
//    //    //base.OnClientConnect(conn);
//    //    ClientScene.AddPlayer(conn, 0);
//    //}
//
//    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
//    //{
//    //    // Intentionally not calling base here - we want to control the spawning of prefabs
//    //    Debug.Log("OnServerAddPlayer");
//
//    //    NetworkPlayer newPlayer = Instantiate(NetworkPlayerPrefab);
//    //    DontDestroyOnLoad(newPlayer);
//    //    NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId);
//    //}
//
    public void RegisterNetworkPlayer(NetworkPlayer player)
    {
        if (players.Count <= 2)
        {
            players.Add(player); 
        }
//		player.SetPlayerId (players.Count);
//		player.becameReady += SetPlayersReady;
//		player.OnPlayerReady ();
    }

	public void DeregisterNetworkPlayer(NetworkPlayer player){
		players.Remove (player);
	}
//
//	public void SetPlayersReady(NetworkPlayer npl){
//		if (players.Count == 2) {
//			playersReady = true;
//		}
//	}
//
//    public void TurnStart()
//    {
//        int turn = GetNextTurn();
//		ActivePlayer = turn;
//        players[turn].StartTurn();
//    }
}
