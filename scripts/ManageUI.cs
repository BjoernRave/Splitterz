using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class ManageUI : MonoBehaviour
{
    public static double percentageDiff = new double();
    public static int EnemyCount = new int();
    public static int PlayersLifes = 3;
    public Text Life;
    public Text FillPercentage;
    public Text PointBanner;
    public GameObject Wallspeed;
    public GameObject TimeSpeed;
    public GameObject PlaceBomb;
    public GameObject ShootMissile;
    GameObject[] PossibleBonus;
    public float ChanceForDrop = 2;
    public static bool WallsGoFaster = false;
    public static bool WallsGoSlower = false;
    public static bool Subtractable = true;
    public GameObject PauseRestartScreen;
    public GameObject RestartButton;
    public GameObject ResumeButton;
    public GameObject NextLevelButton;
    public GameObject PauseExitButton;
    public GameObject NoInternet;
    public GameObject LifeSubtract;
    public GameObject AdButton;
    public GameObject FailBanner;
    public GameObject WinBanner;
    bool Adwatched = false;
    public Image Muted;
    float currentimescale = 1;
    public Text ScoreOnPause;
    public GameObject Tutorial;
    public GameObject LeftText;
    public GameObject RightText;
    public void DisableOnDestroys()
    {
        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Enemy.GetComponent<Enemy1Script>() != null)
            {
                Enemy.GetComponent<Enemy1Script>().SceneChanging = false;
            }
            else if (Enemy.GetComponent<Enemy2Script>() != null)
            {
                Enemy.GetComponent<Enemy2Script>().SceneChanging = false;
            }
            else if (Enemy.GetComponent<Enemy3Script>() != null)
            {
                Enemy.GetComponent<Enemy3Script>().SceneChanging = false;
            }
            else if (Enemy.GetComponent<Enemy4Script>() != null)
            {
                Enemy.GetComponent<Enemy4Script>().SceneChanging = false;
            }
            else if (Enemy.GetComponent<BombScript>() != null)
            {
                Enemy.GetComponent<BombScript>().SceneChanging = false;
            }
        }
    }
    public void ResumeGame()
    {
        RestartButton.SetActive(false);
        GameObject.Find("Interface").GetComponent<TouchInput>()._Character = null;
        PauseRestartScreen.SetActive(false);
        ResumeButton.SetActive(false);
        Time.timeScale = currentimescale;
        PauseExitButton.SetActive(true);
    }


    public void RestartGame()
    {
        ResumeButton.SetActive(false);
        GameObject.Find("Interface").GetComponent<TouchInput>()._Character = null;
        PlayersLifes = 3;
        PauseRestartScreen.SetActive(false);
        ManageWalls.Assignable = true;
        if (GameObject.Find("0") != null)
        {
            GameObject.Find("0").GetComponent<Overlapping2>().Assigning = true;
        }
        PersistentManagerScript.Instance.Points = 0;
        DisableOnDestroys();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        FailBanner.SetActive(false);
        WinBanner.SetActive(false);
        Adwatched = false;
        RestartButton.SetActive(false);
        percentageDiff = 0;
    }


    public void PauseButton()
    {
        RestartButton.SetActive(true);
        currentimescale = Time.timeScale;
        PauseRestartScreen.SetActive(true);
        ResumeButton.SetActive(true);
        Time.timeScale = 0;
        GameObject.Find("Interface").GetComponent<TouchInput>()._Character = new GameObject();
        PauseExitButton.SetActive(false);
    }

    public void Sound(bool value)
    {

        PersistentManagerScript.Instance.SoundMuted = !PersistentManagerScript.Instance.SoundMuted;


    }



    public void BacktoMenu()
    {
        PlayersLifes = 3;
        percentageDiff = 0;
        DisableOnDestroys();
        PersistentManagerScript.Instance.Points = 0;
        Adwatched = false;
        SceneManager.LoadScene("StartScreen");
    }

    public void LoadNexteLevel()
    {
        SceneManager.LoadScene(Convert.ToString(PersistentManagerScript.Instance.CurrentSceneNumber));
        GameObject.Find("Interface").GetComponent<TouchInput>()._Character = null;
        percentageDiff = 0;
        DisableOnDestroys();
    }

    public void ShowRewardedVideo()
    {
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) || PlayerPrefs.GetInt("NoAds") == 1)
        {
            if (PlayerPrefs.GetInt("NoAds") != 1)
            {
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }
            Adwatched = true;
            PlayersLifes += 1;
            AdButton.SetActive(false);
            ResumeButton.SetActive(true);
            FailBanner.SetActive(false);
        }
        else
        {
            NoInternet.SetActive(true);
        }
    }

    public void SpawnBonus(Vector3 Position)
    {
        if (UnityEngine.Random.value < 0.5)
        {
            print("powerup");
            GameObject Bonus = Instantiate(PossibleBonus[UnityEngine.Random.Range(0, 3)], Position, Quaternion.identity);
            Bonus.GetComponent<Rigidbody2D>().AddForce(RandDir(), ForceMode2D.Impulse);


        }
    }

    public void SubtractLife()
    {
        if (Subtractable)
        {
            print("life -1");
            PlayersLifes -= 1;
            Subtractable = false;
            LifeSubtract.SetActive(true);
        }
    }

    public Vector2 RandDir()
    {
        int i = UnityEngine.Random.Range(1, 3);
        if (i == 1)
        {
            return new Vector2(1000, 1000);
        }
        else
        {
            return new Vector2(-1000, 1000);
        }
    }



    private void Start()
    {
        PossibleBonus = new GameObject[3];
        PossibleBonus[0] = Wallspeed;
        PossibleBonus[1] = TimeSpeed;


        //PossibleBonus[4] = ShootMissile;
        PossibleBonus[2] = PlaceBomb;



    }




    void Update()
    {
        if (PersistentManagerScript.Instance.SoundMuted)
        {
            Muted.enabled = true;
        }
        else
        {
            Muted.enabled = false;
        }

        if (PlayerPrefs.GetInt("Tutorial") != 1)
        {
            GameObject.Find("Enemy 1").GetComponent<Enemy1Script>().enabled = false;
            GameObject.Find("Interface").GetComponent<TouchInput>()._Character = new GameObject();
            Instantiate(Tutorial, Tutorial.transform.position, Quaternion.identity);
            PlayerPrefs.SetInt("Tutorial", 1);
        }


        Life.text = Convert.ToString(PlayersLifes);

        PointBanner.text = "Score " + Convert.ToString(PersistentManagerScript.Instance.Points);
        ScoreOnPause.text = "Score " + Convert.ToString(PersistentManagerScript.Instance.Points);

        if (percentageDiff != 0)
        {
            if (percentageDiff >= 90 && GameObject.Find("Interface").GetComponent<TouchInput>()._Character == null)
            {
                PersistentManagerScript.Instance.CurrentSceneNumber += 1;
                GameObject.Find("Interface").GetComponent<TouchInput>()._Character = new GameObject();
                Time.timeScale = 0;
                PauseRestartScreen.SetActive(true);
                NextLevelButton.SetActive(true);
                PauseExitButton.SetActive(false);
                WinBanner.SetActive(true);
                RestartButton.SetActive(true);
                if (PersistentManagerScript.Instance.FinishedLevel < PersistentManagerScript.Instance.CurrentSceneNumber)
                {
                    PersistentManagerScript.Instance.FinishedLevel += 1;
                    PlayerPrefs.SetInt("FinishedLevel", PersistentManagerScript.Instance.FinishedLevel);
                }
            }

            FillPercentage.text = Convert.ToString(percentageDiff) + "% Filled";
            if (Shortestpath3.FillWholeField)
            {
                FillPercentage.text = "100% Filled";
            }
        }

        if (PlayersLifes == 0 && Time.timeScale != 0)
        {
            if (Adwatched)
            {
                PauseRestartScreen.SetActive(true);
                RestartButton.SetActive(true);
                PauseExitButton.SetActive(false);
                FailBanner.SetActive(true);
                GameObject.Find("Interface").GetComponent<TouchInput>()._Character = new GameObject();
                Time.timeScale = 0;
            }
            else
            {
                GameObject.Find("Interface").GetComponent<TouchInput>()._Character = new GameObject();
                PauseRestartScreen.SetActive(true);
                RestartButton.SetActive(true);
                Time.timeScale = 0;
                PauseExitButton.SetActive(false);
                AdButton.SetActive(true);
                FailBanner.SetActive(true);
            }
            if (!Adwatched)
            {
                print("lul");
                PersistentManagerScript.Instance.TimesFailed += 1;
            }
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

        WallsGoFaster = false;
        WallsGoSlower = false;
        PauseRestartScreen.SetActive(false);
        ResumeButton.SetActive(false);
        RestartButton.SetActive(false);
        NextLevelButton.SetActive(false);


    }
}
