using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void YesExit()
    {
        Application.Quit();

    }
    public void NoExit()
    {
        Destroy(GameObject.Find("ExitMenu(Clone)"));
        GameObject.Find("Canvas").GetComponent<MenuScript>().Exitactive = false;
    }
}
