using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageWalls : MonoBehaviour
{
    public LayerMask PossibleWalls;
    public static bool Assignable = true;
    public static List<GameObject> WallList = new List<GameObject>();
    public static bool AssignmentComplete = false;
    public static List<GameObject> ActiveWalls = new List<GameObject>();
    public static Vector2[] wayPointList;
    public static bool LevelLoad = false;
    public GameObject CurrentLevelText;
    public GameObject FadeIn;

    public static int WallListCount;
    public static int ActiveWallsCount;

    void Update()
    {


        if (WallList.Count == 0 && wayPointList == null)
        {
            WallList.AddRange(GameObject.FindGameObjectsWithTag("TopWalls"));
            WallList.AddRange(GameObject.FindGameObjectsWithTag("LeftWalls"));
            WallList.AddRange(GameObject.FindGameObjectsWithTag("RightWalls"));
            WallList.AddRange(GameObject.FindGameObjectsWithTag("DownWalls"));

        }

        if (WallList.Count == ActiveWalls.Count && WallList.Count != 0)
        {

            Assignable = false;
            Overlapping2.FirstWall = null;
            int WallnumbertoAssign = 0;



            foreach (GameObject wall in ActiveWalls)
            {
                wall.name = Convert.ToString(WallnumbertoAssign);
                WallnumbertoAssign += 1;
            }

            WallList.Clear();
            if (swipegame.WallModul != null)
            {
                AssignmentComplete = true;
            }
            else
            {

                if (wayPointList == null)
                {
                    wayPointList = new Vector2[ActiveWalls.Count];
                    for (int i = 0; i < ActiveWalls.Count; i++)
                    {
                        if (ActiveWalls[i].GetComponent<StopWallLT>() != null)
                        {
                            wayPointList[i] = ActiveWalls[i].GetComponent<StopWallLT>().corner1;
                        }
                        else
                        {
                            wayPointList[i] = ActiveWalls[i].GetComponent<StopWallRD>().corner1;
                        }
                    }
                }
                ActiveWalls.Clear();

            }
        }

        if (StopWallLT.IsCollidedLT && StopWallRD.IsCollidedRD && !StopWallLT.IsSplittableLT && !StopWallRD.IsSplittableRD || MainScript.NumberPossible)
        {

            if (swipegame._Character != null)
            {
                swipegame._Character.layer = 4;
            }
            WallList.AddRange(GameObject.FindGameObjectsWithTag("TopWalls"));
            WallList.AddRange(GameObject.FindGameObjectsWithTag("LeftWalls"));
            WallList.AddRange(GameObject.FindGameObjectsWithTag("RightWalls"));
            WallList.AddRange(GameObject.FindGameObjectsWithTag("DownWalls"));

            Assignable = true;
            StopWallLT.IsCollidedLT = false;
            StopWallRD.IsCollidedRD = false;
            MainScript.NumberPossible = false;
        }


    }


    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnLevelLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoad;
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode loadSceneMode)

    {
        ActiveWalls.Clear();
        WallList.Clear();
        MainScript.FillWholeField = false;
        CurrentLevelText.SetActive(true);
        FadeIn.SetActive(true);
        if (PersistentManagerScript.Instance.NormalMode)
        {
            CurrentLevelText.GetComponent<Text>().text = "Level " + SceneManager.GetActiveScene().name;
        }
        else
        {
            CurrentLevelText.GetComponent<Text>().text = "Stage " + Convert.ToString(EndlessScript.stages);
        }

        wayPointList = null;
        Assignable = true;
        Time.timeScale = 1;
        AssignmentComplete = false;
        MainScript.NewWallX = 0;
        MainScript.NewWallY = 0;
        StopWallLT.IsCollidedLT = false;
        StopWallRD.IsCollidedRD = false;
        StopWallLT.IsSplittableLT = true;
        StopWallRD.IsSplittableRD = true;

    }

}



