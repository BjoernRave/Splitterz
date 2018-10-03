using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float Time;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, Time);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
