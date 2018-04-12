using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Facebook.Unity;

namespace CPG
{
    public class GameManager : MonoBehaviour
    {

		public static GameManager Instance;

		public GameObject waiting;

		public GameObject Player;

		public GameObject Opponent;

		private void Awake(){
			if (!Instance) {
				Instance = this;
			}
		}

		private void Start()
		{
			if (FB.IsLoggedIn) {
				Player.GetComponentInChildren<RawImage>().texture = LobbyManager.Instance.profilePic;
			}
		}

        public void OnBackClicked()
        {
            SceneManager.LoadScene("LobbyScene");
            LobbyManager.Instance.eNetState = NetworkState.INACTIVE;
        }
    } 
}
