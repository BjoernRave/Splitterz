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

    public Vector2 corner1 = new Vector2();
    public Vector2 corner2 = new Vector2();
    public static Vector2 CollisionPos;
    public static bool IsCollidedRD = false;
    public static bool IsSplittableRD = true;
    Vector2 Corner2;
    public static bool Splitting = true;
    bool CanSplit = false;
    public static Vector3 objectposition;
    Vector3 GameObjectposition;

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


    void Start()
    {
        GameObjectposition = gameObject.transform.position;
        AssignValues(gameObject);

    }

    private void Update()
    {


        if (IsCollidedRD && StopWallLT.IsCollidedLT && Splitting && CanSplit && swipegame._Character != null)
        {
            AssignValues(gameObject);
            OriginalSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
            GameObject Player = swipegame._Character;


            CanSplit = false;



            Vector2 oldScale = new Vector2();
            Vector2 newScale = new Vector2();

            if (gameObject.tag == "RightWalls")
            {
                CollisionPos.y = Player.transform.position.y;
                if (!StopWallLT.Splitting)
                {
                    CollisionPos.y = StopWallLT.CollisionPos.y;
                }

                newPosition = new Vector3(GameObjectposition.x, (corner2.y + CollisionPos.y) / 2, 0);
                oldPosition = new Vector3(GameObjectposition.x, (corner1.y + CollisionPos.y) / 2, 0);
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

                newPosition = new Vector3((corner2.x + CollisionPos.x) / 2, GameObjectposition.y, 0);
                oldPosition = new Vector3((corner1.x + CollisionPos.x) / 2, GameObjectposition.y, 0);
                OldWallLength = corner1.x - CollisionPos.x;
                PercentOldWall = (100 / OriginalSize.x) * (OldWallLength / 100);
            }
            GameObject NewWallRD = Instantiate(gameObject, newPosition, gameObject.transform.rotation);


            oldScale = new Vector2(2, gameObject.transform.localScale.y * PercentOldWall);
            newScale = new Vector2(2, gameObject.transform.localScale.y * (1 - PercentOldWall));


            gameObject.layer = 11;
            NewWallRD.layer = 11;
            NewWallRD.transform.localScale = newScale;
            gameObject.transform.position = oldPosition;
            gameObject.transform.localScale = oldScale;
            NewWallRD.name = gameObject.name + ".1";
            NewWallRD.GetComponent<Overlapping2>().Assigning = false;
            IsSplittableRD = false;

        }



    }
    public void StartSplit(GameObject collision)
    {

        PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.Collide);
        AssignValues(gameObject);
        Vector3 Extents = collision.GetComponent<SpriteRenderer>().bounds.extents;
        Vector3 collisionposition = collision.transform.position;
        objectposition = gameObject.transform.position;

        if (swipegame.Horizontal)
        {
            if ((corner1.y - collisionposition.y) < 2.5 && !StopWallLT.TooClosetoWall)
            {
                CollisionPos = new Vector2(collisionposition.x + Extents.x, corner1.y - 0.757f);
                MainScript.NewWallY = CollisionPos.y;
                Splitting = false;
                IsSplittableRD = false;
            }
            else if ((collisionposition.y - corner2.y) < 2.5 && !StopWallLT.TooClosetoWall)
            {
                CollisionPos = new Vector2(collisionposition.x + Extents.x, corner2.y + 0.757f);
                MainScript.NewWallY = CollisionPos.y;
                Splitting = false;
                IsSplittableRD = false;
            }
            else
            {
                CollisionPos = new Vector2(collisionposition.x + Extents.x + 0.5f, collisionposition.y);
                Splitting = true;
                CanSplit = true;
            }
        }
        else
        {
            if ((corner1.x - collisionposition.x) < 2.5 && !StopWallLT.TooClosetoWall)
            {
                CollisionPos = new Vector2(corner1.x - 0.757f, collisionposition.y - (Extents.y));
                MainScript.NewWallX = CollisionPos.x;
                Splitting = false;
                IsSplittableRD = false;
            }
            else if ((collisionposition.x - corner2.x) < 2.5 && !StopWallLT.TooClosetoWall)
            {
                CollisionPos = new Vector2(corner2.x + 0.757f, collisionposition.y - (Extents.y));
                MainScript.NewWallX = CollisionPos.x;
                Splitting = false;
                IsSplittableRD = false;
            }
            else
            {
                CollisionPos = new Vector2(collisionposition.x, collisionposition.y - Extents.y - 0.5f);
                Splitting = true;
                CanSplit = true;
            }
        }

        collision.GetComponent<Animator>().speed = 0;

        IsCollidedRD = true;

    }







}




