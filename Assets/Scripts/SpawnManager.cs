using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnManager : MonoBehaviour {

    private GameObject cue;

    public Object stickPrefab;

    public GameObject SpawnStick(Vector3 position, NetworkHash128 assetId)
    {
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
        cue = (GameObject)Instantiate(stickPrefab, Vector3.zero, Quaternion.identity);
    }
    // Use this for initialization
    void Start () {
        Init();
        NetworkHash128 stickAssetId = GetComponent<NetworkIdentity>().assetId;
        ClientScene.RegisterSpawnHandler(stickAssetId, SpawnStick, UnspawnStick);
	}
	
	// Update is called once per frame
	void Update () {
        //NetworkHash128 creatureAssetId = NetworkHash128.Parse("e2656f");
        //NetworkServer.Spawn(gameObject, creatureAssetId);
    }
}
