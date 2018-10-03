using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FillColorLayer : MonoBehaviour
{

    public static int FillLayerNumber = -1;

    public GameObject WalltoAttach;
    public float pastTime;
    public float Addedtime;
    bool ableToDestroy = true;
    void Start()
    {
        pastTime = Time.realtimeSinceStartup;

        gameObject.GetComponent<SpriteRenderer>().color = swipegame._Character.GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().sortingOrder = FillLayerNumber;
        FillLayerNumber += -1;

    }


    private void Update()
    {
        Addedtime = Time.realtimeSinceStartup;

        if (Addedtime >= pastTime + 2.3f)
        {
            ableToDestroy = false;
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && ableToDestroy)
        {
            if (collision.gameObject.GetComponent<Enemy3Script>() != null)
            {
                if (collision.GetComponent<Enemy3Script>().Destroyable)
                {
                    Destroy(collision.gameObject);
                }
            }
            else
            {
                Destroy(collision.gameObject);
            }

        }
    }

}
