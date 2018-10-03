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
        if (PlayerPrefs.GetInt("FinishedLevel") + 1 < Convert.ToInt32(gameObject.name))
        {
            LevelLoadable = false;
            Image[] Images = GetComponentsInChildren<Image>();
            foreach (Image Pic in Images)
            {
                Color NewColor = Pic.GetComponent<Image>().color;
                NewColor.a = 0.3f;
                Pic.GetComponent<Image>().color = NewColor;
            }
            Color NewColor2 = Color.white;
            NewColor2.a = 0.3f;
            GetComponentInChildren<Text>().color = NewColor2;

        }
        else
        {
            if (PlayerPrefs.GetInt(Convert.ToString(gameObject.name)) == 3)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                transform.GetChild(1).GetComponent<Image>().color = Color.yellow;
                transform.GetChild(2).GetComponent<Image>().color = Color.yellow;
            }
            else if (PlayerPrefs.GetInt(Convert.ToString(gameObject.name)) == 2)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                transform.GetChild(1).GetComponent<Image>().color = Color.yellow;
            }
            else if (PlayerPrefs.GetInt(Convert.ToString(gameObject.name)) == 1)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }

        }
    }
}
