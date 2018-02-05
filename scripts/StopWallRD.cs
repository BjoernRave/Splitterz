using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StopWallRD : MonoBehaviour
{
    // public static GameObject NewWallRD = new GameObject();
    public static Vector2 OldRDCorner1 = new Vector2();
    public static Vector2 OldRDCorner2 = new Vector2();
    Vector3 newPosition = new Vector3();
    Vector3 oldPosition = new Vector3();
    Vector2 OriginalSize = new Vector3();
    float OldWallLength = new float();
    float PercentOldWall = new float();
    public static Vector2 CollidedWallposition;
    public Vector2 corner1 = new Vector2();
    public Vector2 corner2 = new Vector2();
    public static Vector2 CollisionPos;
    public static bool IsCollidedRD = false;
    public static bool IsSplittableRD = true;
    Vector2 Corner2;
    public static bool Splitting = true;
    bool CanSplit = false;
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

    private void Update()
    {
        AssignValues(gameObject);
        OriginalSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;



        if (IsCollidedRD && StopWallLT.IsCollidedLT && Splitting && CanSplit && GameObject.Find("Player(Clone)") != null)
        {
            GameObject Player = GameObject.Find("Player(Clone)");

            CanSplit = false;
            print("splitRD");


            Vector2 oldScale = new Vector2();
            Vector2 newScale = new Vector2();

            if (gameObject.tag == "RightWalls")
            {
                CollisionPos.y = Player.transform.position.y;
                if (!StopWallLT.Splitting)
                {
                    CollisionPos.y = StopWallLT.CollisionPos.y;
                }

                newPosition = new Vector3(gameObject.transform.position.x, (corner2.y + CollisionPos.y) / 2, 0);
                oldPosition = new Vector3(gameObject.transform.position.x, (corner1.y + CollisionPos.y) / 2, 0);
                OldWallLength = corner1.y - CollisionPos.y;
                PercentOldWall = (100 / OriginalSize.y) * (OldWallLength / 100);
            }
            else
            {
                CollisionPos.x = Player.transform.position.x;
                if (!StopWallLT.Splitting)
                {
                    CollisionPos.x = StopWallLT.CollisionPos.x;
                }

                newPosition = new Vector3((corner2.x + CollisionPos.x) / 2, gameObject.transform.position.y, 0);
                oldPosition = new Vector3((corner1.x + CollisionPos.x) / 2, gameObject.transform.position.y, 0);
                OldWallLength = corner1.x - CollisionPos.x;
                PercentOldWall = (100 / OriginalSize.x) * (OldWallLength / 100);
            }
            GameObject NewWallRD = Instantiate(gameObject, newPosition, gameObject.transform.rotation);

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
            NewWallRD.layer = 11;
            NewWallRD.transform.localScale = newScale;
            gameObject.transform.position = oldPosition;
            gameObject.transform.localScale = oldScale;
            NewWallRD.name = gameObject.name + ".1";
            NewWallRD.GetComponent<Overlapping2>().Assigning = false;
            Splitting = false;

        }



    }




    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Wand" && gameObject.layer == 11 && IsSplittableRD)
        {
            GameObject Player = GameObject.Find("Player(Clone)");
            Player.GetComponent<RayCastRotation>().enabled = false;
            SpriteRenderer CollSPrite = collision.GetComponent<SpriteRenderer>();
            CollidedWallposition = gameObject.transform.position;

            if (RayCastRotation.LastAxis == Axis.Horizontal)
            {
                if ((corner1.y - collision.transform.position.y) < 3 && !StopWallLT.TooClosetoWall)
                {
                    CollisionPos = new Vector2(collision.transform.position.x + CollSPrite.bounds.extents.x, corner1.y - 0.757f);
                    Shortestpath3.NewWallY = CollisionPos.y;
                    Splitting = false;
                }
                else if ((collision.transform.position.y - corner2.y) < 3 && !StopWallLT.TooClosetoWall)
                {
                    CollisionPos = new Vector2(collision.transform.position.x + CollSPrite.bounds.extents.x, corner2.y + 0.757f);
                    Shortestpath3.NewWallY = CollisionPos.y;
                    Splitting = false;
                }
                else
                {
                    CollisionPos = Player.GetComponent<RayCastRotation>().WallRight.point;
                    Splitting = true;
                    CanSplit = true;
                }
            }
            else
            {
                if ((corner1.x - collision.transform.position.x) < 3 && !StopWallLT.TooClosetoWall)
                {
                    CollisionPos = new Vector2(corner1.x - 0.757f, collision.transform.position.y - (CollSPrite.bounds.extents.y));
                    Shortestpath3.NewWallX = CollisionPos.x;
                    Splitting = false;
                }
                else if ((collision.transform.position.x - corner2.x) < 3 && !StopWallLT.TooClosetoWall)
                {
                    CollisionPos = new Vector2(corner2.x + 0.757f, collision.transform.position.y - (CollSPrite.bounds.extents.y));
                    Shortestpath3.NewWallX = CollisionPos.x;
                    Splitting = false;
                }
                else
                {
                    CollisionPos = Player.GetComponent<RayCastRotation>().WallDown.point;
                    Splitting = true;
                    CanSplit = true;
                }
            }

            collision.GetComponent<Animator>().speed = 0;
            if (!Splitting)
            {
                print("RDnoSplit");
            }

            IsCollidedRD = true;
            IsSplittableRD = false;
        }
    }


}




