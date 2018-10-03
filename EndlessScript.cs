using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessScript : MonoBehaviour
{

    public GameObject lvl1;
    public GameObject lvl2;
    public GameObject lvl3;
    public GameObject lvl4;
    public GameObject lvl5;
    public GameObject lvl6;
    public GameObject lvl7;
    public GameObject lvl8;
    public GameObject lvl9;
    public GameObject lvl10;

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;
    public GameObject Background;

    GameObject[] Enemyprefabs = new GameObject[4];

    GameObject[] Level = new GameObject[10];

    public static int stages = 1;
    GameObject CurrentStage;

    GameObject[] Enemies;

    List<Vector2> SpawnPoints = new List<Vector2>();

    Color[] Colors = new Color[10];




    // Use this for initialization
    void Start()
    {
        Level[0] = lvl1;
        Level[1] = lvl2;
        Level[2] = lvl3;
        Level[3] = lvl4;
        Level[4] = lvl5;
        Level[5] = lvl6;
        Level[6] = lvl7;
        Level[7] = lvl8;
        Level[8] = lvl9;
        Level[9] = lvl10;
        Enemyprefabs[0] = Enemy1;
        Enemyprefabs[1] = Enemy2;
        Enemyprefabs[2] = Enemy3;
        Enemyprefabs[3] = Enemy4;
        Colors[0] = Color.green;
        Colors[1] = Color.red;
        Colors[2] = Color.blue;
        Colors[3] = Color.cyan;
        Colors[4] = Color.magenta;
        Colors[5] = Color.yellow;
        Colors[6] = new Color(0, 100, 0, 255);
        Colors[7] = new Color(214, 0, 199, 255);
        Colors[8] = new Color(255, 165, 0, 255);
        Colors[9] = new Color(135, 206, 250, 255);


        GenerateStage();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void GenerateStage()
    {

        // if (CurrentStage != null)
        // {
        //     Destroy(CurrentStage);
        // }
        // Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // foreach (GameObject Enemy in Enemies)
        // {
        //     Destroy(Enemy);
        // }
        Background.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, 10)];

        stages += 1;
        int Stage = Random.Range(0, 8);
        if (PlayerPrefs.GetInt("Tutorial") != 1)
        {
            CurrentStage = Instantiate(Level[9], Level[9].transform.position, Quaternion.identity);
        }
        else
        {
            CurrentStage = Instantiate(Level[Stage], Level[Stage].transform.position, Quaternion.identity);
        }

        SpawnPoints.Add(CurrentStage.transform.GetChild(0).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(1).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(2).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(3).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(4).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(5).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(6).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(7).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(8).transform.position);
        SpawnPoints.Add(CurrentStage.transform.GetChild(9).transform.position);

        int SPCount = 9;
        for (int i = 0; i < stages - 1; i++)
        {

            int SP = Random.Range(0, SPCount);
            SPCount -= 1;
            Instantiate(Enemyprefabs[Random.Range(0, 4)], SpawnPoints[SP], Quaternion.identity);
            SpawnPoints.RemoveAt(SP);
        }

    }
}
