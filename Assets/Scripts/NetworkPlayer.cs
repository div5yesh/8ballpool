using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{

    [SerializeField]
    public GameObject CueManagerPrefab;

    CueManager cueManager;
    private int playerId;

    [SerializeField]
    public GameObject TurnTimer;

    float TurnTime = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        UpdateTimer();

    }

    [Server]
    private void UpdateTimer()
    {
        if (NetworkManager.Instance.GetActivePlayer() == this)
        {
            if (TurnTime <= 0)
            {
                RpcTurnEnd();
            }
            else
            {
                TurnTime -= Time.deltaTime;
                GameObject.FindWithTag("Timer").GetComponent<TextMesh>().text = Mathf.Round(TurnTime).ToString();
            } 
        }
    }

    [Client]
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);

        base.OnStartClient();
        Debug.Log("Client Network Player start");

        NetworkManager.Instance.RegisterNetworkPlayer(this);
    }

    [Client]
    public void OnPlayersReady()
    {
        if (hasAuthority)
        {
            CmdClientReadyInScene();
        }
    }

    [ClientRpc]
    void RpcTurnEnd()
    {
        cueManager.OnTurnEnd();
        NetworkManager.Instance.TurnEnd();
    }

    [ClientRpc]
    public void RpcStartTurn()
    {
        TurnTime = 30;
        OnPlayersReady();
        cueManager.OnTurnStart();
    }

    [Command]
    private void CmdClientReadyInScene()
    {
        Debug.Log("CmdClientReadyInScene");
        GameObject cueManagerObject = Instantiate(CueManagerPrefab);
        NetworkServer.SpawnWithClientAuthority(cueManagerObject, connectionToClient);
        cueManager = cueManagerObject.GetComponent<CueManager>();
        cueManager.SetPlayerId(playerId);
    }

    [Command]
    public void CmdShoot()
    {

    }
}
