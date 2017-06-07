using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootCue : MonoBehaviour, IPointerClickHandler{
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 pos = eventData.pointerCurrentRaycast.worldPosition;
        int x = Mathf.FloorToInt(pos.x);
        int z = Mathf.FloorToInt(pos.z);
        Debug.Log(x + "" + z);
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
