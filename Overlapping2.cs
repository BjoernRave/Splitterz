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

    LayerMask Walls;


    void AssignValues()
    {

        Vector3 Extents = gameObject.GetComponent<SpriteRenderer>().bounds.extents;
        Vector3 gameobjectposition = gameObject.transform.position;
        if (string.Equals(gameObject.tag, "TopWalls", StringComparison.OrdinalIgnoreCase))
        {
            CheckBoxPos = new Vector2((gameobjectposition.x + Extents.x) - 0.775f, gameobjectposition.y);
        }
        else if (string.Equals(gameObject.tag, "LeftWalls", StringComparison.OrdinalIgnoreCase))
        {
            CheckBoxPos = new Vector2(gameobjectposition.x, (gameobjectposition.y + Extents.y) - 0.775f);
        }
        else if (string.Equals(gameObject.tag, "RightWalls", StringComparison.OrdinalIgnoreCase))
        {
            CheckBoxPos = new Vector2(gameobjectposition.x, (gameobjectposition.y - Extents.y) + 0.775f);
        }
        else if (string.Equals(gameObject.tag, "DownWalls", StringComparison.OrdinalIgnoreCase))
        {
            CheckBoxPos = new Vector2((gameobjectposition.x - Extents.x) + 0.775f, gameobjectposition.y);
        }
    }
    List<GameObject> PossibleStarters = new List<GameObject>();


    public static GameObject FirstWall;

    Collider2D[] OverlappingObjects;


    void Start()
    {
        Walls = GameObject.Find("Interface").GetComponent<swipegame>().PossibleWalls;
    }



    void Update()
    {



        if (ManageWalls.ActiveWalls.Count == ManageWalls.WallList.Count && ManageWalls.ActiveWalls.Count != 0)
        {
            ManageWalls.Assignable = false;
        }
        else if (Assigning && ManageWalls.Assignable && !String.Equals(gameObject.tag, "out", StringComparison.OrdinalIgnoreCase))
        {
            //Assign the Number, get next Wall to assign

            AssignValues();
            gameObject.layer = 0;
            ManageWalls.ActiveWalls.Add(gameObject);
            OverlappingObjects = Physics2D.OverlapBoxAll(CheckBoxPos, new Vector2(1.55f, 1.55f), 0, Walls);
            gameObject.layer = 11;

            Assigning = false;
            if (FirstWall == null)
            {
                FirstWall = gameObject;
            }


            foreach (Collider2D OverlappedObj in OverlappingObjects)
            {

                if (!string.Equals(OverlappedObj.tag, "out", StringComparison.OrdinalIgnoreCase) && (OverlappedObj.gameObject == FirstWall || ManageWalls.ActiveWalls.Contains(OverlappedObj.gameObject) == false))
                {
                    OverlappedObj.GetComponent<Overlapping2>().Assigning = true;

                    break;
                }

            }
        }


        //change layer, so only relevant wall get ontriggerhit




        // if Wall 0 is out, get new Wall to be 0
        if (Assigning && string.Equals(gameObject.name, "out", StringComparison.OrdinalIgnoreCase))
        {
            foreach (MainScript.Wallprops wall in MainScript.WallpropsList)
            {
                if (string.Equals(wall.Wall.tag, "TopWalls", StringComparison.OrdinalIgnoreCase))
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

            }
        }
    }
}
