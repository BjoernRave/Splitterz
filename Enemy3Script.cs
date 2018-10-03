using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy3Script : MonoBehaviour
{

    // put the points from unity interface
    public Vector2[] wayPointList;
    public bool SceneChanging = true;
    public int currentWayPoint = 0;
    Vector3 targetWayPoint;
    public GameObject Particles;
    ParticleSystem ps;
    Collider2D OverlappingObject;
    public bool Destroyable = false;
    public float speed = 5f;
    public GameObject BonusPoints;
    public LayerMask WallsOut;
    // Use this for initialization
    void Start()
    {
        ps = Particles.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        OverlappingObject = Physics2D.OverlapBox(gameObject.transform.position, new Vector2(0.5f, 0.5f), 0, WallsOut);


        if (OverlappingObject != null)
        {
            Destroyable = true;
        }
        else
        {
            Destroyable = false;
        }

        if (ManageWalls.wayPointList != null)
        {
            wayPointList = ManageWalls.wayPointList;


            // check if we have somewere to walk
            if (currentWayPoint < wayPointList.Length)
            {
                if (targetWayPoint == new Vector3(0, 0, 0))
                    targetWayPoint = wayPointList[currentWayPoint];
                walk();
            }
        }
    }

    void walk()
    {
        // rotate towards the target
        // transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);

        if (transform.position == targetWayPoint)
        {
            if (currentWayPoint >= wayPointList.Length - 1)
            {
                currentWayPoint = -1;
            }
            currentWayPoint++;
            targetWayPoint = wayPointList[currentWayPoint];
        }
    }
    void OnApplicationQuit()
    {
        SceneChanging = false;
    }

    private void OnDestroy()
    {
        if (SceneChanging)
        {
            PersistentManagerScript.Instance.musicsource.PlayOneShot(PersistentManagerScript.Instance.EnemieKilled);
            var col = ps.colorOverLifetime;
            col.enabled = true;
            PlayerPrefs.SetInt("EnemiesKilled", PlayerPrefs.GetInt("EnemiesKilled") + 1);
            PersistentManagerScript.Instance.CheckAchievements();
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

            col.color = grad;

            Instantiate(Particles, transform.position, Quaternion.identity);
            ManageUI Interfacscript = GameObject.Find("Interface").GetComponent<ManageUI>();
            Interfacscript.SpawnBonus(gameObject.transform.position);
            if (!PersistentManagerScript.Instance.NormalMode)
            {
                GameObject bonus = Instantiate(BonusPoints, gameObject.transform.position, Quaternion.identity);
                bonus.GetComponent<MeshRenderer>().sortingOrder = 500;
                PersistentManagerScript.Instance.Points += 400;
                string Points = "Score " + Convert.ToString(PersistentManagerScript.Instance.Points);
                Interfacscript.PointBanner.text = Points;
                Interfacscript.ScoreOnPause.text = Points;
            }
        }
    }
}