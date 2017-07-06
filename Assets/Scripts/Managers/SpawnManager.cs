using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityRandom = UnityEngine.Random;

public class SpawnManager : NetworkBehaviour
{

    private GameObject cue;

    private GameObject cueBall;

    Quaternion ROTATION = Quaternion.Euler(0, 0, 1.76f);

    Vector3 ROTATION_AXIS = new Vector3(0, 1, 0);

    public Object cuePrefab;

    public float timer = 10;

    public bool isPlayerTurn = true;

    //public GameObject SpawnStick(Vector3 position, NetworkHash128 assetId)
    //{
    //    Debug.Log("spawn object " + position);
    //    cue.transform.position = position;
    //    cue.SetActive(true);
    //    return cue;
    //}

    //public void UnspawnStick(GameObject spawned)
    //{
    //    Debug.Log("unspawn object " + spawned.name);
    //    spawned.SetActive(false);
    //}

    void Awake()
    {
        
        cue = (GameObject)Instantiate(cuePrefab);
        cue.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        cueBall = GameObject.FindWithTag("CueBall");
        if (isLocalPlayer)
        {
            //Init();
            //NetworkHash128 cueAssetId = cue.GetComponent<NetworkIdentity>().assetId;
            //ClientScene.RegisterSpawnHandler(cueAssetId, SpawnStick, UnspawnStick);

            CmdSpawnPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isServer)
        {
            
            //NetworkHash128 creatureAssetId = NetworkHash128.Parse("e2656f");
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if(timer <= 0)
        {
            isPlayerTurn = !isPlayerTurn;
            CmdUnspawnPlayer();
        }

        if (isPlayerTurn)
        {
            timer -= Time.deltaTime;
            GetComponentInChildren<TextMesh>().text = Mathf.Round(timer).ToString();
        }
    }

    private void OnDestroy()
    {
        Destroy(cue);
    }

    [Command]
    public void CmdSpawnPlayer()
    {
        Vector3 position = new Vector3(cueBall.transform.position.x + 0.775f, 0.82f, cueBall.transform.position.z);
        cue.transform.SetPositionAndRotation(position, ROTATION);
        cue.transform.RotateAround(cueBall.transform.position, ROTATION_AXIS, UnityRandom.Range(0, 360));
        cue.SetActive(true);
        NetworkServer.Spawn(cue);
        //NetworkServer.SpawnWithClientAuthority(cue, connectionToClient);
    }

    [Command]
    public void CmdUnspawnPlayer()
    {
        NetworkServer.UnSpawn(cue);
        cue.SetActive(false);
    }
}
