using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using UnityEngine.UI;

namespace CPG
{
	public enum NetworkState
	{
		INACTIVE,
		CONNECTING,
		CONNECTED
	}

	public class LobbyManager : MonoBehaviour 
	{
		public NetworkState eNetState;

		public static LobbyManager Instance;

		public Texture2D profilePic;

		private void Awake()
		{
			if (!Instance)
			{
				Instance = this;
			}
		}

		void Start () 
		{
			eNetState = NetworkState.INACTIVE;
			GetUserData ();
		}

		private void GetUserData()
		{
			FB.API("/me", HttpMethod.GET, OnFBResponse);
			FB.API("/me/picture", HttpMethod.GET, OnProfilePhotoResponse);
		}

		private void OnFBResponse(IResult result)
		{
			if (result != null) 
			{
				if (!string.IsNullOrEmpty(result.Error))
				{
					Debug.Log ("Error" + result.Error);
				}
				else if (result.Cancelled)
				{
					Debug.Log ("Cancelled" + result.RawResult);
				}
				else if (!string.IsNullOrEmpty(result.RawResult))
				{
					GameObject.FindGameObjectWithTag ("Username").GetComponent<Text> ().text = "Hello, " + result.ResultDictionary ["name"].ToString() + "!";
					Debug.Log ("Result" + result.RawResult);
				}
				else
				{
					Debug.Log ("Empty response");
				}
			}
		}

		private void OnProfilePhotoResponse(IGraphResult result)
		{
			if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
			{
				profilePic = result.Texture;
			}
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

		public void OnLogoutClicked()
		{
			//if (FB.IsLoggedIn) 
			{
				SceneManager.LoadScene("LoginScene");
				FB.LogOut ();
			}
		}
	}
}