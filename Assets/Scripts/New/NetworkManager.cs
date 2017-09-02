using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        List<NetworkPlayer> players;

        public static NetworkManager Instance;

        public Rigidbody[] balls;

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
        }

        // Update is called once per frame
        void Update()
        {
            if (GameState == GameState.STOP && players.Count > 0)
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
            if (!IsTableSleeping())
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

        public bool IsTableSleeping()
        {
            bool tableSleeping = true;
            foreach (var ball in balls)
            {
                tableSleeping = tableSleeping && ball.IsSleeping();
            }

            return tableSleeping;
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
    }
}
