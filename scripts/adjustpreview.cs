using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class adjustpreview : MonoBehaviour
{


    Vector3 newSize;
    GameObject Player;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if (RayCastRotation.LastAxis == Axis.Horizontal)
        {
            newSize = new Vector3(0.77f, RayCastRotation.VerticalDistance + 2, 0);
            gameObject.transform.position = new Vector2((Player.GetComponent<RayCastRotation>().WallLeft.point.x + Player.GetComponent<RayCastRotation>().WallRight.point.x) / 2, Player.transform.position.y);

        }
        else
        {
            newSize = new Vector3(0.77f, RayCastRotation.HorizontalDistance + 2, 0);

            gameObject.transform.position = new Vector2(Player.transform.position.x, (Player.GetComponent<RayCastRotation>().WallUp.point.y + Player.GetComponent<RayCastRotation>().WallDown.point.y) / 2);
        }



        var scale = new Vector3(1, newSize.y / 7.4f, 0);

        gameObject.transform.localScale = scale;
    }
}
