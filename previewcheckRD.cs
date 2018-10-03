using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class previewcheckRD : MonoBehaviour
{


    public static bool previewtouchwallRD = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 && !previewtouchwallRD)
        {
            previewtouchwallRD = true;

        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 && previewtouchwallRD)
        {
            previewtouchwallRD = false;

        }
    }
}