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
        public float time = 100;

		[SyncVar(hook = "OnBallType")]
		BallType btype = BallType.NONE;

		SyncListInt potted = new SyncListInt ();

        public CueManager cueManager;

        [SyncVar]
        public bool ready = false;

        // Use this for initialization
        void Start()
        {
			cueManager.playerInput.OnPlayerInput += OnPlayerInput;
			cueManager.playerInput.OnFoulInput += OnFoulInput;
        }

        // Update is called once per frame
        [Server]
        void Update()
        {
            if (isTurn && NetworkManager.Instance.GameState != GameState.SHOOTING)
            {
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    NetworkManager.Instance.AlterTurns();
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

			potted.Callback = OnIntChanged;
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            cueManager.SetupLocalPlayer();
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
            time = 90;
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

		public void UpdateScore(int ball, BallType type)
		{
			if (potted.Count == 0) 
			{
				btype = type;
			}

			if(type == BallType.CUE){
				// foul
			}
			else if (btype == type) {
				potted.Add (ball);
				// player turn reset
			} else {
				// other player's pot
				// foul
			}
		}

        void OnPlayerInput(PlayerAction action, float amount)
        {
            //TODO: also check for localplayer while just shooting, as rotation has player authority
            //if (isLocalPlayer)
            if (action == PlayerAction.SHOOT)
            {
                CmdOnPlayerInput(action, amount);
            }
            else
            {
                cueManager.cueStick.RotateCueStick(amount);
            }
        }

		void OnFoulInput(PlayerAction action, Vector3 v)
		{
			if (action == PlayerAction.MOVECUEBALL)
			{
				CmdOnFoulInput(action, v);
			}
		}

		[Command]
		void CmdOnFoulInput(PlayerAction action, Vector3 v)
		{
			Debug.Log ("player "+v);
			cueManager.cueStick.MoveCueBall (v);
		}

        [Command]
        void CmdOnPlayerInput(PlayerAction action, float amount)
        {
            cueManager.playerInput.DisableControls();
            cueManager.cueStick.Shoot(amount);
        }

        public void UpdateTimeDisplay(float curtime)
        {
			GameObject timerText = GameObject.FindWithTag ("Timer");
			Text timer = timerText.GetComponent<Text> ();
			timer.text = /*(CPG.NetworkManager.Instance.ActivePlayer + 1) + ": " +*/ Mathf.Round(curtime).ToString();
        }

		public void OnBallType(BallType type)
		{
			// assign to other player
			// NetworkManager.Instance.AssignBallType();
		}

		void OnIntChanged(SyncListInt.Operation op, int index)
		{
			Debug.Log("List changed " + potted.ToString());
		}
    }
}
