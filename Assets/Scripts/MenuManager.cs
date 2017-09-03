using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public void OnPlayClicked()
    {
        CPG.NetworkManager.Instance.CreateOrJoin("poolgame", (success, matchInfo) =>
        {
            SceneManager.LoadScene("GameScene");
        });
    }
}
