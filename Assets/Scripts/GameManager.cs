using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CPG
{
    public class GameManager : MonoBehaviour
    {

		private void Start()
		{
			GameObject.FindGameObjectWithTag ("ProfilePic").GetComponent<RawImage> ().texture = LobbyManager.Instance.profilePic;
		}

        public void OnBackClicked()
        {
            SceneManager.LoadScene("LobbyScene");
            LobbyManager.Instance.eNetState = NetworkState.INACTIVE;
        }
    } 
}
