using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StopWallLT : MonoBehaviour
{
    public static Vector3 objectposition;
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
    Vector3 gameobjectposition;


    void AssignValues(GameObject wall)
    {

        Vector3 Extents = wall.GetComponent<SpriteRenderer>().bounds.extents;
        Vector3 wallposition = wall.transform.position;

        if (string.Equals(wall.tag, "TopWalls", StringComparison.OrdinalIgnoreCase))
        {
            Vector2 Corner1 = new Vector2(wallposition.x - Extents.x, wallposition.y - Extents.y);
            Corner2 = new Vector2(wallposition.x + Extents.x, wallposition.y - Extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
        else if (string.Equals(wall.tag, "LeftWalls", StringComparison.OrdinalIgnoreCase))
        {
            Vector2 Corner1 = new Vector2(wallposition.x + Extents.x, wallposition.y - Extents.y);
            Corner2 = new Vector2(wallposition.x + Extents.x, wallposition.y + Extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
        else if (string.Equals(wall.tag, "RightWalls", StringComparison.OrdinalIgnoreCase))
        {
            Vector2 Corner1 = new Vector2(wallposition.x - Extents.x, wallposition.y + Extents.y);
            Corner2 = new Vector2(wallposition.x - Extents.x, wallposition.y - Extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
        else if (string.Equals(wall.tag, "DownWalls", StringComparison.OrdinalIgnoreCase))
        {
            Vector2 Corner1 = new Vector2(wallposition.x + Extents.x, wallposition.y + Extents.y);
            Corner2 = new Vector2(wallposition.x - Extents.x, wallposition.y + Extents.y);
            corner1 = Corner1;
            corner2 = Corner2;
        }
    }
    public static bool Splitting = true;
    public static bool TooClosetoWall = false;
    bool CanSplit = false;

    void Start()
    {
        gameobjectposition = gameObject.transform.position;

        AssignValues(gameObject);
    }


    private void Update()
    {
        if (IsCollidedLT && StopWallRD.IsCollidedRD && Splitting && CanSplit && swipegame._Character != null)
        {
            AssignValues(gameObject);
            OriginalSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
            GameObject Player = swipegame._Character;

            Vector3 LocalScaleObj = gameObject.transform.localScale;
            CanSplit = false;



            Vector2 oldScale = new Vector2();
            Vector2 newScale = new Vector2();

            if (swipegame.Horizontal)
            {
                CollisionPos.y = Player.transform.position.y;
                if (!StopWallRD.Splitting)
                {
                    CollisionPos.y = StopWallRD.CollisionPos.y;
                }
                newPosition = new Vector3(gameobjectposition.x, (corner2.y + CollisionPos.y) / 2, 0);
                oldPosition = new Vector3(gameobjectposition.x, (corner1.y + CollisionPos.y) / 2, 0);
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


                newPosition = new Vector3((corner2.x + CollisionPos.x) / 2, gameobjectposition.y, 0);
                oldPosition = new Vector3((corner1.x + CollisionPos.x) / 2, gameobjectposition.y, 0);
                OldWallLength = CollisionPos.x - corner1.x;
                PercentOldWall = (100 / OriginalSize.x) * (OldWallLength / 100);
            }
            GameObject NewWallLT = Instantiate(gameObject, newPosition, gameObject.transform.rotation);

            oldScale = new Vector2(2, LocalScaleObj.y * PercentOldWall);
            newScale = new Vector2(2, LocalScaleObj.y * (1 - PercentOldWall));


            gameObject.layer = 11;
            NewWallLT.layer = 11;
            NewWallLT.transform.localScale = newScale;
            gameObject.transform.position = oldPosition;
            gameObject.transform.localScale = oldScale;
            NewWallLT.name = gameObject.name + ".1";
            NewWallLT.GetComponent<Overlapping2>().Assigning = false;
            IsSplittableLT = false;
        }
    }


    public void StartSplit(GameObject collision)
    {
        objectposition = gameObject.transform.position;
        PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.Collide);

        AssignValues(gameObject);



        Vector3 Extents = collision.GetComponent<SpriteRenderer>().bounds.extents;

        Vector3 Collisionposition = collision.transform.position;

        if (swipegame.Horizontal)
        {
            if ((corner2.y - Collisionposition.y) < 2.5 && !TooClosetoWall)
            {
                CollisionPos = new Vector2(Collisionposition.x - Extents.x, corner2.y - 0.757f);
                MainScript.NewWallY = CollisionPos.y;
                Splitting = false;
                IsSplittableLT = false;
            }
            else if ((Collisionposition.y - corner1.y) < 2.5 && !TooClosetoWall)
            {
                CollisionPos = new Vector2(Collisionposition.x - Extents.x, corner1.y + 0.757f);
                MainScript.NewWallY = CollisionPos.y;
                Splitting = false;
                IsSplittableLT = false;
            }
            else
            {
                CollisionPos = new Vector2(Collisionposition.x - Extents.x - 0.5f, Collisionposition.y);
                Splitting = true;
                CanSplit = true;
            }
        }
        else
        {
            if ((corner2.x - Collisionposition.x) < 2.5 && !TooClosetoWall)
            {
                CollisionPos = new Vector2(corner2.x - 0.757f, Collisionposition.y + Extents.y);
                MainScript.NewWallX = CollisionPos.x;
                Splitting = false;
                IsSplittableLT = false;
            }
            else if ((Collisionposition.x - corner1.x) < 2.5 && !TooClosetoWall)
            {
                CollisionPos = new Vector2(corner1.x + 0.757f, Collisionposition.y + Extents.y);
                MainScript.NewWallX = CollisionPos.x;
                Splitting = false;
                IsSplittableLT = false;
            }
            else
            {
                CollisionPos = new Vector2(Collisionposition.x, Collisionposition.y + Extents.y + 0.5f);
                Splitting = true;
                CanSplit = true;
            }
        }

        collision.GetComponent<Animator>().speed = 0;

        IsCollidedLT = true;

    }



}






