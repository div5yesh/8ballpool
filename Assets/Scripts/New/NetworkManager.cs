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
            if (eGameState == GameState.STOP && players.Count == 2)
            {
                CheckPlayersReady();
            }

            if (eGameState == GameState.SHOOTING)
            {
                if (CheckForTurnEnd())
                {
                    eGameState = GameState.TURN;
                    AlterTurns();
                }
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
                eGameState = GameState.START;
                players[iActivePlayer].StartGame();
            }
        }

        public bool CheckForTurnEnd()
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
