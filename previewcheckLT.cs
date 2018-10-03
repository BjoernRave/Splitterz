using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class previewcheckLT : MonoBehaviour
{


    public static bool previewtouchwallLT = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 && !previewtouchwallLT)
        {
            previewtouchwallLT = true;

        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 && previewtouchwallLT)
        {
            previewtouchwallLT = false;

        }
    }
}