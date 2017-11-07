using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;

namespace CPG
{
	public class User
	{
		public long userId;
		public string username;
	}

    public class LoginManager : MonoBehaviour
    {

        public static LoginManager Instance;

		public static User UserData;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }

        private void Start()
        {
			InitFBLogin();
			InitGoogleLogin();
        }

		private void InitGoogleLogin()
		{
			
		}

		#region FB Login

		private void InitFBLogin()
		{
			FB.Init(OnInitComplete, OnHideUnity);
			Debug.Log ("FB init" + FB.AppId);
			GoToLobby ();
		}

		public void OnFBLoginClicked()
		{
			if (FB.IsInitialized) 
			{
				FB.LogInWithReadPermissions (new List<string> () { "public_profile", "email" }, OnFBResponse);
			}
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
					GoToLobby ();
					Debug.Log ("Result" + result.RawResult);
				}
				else
				{
					Debug.Log ("Empty response");
				}
			}
		}

		private void OnInitComplete()
        {
			Debug.Log("OnInitCompleteCalled IsLoggedIn IsInitialized " + FB.IsLoggedIn + FB.IsInitialized);
            if (AccessToken.CurrentAccessToken != null)
            {
                Debug.Log(AccessToken.CurrentAccessToken.ToString());
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            Debug.Log("Is game shown: " + isGameShown);
        }

		#endregion

		public void OnGoogleLoginClicked()
		{

		}

		private void GoToLobby()
        {
			if (FB.IsLoggedIn) 
			{
				SceneManager.LoadScene("LobbyScene");
			}
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }
    } 
}
