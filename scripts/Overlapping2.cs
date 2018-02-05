using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Overlapping2 : MonoBehaviour
{
    Vector2 CheckBoxPos;
    public bool Assigning = false;
    public static bool AssigningComplete = false;
    public static int test = 0;
    void AssignValues()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (gameObject.tag == "TopWalls")
        {
            CheckBoxPos = new Vector2((gameObject.transform.position.x + sr.bounds.extents.x) - 0.775f, gameObject.transform.position.y);
        }
        else if (gameObject.tag == "LeftWalls")
        {
            CheckBoxPos = new Vector2(gameObject.transform.position.x, (gameObject.transform.position.y + sr.bounds.extents.y) - 0.775f);
        }
        else if (gameObject.tag == "RightWalls")
        {
            CheckBoxPos = new Vector2(gameObject.transform.position.x, (gameObject.transform.position.y - sr.bounds.extents.y) + 0.775f);
        }
        else if (gameObject.tag == "DownWalls")
        {
            CheckBoxPos = new Vector2((gameObject.transform.position.x - sr.bounds.extents.x) + 0.775f, gameObject.transform.position.y);
        }
    }
    List<GameObject> PossibleStarters = new List<GameObject>();


    public static GameObject FirstWall;





    void Update()
    {

        if (ManageWalls.ActiveWalls.Count == ManageWalls.WallList.Count && ManageWalls.ActiveWalls.Count != 0)
        {
            ManageWalls.Assignable = false;
        }
        else if (Assigning == true && ManageWalls.Assignable == true && gameObject.tag != "out")
        {
            //Assign the Number, get next Wall to assign
            Collider2D[] OverlappingObjects;
            AssignValues();
            gameObject.layer = 0;
            ManageWalls.ActiveWalls.Add(gameObject);
            OverlappingObjects = Physics2D.OverlapBoxAll(CheckBoxPos, new Vector2(1.55f, 1.55f), 0, GameObject.Find("Main Camera").GetComponent<ManageWalls>().PossibleWalls);


            Assigning = false;
            if (FirstWall == null)
            {
                FirstWall = gameObject;
            }


            foreach (Collider2D OverlappedObj in OverlappingObjects)
            {

                if (OverlappedObj.gameObject.tag != "out" && OverlappedObj.gameObject == FirstWall)
                {
                    OverlappedObj.GetComponent<Overlapping2>().Assigning = true;
                    break;
                }
                else if (OverlappedObj.gameObject.tag != "out" && ManageWalls.ActiveWalls.Contains(OverlappedObj.gameObject) == false)
                {
                    OverlappedObj.GetComponent<Overlapping2>().Assigning = true;
                    break;
                }

            }
        }


        //change tag, so only relevant wall get ontriggerhit
        if (gameObject.tag == "RightWalls" || gameObject.tag == "LeftWalls" && gameObject.name != "out")
        {
            if (RayCastRotation.LastAxis == Axis.Vertical)
            {
                gameObject.layer = 12;

            }
            else
            {
                gameObject.layer = 11;

            }
        }
        else if (gameObject.tag == "TopWalls" || gameObject.tag == "DownWalls" && gameObject.name != "out")
        {
            if (RayCastRotation.LastAxis == Axis.Horizontal)
            {
                gameObject.layer = 12;

            }
            else
            {
                gameObject.layer = 11;

            }
        }




        // if Wall 0 is out, get new Wall to be 0
        if (Assigning && gameObject.name == "out")
        {
            foreach (Shortestpath3.Wallprops wall in Shortestpath3.WallpropsList)
            {
                if (wall.Wall.tag == "TopWalls")
                {
                    PossibleStarters.Add(wall.Wall);
                    //wall.Wall.GetComponent<Overlapping2>().Assigning = true;
                    //break;
                }
            }
            if (PossibleStarters.Count != 0)
            {
                PossibleStarters.Sort((x, y) => x.transform.position.x.CompareTo(y.transform.position.x));
                PossibleStarters[0].GetComponent<Overlapping2>().Assigning = true;
                Assigning = false;
                // print("lol");
            }

        }


    }
}
