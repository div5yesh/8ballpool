using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace CPG
{
    public class NetworkPlayer : NetworkBehaviour
    {

        [SyncVar(hook = "OnTurnChange")]
        public bool isTurn = false;

        [SyncVar(hook = "UpdateTimeDisplay")]
        public float time = 15;

        public CueManager cueManager;

        [SyncVar]
        public bool ready = false;

        // Use this for initialization
        void Start()
        {
            cueManager.playerInput.OnPlayerInput += CmdOnPlayerInput;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTurn && NetworkManager.Instance.GameState != GameState.SHOOTING)
            {
                time -= Time.deltaTime;
                if(time <= 0)
                {
                    TurnEnd();
                }
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(this);

            base.OnStartClient();
            Debug.Log("Client Network Player start");
            StartPlayer();

            CPG.NetworkManager.Instance.RegisterNetworkPlayer(this);
        }

        [Server]
        public void StartPlayer()
        {
            ready = true;
        }

        public void StartGame()
        {
            TurnStart();
        }

        [Server]
        public void TurnStart()
        {
            isTurn = true;
            time = 15;
            RpcTurnStart();
        }

        [ClientRpc]
        void RpcTurnStart()
        {
            cueManager.TurnStart();
        }

        [Server]
        public void TurnEnd()
        {
            isTurn = false;
            RpcTurnEnd();
            NetworkManager.Instance.AlterTurns();
        }

        [ClientRpc]
        void RpcTurnEnd()
        {
            cueManager.TurnEnd();
        }

        public override void OnNetworkDestroy()
        {
            base.OnNetworkDestroy();
            CPG.NetworkManager.Instance.DeregisterNetworkPlayer(this);
        }

        public void OnTurnChange(bool turn)
        {
            if (isLocalPlayer)
            {
                //play turn sound
                //playerControlPanel.GetComponentInChildren<Button>().enabled = turn; 
            }
        }

        [Command]
        void CmdOnPlayerInput(PlayerAction action, float amount)
        {
            if (action == PlayerAction.ROTATE)
            {
                cueManager.cueStick.RotateCueStick(amount);
            }
            else
            {
                cueManager.cueStick.Shoot(amount);
            }
        }

        public void UpdateTimeDisplay(float curtime)
        {
            GameObject.FindWithTag("Timer").GetComponent<TextMesh>().text = "Player " +
                (CPG.NetworkManager.Instance.ActivePlayer + 1) + ": " + Mathf.Round(curtime).ToString();
        }
    } 
}
