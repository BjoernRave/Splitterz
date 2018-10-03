using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class progressscreen : MonoBehaviour
{

    public GameObject Ach1;
    public GameObject Ach2;
    public GameObject Ach3;
    public GameObject Ach4;
    public GameObject Ach5;
    public GameObject Ach6;
    public GameObject Ach7;
    public GameObject Ach8;
    public GameObject Ach9;
    public GameObject Ach10;
    public GameObject Ach11;
    public GameObject Ach12;

    public Text TotalProgress;
    public Text LevelCompleted;

    int AchComplete;

    // Use this for initialization

    public void BackHome()
    {
        SceneManager.LoadScene("StartScreen");

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartScreen");
        }
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("FinishedLevel") >= 10)
        {
            adjustAlpha(Ach1);

        }
        if (PlayerPrefs.GetInt("FinishedLevel") >= 25)
        {
            adjustAlpha(Ach2);

        }
        if (PlayerPrefs.GetInt("FinishedLevel") >= 50)
        {
            adjustAlpha(Ach3);

        }
        if (PlayerPrefs.GetInt("FullStar") >= 10)
        {
            adjustAlpha(Ach4);

        }
        if (PlayerPrefs.GetInt("FullStar") >= 25)
        {
            adjustAlpha(Ach5);

        }
        if (PlayerPrefs.GetInt("FullStar") >= 50)
        {
            adjustAlpha(Ach6);

        }
        if (PlayerPrefs.GetInt("EnemiesKilled") >= 50)
        {
            adjustAlpha(Ach7);

        }
        if (PlayerPrefs.GetInt("EnemiesKilled") >= 100)
        {
            adjustAlpha(Ach8);

        }
        if (PlayerPrefs.GetInt("EnemiesKilled") >= 200)
        {
            adjustAlpha(Ach9);

        }
        if (PlayerPrefs.GetFloat("AreaFilled") >= 250000)
        {
            adjustAlpha(Ach10);

        }
        if (PlayerPrefs.GetFloat("AreaFilled") >= 500000)
        {
            adjustAlpha(Ach11);

        }
        if (PlayerPrefs.GetFloat("AreaFilled") >= 1000000)
        {
            adjustAlpha(Ach12);

        }
        LevelCompleted.text = Convert.ToString(PlayerPrefs.GetInt("FinishedLevel")) + " Level completed";
        float PercentComplete = PlayerPrefs.GetInt("FinishedLevel") * 1.4f;
        PercentComplete += AchComplete * 2.5f;
        TotalProgress.text = "Completed the game: " + Convert.ToString(PercentComplete) + "%";

    }
    public void adjustAlpha(GameObject GObj)
    {
        Image[] Images = GObj.GetComponentsInChildren<Image>();
        foreach (Image pic in Images)
        {
            Color NewColor = pic.color;
            NewColor.a = 1;
            pic.color = NewColor;
        }
        Color NewColor2 = GObj.GetComponentInChildren<Text>().color;
        NewColor2.a = 1;
        GObj.GetComponentInChildren<Text>().color = NewColor2;
        AchComplete += 1;
    }

}
