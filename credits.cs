using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AppodealAds.Unity.Common;
using AppodealAds.Unity.Android;
using AppodealAds.Unity.Api;

public class credits : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
