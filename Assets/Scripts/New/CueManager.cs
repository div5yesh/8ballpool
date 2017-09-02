using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CPG
{
    public class CueManager : MonoBehaviour
    {
        public CueStick cueStick;

        public PlayerControls playerInput;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TurnStart()
        {
            cueStick.Spawn();
            playerInput.EnableControls();
        }

        

        public void TurnEnd()
        {
            cueStick.Unspawn();
            playerInput.DisableControls();
        }
    } 
}
