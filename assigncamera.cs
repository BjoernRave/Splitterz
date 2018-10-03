using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assigncamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		   GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
