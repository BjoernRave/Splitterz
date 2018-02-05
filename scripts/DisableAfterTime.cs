using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float Time;

    IEnumerator DisableObject(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            StartCoroutine(DisableObject(Time));
        }
    }
}
