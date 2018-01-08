using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Facebook.Unity;

namespace CPG
{
//	public class Dialog
//	{
//		public string title;
//		public string text;
//		public List<Button> buttons = new List<Button> ();
//
//		public Dialog(string title)
//		{
//			this.title = title;
//		}
//
//		public void AddButtons(Button[] b)
//		{
//			buttons.AddRange (b);
//		}
//	}
//
//	public class DialogManager
//	{
//		public void AddDialog(Dialog d)
//		{
//			
//		}
//	}

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
