using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace CPG
{
    public enum GameState
    {
        START,
        STOP,
        PAUSE,
        WAITING,
        TURN,
        SHOOTING
    }

    public class NetworkManager : UnityEngine.Networking.NetworkManager
    {
        public event Action<bool, MatchInfo> matchCreated;

        public event Action<bool, MatchInfo> matchJoined;

        private Action<bool, MatchInfo> NextMatchCreatedCallback;

        List<NetworkPlayer> players;

        public static NetworkManager Instance;

        GameState eGameState = GameState.STOP;
        public GameState GameState
        {
            get
            {
                return eGameState;
            }
            set
            {
                eGameState = value;
                OnGameStateChange(eGameState);
            }
        }

        int iActivePlayer = 0;
        public int ActivePlayer
        {
            get
            {
                return iActivePlayer;
            }
        }

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameState == GameState.STOP && players.Count == 2)
            {
                CheckPlayersReady();
            }
        }

        void OnGameStateChange(GameState state)
        {
            if(state == GameState.SHOOTING)
            {
                StartCoroutine(WaitAndCheckForTableSleep());

            }
        }

        IEnumerator WaitAndCheckForTableSleep()
        {
            yield return new WaitForSeconds(2);
            if (!TableManager.Instance.IsTableSleeping())
            {
                StartCoroutine(WaitAndCheckForTableSleep());
            }
            else
            {
                GameState = GameState.TURN;
                AlterTurns();
            }
        }

        void CheckPlayersReady()
        {
            bool playersReady = true;
            foreach (var player in players)
            {
                playersReady &= player.ready;
            }

            if (playersReady)
            {
                GameState = GameState.START;
                players[iActivePlayer].StartGame();
            }
        }

        public void AlterTurns()
        {
            players[iActivePlayer].TurnEnd();
            iActivePlayer = (iActivePlayer + 1) % players.Count;
            players[iActivePlayer].TurnStart();
        }

        public void RegisterNetworkPlayer(NetworkPlayer player)
        {
            if (players.Count <= 2)
            {
                players.Add(player);
            }
        }

        public void DeregisterNetworkPlayer(NetworkPlayer player)
        {
            players.Remove(player);
        }

        public void CreateOrJoin(string gameName, Action<bool, MatchInfo> onCreate)
        {
            StartMatchMaker();
            NextMatchCreatedCallback = onCreate;
            matchMaker.ListMatches(0, 10, "poolgame", true, 0, 0, OnMatchList);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "GameScene")
            {
                NetworkServer.SpawnObjects();
            }
        }

        public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            Debug.Log("Matches:" + matches.Count);
            if (success && matches.Count > 0)
            {
                Debug.Log(matches[0].name+matches[0].networkId);
                id = matches[0].networkId.ToString();
                matchMaker.JoinMatch(matches[0].networkId, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchJoined);
            }
            else
            {
                CreateMatch("poolgame");
            }
        }

        public void CreateMatch(string matchName)
        {
            matchMaker.CreateMatch(matchName, 2, true, string.Empty, string.Empty, string.Empty, 0, 0, OnMatchCreate);
        }

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchCreate(success, extendedInfo, matchInfo);
            Debug.Log("OnMatchCreate"+success+matchInfo.networkId);
            id = matchInfo.networkId.ToString();

            // Fire callback
            if (NextMatchCreatedCallback != null)
            {
                NextMatchCreatedCallback(success, matchInfo);
                NextMatchCreatedCallback = null;
            }

            // Fire event
            if (matchCreated != null)
            {
                matchCreated(success, matchInfo);
            }
        }

        public string id = "";

        private void OnGUI()
        {
            GUILayout.Label(id.ToString());
        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            base.OnMatchJoined(success, extendedInfo, matchInfo);
            Debug.Log("OnMatchJoined"+matchInfo.networkId);

            // Fire callback
            if (NextMatchCreatedCallback != null)
            {
                NextMatchCreatedCallback(success, matchInfo);
                NextMatchCreatedCallback = null;
            }

            // Fire event
            if (matchJoined != null)
            {
                matchJoined(success, matchInfo);
            }
        }
    }
}
