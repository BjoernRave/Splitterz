using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPreview : MonoBehaviour
{
    float initializationTime;
    float timeSinceInitialization;

    void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;
    }
    void Update()
    {
        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timeSinceInitialization > 0.5)
        {
            if (collision.tag == "LeftWalls" || collision.tag == "RightWalls" || collision.tag == "TopWalls" || collision.tag == "DownWalls")
            {


                StopWallLT.TooClosetoWall = true;

            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timeSinceInitialization > 0.5)
        {
            if (collision.tag == "LeftWalls" || collision.tag == "RightWalls" || collision.tag == "TopWalls" || collision.tag == "DownWalls")
            {

                StopWallLT.TooClosetoWall = true;

            }
        }
    }

}
