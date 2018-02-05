using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpdateNumber : MonoBehaviour
{
    public bool LevelLoadable = true;
    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<Text>().text = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (PersistentManagerScript.Instance.FinishedLevel + 1 < Convert.ToInt32(gameObject.name))
        {
            LevelLoadable = false;
            Color NewColor = new Color();
            NewColor = Color.white;
            NewColor.a = 0.3f;
            GetComponent<Image>().color = NewColor;
            GetComponentInChildren<Text>().color = NewColor;

        }
    }
}
