using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CPG
{
    public class GameManager : MonoBehaviour
    {

        public void OnBackClicked()
        {
            SceneManager.LoadScene("MainMenu");
            MenuManager.Instance.eNetState = NetworkState.INACTIVE;
        }
    } 
}
