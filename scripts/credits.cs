using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour
{
    public int i;
    // Use this for initialization
    void Start()
    {
        i = 1;
    }

    // Update is called once per frame
    void Update()
    {
        i += 1;
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
