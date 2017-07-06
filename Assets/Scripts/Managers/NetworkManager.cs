using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

namespace Cash8BallPool.Networking
{
    public enum SceneChangeMode
    {
        None,
        Game,
        Menu
    }

    public enum NetworkState
    {
        Inactive,
        Pregame,
        Connecting,
        InLobby,
        InGame
    }

    public enum NetworkGameType
    {
        Matchmaking,
        Direct,
        Singleplayer
    }

    public class NetworkManager : UnityEngine.Networking.NetworkManager
    {

        [SerializeField]
        protected uint MultiplayerMaxPlayers = 2;

        public event Action<bool, MatchInfo> matchCreated;

        public event Action<bool, MatchInfo> matchJoined;

        private Action<bool, MatchInfo> NextMatchCreatedCallback;

        public static NetworkManager Instance
        {
            get;
            protected set;
        }

        public NetworkState state
        {
            get;
            protected set;
        }

        public NetworkGameType gameType
        {
            get;
            protected set;
        }

        public List<NetworkPlayer> connectedPlayers
        {
            get;
            private set;
        }

        #region Unity Methods

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;

                connectedPlayers = new List<NetworkPlayer>();
            }
        }

        // Use this for initialization
        void Start()
        {
            state = NetworkState.Inactive;
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        #endregion

        #region Methods

        public void StartMatchmakingGame(string gameName, Action<bool, MatchInfo> onCreate)
        {
            if (state != NetworkState.Inactive)
            {
                throw new InvalidOperationException("Network currently active. Disconnect first.");
            }

            state = NetworkState.Connecting;
            gameType = NetworkGameType.Matchmaking;

            StartMatchMaker();
            NextMatchCreatedCallback = onCreate;

            matchMaker.CreateMatch(gameName, MultiplayerMaxPlayers, true, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchCreate);
        }

        public void StartMatchingmakingClient()
        {
            if (state != NetworkState.Inactive)
            {
                throw new InvalidOperationException("Network currently active. Disconnect first.");
            }

            state = NetworkState.Pregame;
            gameType = NetworkGameType.Matchmaking;
            StartMatchMaker();
        }

        private void OnList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            NetworkID netID = matches[matches.Count - 1].networkId;
        }

        public void ListMatches(string matchName, Action<bool, NetworkID>OnListMatches)
        {
            matchMaker.ListMatches(0, 10, matchName, true, 0, 0, OnList);
        }

        public void JoinMatchmakingGame(NetworkID networkId, Action<bool, MatchInfo> onJoin)
        {
            if (gameType != NetworkGameType.Matchmaking ||
                state != NetworkState.Pregame)
            {
                throw new InvalidOperationException("Game not in matching state. Make sure you call StartMatchmakingClient first.");
            }

            state = NetworkState.Connecting;

            //m_NextMatchJoinedCallback = onJoin;
            matchMaker.JoinMatch(networkId, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchJoined);
        }

        #endregion

        #region Networking Events

        //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        //{
        //    // Intentionally not calling base here - we want to control the spawning of prefabs
        //    Debug.Log("OnServerAddPlayer");

        //    NetworkPlayer newPlayer = Instantiate<NetworkPlayer>(m_NetworkPlayerPrefab);
        //    DontDestroyOnLoad(newPlayer);
        //    NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId);
        //}

        //public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        //{
        //    Debug.Log("OnServerRemovePlayer");
        //    base.OnServerRemovePlayer(conn, player);

        //    NetworkPlayer connectedPlayer = GetPlayerForConnection(conn);
        //    if (connectedPlayer != null)
        //    {
        //        Destroy(connectedPlayer);
        //        connectedPlayers.Remove(connectedPlayer);
        //    }
        //}

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchCreate(success, extendedInfo, matchInfo);
            Debug.Log("OnMatchCreate");

            if (success)
            {
                state = NetworkState.InLobby;
            }
            else
            {
                state = NetworkState.Inactive;
            }

            // Fire callback
            if (NextMatchCreatedCallback != null)
            {
                NextMatchCreatedCallback(success, matchInfo);
                NextMatchCreatedCallback = null;
            }

            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);

            StartHost(hostInfo);

            // Fire event
            if (matchCreated != null)
            {
                matchCreated(success, matchInfo);
            }
        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchJoined(success, extendedInfo, matchInfo);
            Debug.Log("OnMatchJoined");

            if (success)
            {
                state = NetworkState.InLobby;
            }
            else
            {
                state = NetworkState.Pregame;
            }

            // Fire callback
            //if (m_NextMatchJoinedCallback != null)
            //{
            //    m_NextMatchJoinedCallback(success, matchInfo);
            //    m_NextMatchJoinedCallback = null;
            //}

            // Fire event
            if (matchJoined != null)
            {
                matchJoined(success, matchInfo);
            }
        }

        #endregion
    }
}