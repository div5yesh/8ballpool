using Cash8BallPool.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace Cash8BallPool.Menu
{
    public class MenuManager : MonoBehaviour
    {
        NetworkManager NetManager;

        public string MatchName = "poolgame";

        protected virtual void Start()
        {
            NetManager = NetworkManager.Instance;
        }

        public void OnPlayClicked()
        {
            NetManager.StartMatchmakingGame(MatchName, (success, matchInfo) =>
            {
                SceneManager.LoadScene("GameScene");
            });
        }

        public void OnJoinClicked()
        {
            NetManager.StartMatchingmakingClient();
            NetManager.ListMatches(MatchName);
        }
    }
}
