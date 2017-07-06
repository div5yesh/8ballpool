using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public CueManager[] Cues;
    //will contain array of cues,with spawn points having gizmos
    //and cam contrl to focus for 3d shoot
	// Use this for initialization
	void Start () {
        StartCoroutine(GameLoop());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStarting());
        yield return StartCoroutine(GamePlaying());
        //yield return StartCoroutine(GameEnding());

        // load game again or start gameloop again
    }

    IEnumerator GameStarting()
    {
        // reset and set prefabs for cues
        yield return 5;
    }

    IEnumerator GamePlaying()
    {
        //while (!isTableEmpty())
        {
            yield return null;
        }
    }
}
