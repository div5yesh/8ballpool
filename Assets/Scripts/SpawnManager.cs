using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnManager : NetworkBehaviour
{

    private GameObject cue;

    public Object cuePrefab;

    public float timer = 30;

    public bool isPlayerTurn = true;

    public GameObject SpawnStick(Vector3 position, NetworkHash128 assetId)
    {
        Debug.Log("spawn object " + position);
        cue.transform.position = position;
        cue.SetActive(true);
        return cue;
    }

    public void UnspawnStick(GameObject spawned)
    {
        Debug.Log("unspawn object " + spawned.name);
        spawned.SetActive(false);
    }

    void Init()
    {
        Vector3 cueBall = GameObject.FindWithTag("CueBall").transform.position;
        cue = (GameObject)Instantiate(cuePrefab, new Vector3(cueBall.x + 0.775f, 0.82f, cueBall.z), new Quaternion(0, 0, 1.76f, 180));
    }
    // Use this for initialization
    void Start()
    {
        //if (isServer)
        {
            Init();
            NetworkHash128 cueAssetId = cue.GetComponent<NetworkIdentity>().assetId;
            ClientScene.RegisterSpawnHandler(cueAssetId, SpawnStick, UnspawnStick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isServer)
        {
            timer -= Time.deltaTime;
            GetComponentInChildren<TextMesh>().text = Mathf.Round(timer).ToString();
            //NetworkHash128 creatureAssetId = NetworkHash128.Parse("e2656f");
            //NetworkHash128 cueAssetId = cue.GetComponent<NetworkIdentity>().assetId;
            //NetworkServer.Spawn(cue, cueAssetId); 
            CmdSpawnPlayer();
        }
    }

    [Command]
    public void CmdSpawnPlayer()
    {
        NetworkServer.SpawnWithClientAuthority(cue, connectionToClient);
    }

    [Command]
    public void CmdUnspawnPlayer()
    {
    }
}
