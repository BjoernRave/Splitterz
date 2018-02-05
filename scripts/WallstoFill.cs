using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallstoFill : MonoBehaviour
{
    public bool StopAnimation = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (StopAnimation)
        {
            GetComponent<Animator>().speed = 0;
        }
        if (ManageUI.WallsGoFaster && GetComponent<Animator>().speed != 0)
        {
            GetComponent<Animator>().speed = 1.5f;

        }
        if (ManageUI.WallsGoSlower && GetComponent<Animator>().speed != 0)
        {
            GetComponent<Animator>().speed = 0.5f;

        }

        GetComponent<SpriteRenderer>().color = Color.black;
        //GameObject.Find("Player(Clone)").GetComponent<SpriteRenderer>().color;
    }
}
