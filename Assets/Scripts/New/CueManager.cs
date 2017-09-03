using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPG
{
    public class CueManager : MonoBehaviour
    {
        public CueStick cueStick;

        public PlayerControls playerInput;

         bool isLocalPlayer = false;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetupLocalPlayer()
        {
            cueStick.GetComponent<MeshRenderer>().material.color = Color.blue;
            isLocalPlayer = true;
            playerInput.SetupLocalPlayer();
        }

        public void TurnStart()
        {
            cueStick.Spawn();
            if (isLocalPlayer)
            {
                playerInput.EnableControls(); 
            }
        }

        public void TurnEnd()
        {
            cueStick.Unspawn();
            if (isLocalPlayer)
            {
                playerInput.DisableControls(); 
            }
        }
    } 
}
