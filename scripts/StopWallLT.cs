using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StopWallLT : MonoBehaviour
{
    public static Vector2 CollidedWallposition;
    public static Vector2 OldLTCorner1 = new Vector2();
    public static Vector2 OldLTCorner2 = new Vector2();
    public Vector2 corner1 = new Vector2();
    public Vector2 corner2 = new Vector2();
    public static Vector2 CollisionPos;
    public static bool IsCollidedLT = false;
    Vector3 newPosition = new Vector3();
    Vector3 oldPosition = new Vector3();
    Vector2 OriginalSize = new Vector3();
    float OldWallLength = new float();
    float PercentOldWall = new float();
    Vector2 Corner2;
    public static bool IsSplittableLT = true;
    void AssignValues(GameObject wall)
    {
        SpriteRenderer sr = wall.GetComponent<SpriteRenderer>();

        if (wall.tag == "TopWalls")
        {
            Vector2 Corner1 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            Corner2 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
        else if (wall.tag == "LeftWalls")
        {
            Vector2 Corner1 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            Corner2 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
        else if (wall.tag == "RightWalls")
        {
            Vector2 Corner1 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
            Corner2 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y - sr.bounds.extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
        else if (wall.tag == "DownWalls")
        {
            Vector2 Corner1 = new Vector2(wall.transform.position.x + sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
            Corner2 = new Vector2(wall.transform.position.x - sr.bounds.extents.x, wall.transform.position.y + sr.bounds.extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
    }
    public static bool Splitting = true;
    public static bool TooClosetoWall = false;
    bool CanSplit = false;




    private void Update()
    {
        AssignValues(gameObject);
        OriginalSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;


        if (IsCollidedLT && StopWallRD.IsCollidedRD && Splitting && CanSplit && GameObject.Find("Player(Clone)") != null)
        {
            GameObject Player = GameObject.Find("Player(Clone)");

            CanSplit = false;
            print("spliLT");


            Vector2 oldScale = new Vector2();
            Vector2 newScale = new Vector2();

            if (RayCastRotation.LastAxis == Axis.Horizontal)
            {
                CollisionPos.y = Player.transform.position.y;
                if (!StopWallRD.Splitting)
                {
                    CollisionPos.y = StopWallRD.CollisionPos.y;
                }


                newPosition = new Vector3(gameObject.transform.position.x, (corner2.y + CollisionPos.y) / 2, -1);
                oldPosition = new Vector3(gameObject.transform.position.x, (corner1.y + CollisionPos.y) / 2, -1);
                OldWallLength = CollisionPos.y - corner1.y;
                PercentOldWall = (100 / OriginalSize.y) * (OldWallLength / 100);
            }
            else
            {
                CollisionPos.x = Player.transform.position.x;
                if (!StopWallRD.Splitting)
                {
                    CollisionPos.x = StopWallRD.CollisionPos.x;
                }


                newPosition = new Vector3((corner2.x + CollisionPos.x) / 2, gameObject.transform.position.y, -1);
                oldPosition = new Vector3((corner1.x + CollisionPos.x) / 2, gameObject.transform.position.y, -1);
                OldWallLength = CollisionPos.x - corner1.x;
                PercentOldWall = (100 / OriginalSize.x) * (OldWallLength / 100);
            }
            GameObject NewWallLT = Instantiate(gameObject, newPosition, gameObject.transform.rotation);
            if (gameObject.layer == 11)
            {
                oldScale = new Vector2(2, gameObject.transform.localScale.y * PercentOldWall);
                newScale = new Vector2(2, gameObject.transform.localScale.y * (1 - PercentOldWall));
            }
            else
            {
                oldScale = new Vector2(2, gameObject.transform.localScale.y * PercentOldWall);
                newScale = new Vector2(2, gameObject.transform.localScale.y * (1 - PercentOldWall));
            }
            gameObject.layer = 11;
            NewWallLT.layer = 11;
            NewWallLT.transform.localScale = newScale;
            gameObject.transform.position = oldPosition;
            gameObject.transform.localScale = oldScale;
            NewWallLT.name = gameObject.name + ".1";
            NewWallLT.GetComponent<Overlapping2>().Assigning = false;
            Splitting = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wand" && gameObject.layer == 11 && IsSplittableLT)
        {
            GameObject Player = GameObject.Find("Player(Clone)");
            print("collidedLT");
            Player.GetComponent<RayCastRotation>().enabled = false;
            SpriteRenderer CollSPrite = collision.GetComponent<SpriteRenderer>();
            CollidedWallposition = gameObject.transform.position;

            if (RayCastRotation.LastAxis == Axis.Horizontal)
            {
                if ((corner2.y - collision.transform.position.y) < 3.817 && !TooClosetoWall)
                {
                    CollisionPos = new Vector2(collision.transform.position.x - CollSPrite.bounds.extents.x, corner2.y - 0.757f);
                    Shortestpath3.NewWallY = CollisionPos.y;
                    Splitting = false;
                }
                else if ((collision.transform.position.y - corner1.y) < 3.817 && !TooClosetoWall)
                {
                    CollisionPos = new Vector2(collision.transform.position.x - CollSPrite.bounds.extents.x, corner1.y + 0.757f);
                    Shortestpath3.NewWallY = CollisionPos.y;
                    Splitting = false;
                }
                else
                {
                    CollisionPos = Player.GetComponent<RayCastRotation>().WallLeft.point;
                    Splitting = true;
                    CanSplit = true;
                }
            }
            else
            {
                if ((corner2.x - collision.transform.position.x) < 3.817 && !TooClosetoWall)
                {
                    CollisionPos = new Vector2(corner2.x - 0.757f, collision.transform.position.y + (CollSPrite.bounds.extents.y));
                    Shortestpath3.NewWallX = CollisionPos.x;
                    Splitting = false;
                }
                else if ((collision.transform.position.x - corner1.x) < 3.817 && !TooClosetoWall)
                {
                    CollisionPos = new Vector2(corner1.x + 0.757f, collision.transform.position.y + (CollSPrite.bounds.extents.y));
                    Shortestpath3.NewWallX = CollisionPos.x;
                    Splitting = false;
                }
                else
                {
                    CollisionPos = Player.GetComponent<RayCastRotation>().WallUp.point;
                    Splitting = true;
                    CanSplit = true;
                }
            }
            if (!Splitting)
            {
                print("LTnoSplit");
            }
            collision.GetComponent<Animator>().speed = 0;

            IsCollidedLT = true;
            IsSplittableLT = false;
        }
    }
}






