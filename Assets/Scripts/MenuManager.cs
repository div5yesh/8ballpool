using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CPG
{
    public enum NetworkState
    {
        INACTIVE,
        CONNECTING,
        CONNECTED
    }

    public class MenuManager : MonoBehaviour
    {

        public static MenuManager Instance;

        public NetworkState eNetState;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            eNetState = NetworkState.INACTIVE;
        }

        public void OnPlayClicked()
        {
            if (eNetState == NetworkState.INACTIVE)
            {
                eNetState = NetworkState.CONNECTING;
                CPG.NetworkManager.Instance.CreateOrJoin("poolgame", (success, matchInfo) =>
                {
                    eNetState = NetworkState.CONNECTED;
                    SceneManager.LoadScene("GameScene");
                }); 
            }
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }
    } 
}
