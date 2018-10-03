using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallstoFill : MonoBehaviour
{
    public bool StopAnimation = false;
    public bool CanCollide = true;

    public static bool LeftCollided = false;
    public static bool RightCollided = false;

    public static GameObject LTWall;
    public static GameObject RDWall;
    public static GameObject CollisionObjLT;
    public static GameObject CollisionObjRD;

    Animator FillWalls;
    // Use this for initialization
    void Start()
    {
        FillWalls = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftCollided && RightCollided)
        {
            swipegame._Character.GetComponent<PlayerScript>().enabled = false;
            Destroy(swipegame._preview.GetComponent<DestroyPlayer>());

            LTWall.GetComponent<StopWallLT>().StartSplit(CollisionObjLT);
            RDWall.GetComponent<StopWallRD>().StartSplit(CollisionObjRD);
            LeftCollided = false;
            RightCollided = false;

        }
        if (StopAnimation)
        {
            FillWalls.speed = 0;
        }
        if (ManageUI.WallsGoFaster && FillWalls.speed != 0)
        {
            FillWalls.speed = 1.5f;

        }
        if (ManageUI.WallsGoSlower && FillWalls.speed != 0)
        {
            FillWalls.speed = 0.5f;

        }

        GetComponent<SpriteRenderer>().color = Color.black;
        //GameObject.Find("Player(Clone)").GetComponent<SpriteRenderer>().color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if ((other.gameObject == swipegame.TopWall || other.gameObject == swipegame.RightWall || other.gameObject == swipegame.Leftwall || other.gameObject == swipegame.DownWall) && CanCollide && other.gameObject.layer == 11)
        {

            CanCollide = false;
            GetComponentInParent<Animator>().speed = 0;
            if (other.GetComponent<StopWallLT>() != null)
            {
                LTWall = other.gameObject;
                CollisionObjLT = gameObject;
                LeftCollided = true;
            }
            else if (other.GetComponent<StopWallRD>() != null)
            {
                RDWall = other.gameObject;
                CollisionObjRD = gameObject;
                RightCollided = true;
            }
        }
    }
}
