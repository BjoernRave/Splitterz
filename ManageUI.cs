using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine.Analytics;

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
    public GameObject ShootIce;
    GameObject[] PossibleBonus;
    public float ChanceForDrop = 2;
    public static bool WallsGoFaster = false;
    public static bool WallsGoSlower = false;
    public static bool Subtractable = true;

    public GameObject NoInternet;
    public GameObject LifeSubtract;

    bool Adwatched = false;
    public Image Muted;
    float currentimescale = 1;
    public Text ScoreOnPause;
    public Text ScoreOnPause2;
    public Text ScoreOnPause3;
    public Text ScoreOnFinish;
    public Text StageOnPause;
    public GameObject Tutorial;
    public GameObject LeftText;
    public GameObject RightText;

    public GameObject PauseScreen;

    public GameObject NextLevelScreen;
    public GameObject FailedWithAd;
    public GameObject FailedNoAd;
    public GameObject SoundMute;

    public Text CurrentLevel;


    public GameObject Stars;



    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                DisableOnDestroys();
                SceneManager.LoadScene("StartScreen");

            }
            else
            {
                PauseButton();
            }

        }
    }

    void Start()
    {

        Screen.fullScreen = true;
        PossibleBonus = new GameObject[5];
        PossibleBonus[0] = Wallspeed;
        PossibleBonus[1] = TimeSpeed;
        PossibleBonus[3] = ShootMissile;
        PossibleBonus[2] = PlaceBomb;
        PossibleBonus[4] = ShootIce;

        //PlayerPrefs.DeleteKey("RateMenu");


        if (PlayerPrefs.GetInt("Tutorial") != 1)
        {
            Time.timeScale = 0;
            swipegame._Character = new GameObject();
            Instantiate(Tutorial, Tutorial.transform.position, Quaternion.identity);
            PlayerPrefs.SetInt("Tutorial", 1);
        }

    }


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
            else if (Enemy.GetComponent<Enemy6Script>() != null)
            {
                Enemy.GetComponent<Enemy6Script>().SceneChanging = false;
            }
            else if (Enemy.GetComponent<BombScript>() != null)
            {
                Enemy.GetComponent<BombScript>().SceneChanging = false;
            }
        }
    }
    public void ResumeGame()
    {
        swipegame._Character = null;
        PauseScreen.SetActive(false);
        Time.timeScale = currentimescale;
        FailedWithAd.SetActive(false);

        SoundMute.SetActive(false);
    }


    public void RestartGame()
    {
        DisableOnDestroys();
        swipegame._Character = null;
        if (PersistentManagerScript.Instance.NormalMode)
        {
            PlayersLifes = 2;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            PlayersLifes = 3;
            PersistentManagerScript.Instance.Points = 0;
            SceneManager.LoadScene("Endless");
            EndlessScript.stages = 1;
            //GameObject.Find("Interface").GetComponent<EndlessScript>().GenerateStage();
        }

        FailedNoAd.SetActive(false);
        FailedWithAd.SetActive(false);
        NextLevelScreen.SetActive(false);
        ManageWalls.Assignable = true;
        if (GameObject.Find("0") != null)
        {
            GameObject.Find("0").GetComponent<Overlapping2>().Assigning = true;
        }


        PauseScreen.SetActive(false);


        Adwatched = false;
        SoundMute.SetActive(false);
        percentageDiff = 0;
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!focusStatus && Time.timeScale != 0 && !Application.isEditor)
        {
            PauseButton();
        }

    }



    public void PauseButton()
    {

        PauseScreen.SetActive(true);
        currentimescale = Time.timeScale;
        Time.timeScale = 0;
        swipegame._Character = new GameObject();
        if (!PersistentManagerScript.Instance.NormalMode)
        {
            ScoreOnPause.text = "Score: " + Convert.ToString(PersistentManagerScript.Instance.Points);
        }

        SoundMute.SetActive(true);

    }

    public void Sound(bool value)
    {

        PersistentManagerScript.Instance.musicsource.mute = !PersistentManagerScript.Instance.musicsource.mute;

        if (PersistentManagerScript.Instance.musicsource.mute)
        {
            Muted.enabled = true;
            PlayerPrefs.SetInt("Mute", 1);
            SoundMute.GetComponentInChildren<Text>().text = "Umute Sound";
        }
        else
        {
            Muted.enabled = false;
            PlayerPrefs.SetInt("Mute", 0);
            SoundMute.GetComponentInChildren<Text>().text = "Mute Sound";
        }


    }



    public void BacktoMenu()
    {
        PlayersLifes = 3;
        percentageDiff = 0;
        DisableOnDestroys();
        PersistentManagerScript.Instance.Points = 0;
        Adwatched = false;
        SceneManager.LoadScene("StartScreen");
        EndlessScript.stages = 1;
        SoundMute.SetActive(false);
    }

    public void LoadNexteLevel()
    {
        DisableOnDestroys();
        if (PersistentManagerScript.Instance.NormalMode)
        {
            SceneManager.LoadScene(Convert.ToString(PersistentManagerScript.Instance.CurrentSceneNumber));
        }
        else
        {
            SceneManager.LoadScene("Endless");
            // GameObject.Find("Interface").GetComponent<EndlessScript>().GenerateStage();
        }

        swipegame._Character = null;
        percentageDiff = 0;

        NextLevelScreen.SetActive(false);

        SoundMute.SetActive(false);
    }



    public void ShowRewardedVideo()
    {
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) || PersistentManagerScript.Instance.NoAds)
        {
            if (!PersistentManagerScript.Instance.NoAds)
            {
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }
            Adwatched = true;
            PlayersLifes += 1;
            FailedWithAd.SetActive(false);
            PauseScreen.SetActive(true);
            SoundMute.SetActive(true);
            Life.text = Convert.ToString(PlayersLifes);
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

            GameObject Bonus = Instantiate(PossibleBonus[UnityEngine.Random.Range(0, 5)], Position, Quaternion.identity);
            Bonus.GetComponent<Rigidbody2D>().AddForce(RandDir(), ForceMode2D.Impulse);


        }
    }

    public void SubtractLife()
    {
        if (Subtractable)
        {

            PlayersLifes -= 1;
            Subtractable = false;
            LifeSubtract.SetActive(true);
            Life.text = Convert.ToString(PlayersLifes);
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



    public void CheckLevelComletion()
    {



        if (percentageDiff >= 90)
        {
            DisableOnDestroys();
            OnLevelComplete();
            if (PersistentManagerScript.Instance.NormalMode)
            {
                if (PersistentManagerScript.Instance.CurrentSceneNumber < 50)
                {
                    PersistentManagerScript.Instance.CurrentSceneNumber += 1;
                }
                else if (PersistentManagerScript.Instance.CurrentSceneNumber == 50)
                {

                    SceneManager.LoadScene("GameComplete");
                    PersistentManagerScript.Instance.CurrentSceneNumber = 1;
                }



                if (PersistentManagerScript.Instance.FinishedLevel < PersistentManagerScript.Instance.CurrentSceneNumber)
                {
                    PersistentManagerScript.Instance.FinishedLevel += 1;
                    PlayerPrefs.SetInt("FinishedLevel", PersistentManagerScript.Instance.FinishedLevel);
                }
            }
            else
            {
                ScoreOnFinish.text = "Current Score: " + Convert.ToString(PersistentManagerScript.Instance.Points);
                StageOnPause.text = "Finished Stages: " + Convert.ToString(EndlessScript.stages - 1);
                if (PersistentManagerScript.Instance.Points > PlayerPrefs.GetInt("HighScore"))
                {
                    PlayerPrefs.SetInt("HighScore", PersistentManagerScript.Instance.Points);
                }
                // GameObject.Find("Interface").GetComponent<EndlessScript>().GenerateStage();



            }
            SoundMute.SetActive(true);
            NextLevelScreen.SetActive(true);
            swipegame._Character = new GameObject();
            Time.timeScale = 0;



            PauseScreen.SetActive(false);
            PersistentManagerScript.Instance.CheckAchievements();
        }


        if (MainScript.FillWholeField)
        {
            FillPercentage.text = "100% Filled";
        }

        if (PlayersLifes == 0 && Time.timeScale != 0)
        {

            if (Adwatched)
            {
                if (!PersistentManagerScript.Instance.NormalMode)
                {
                    ScoreOnPause2.text = Convert.ToString(PersistentManagerScript.Instance.Points);
                }

                FailedNoAd.SetActive(true);
                SoundMute.SetActive(true);
                swipegame._Character = new GameObject();
                Time.timeScale = 0;
            }
            else
            {
                if (!PersistentManagerScript.Instance.NormalMode)
                {
                    ScoreOnPause3.text = Convert.ToString(PersistentManagerScript.Instance.Points);
                }

                swipegame._Character = new GameObject();
                FailedWithAd.SetActive(true);
                SoundMute.SetActive(true);
                Time.timeScale = 0;
            }
            if (!Adwatched)
            {

                PersistentManagerScript.Instance.TimesFailed += 1;
            }
            if (PersistentManagerScript.Instance.Points > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", PersistentManagerScript.Instance.Points);
            }
            if (PersistentManagerScript.Instance.TimesFailed >= 5)
            {
                if (Appodeal.isLoaded(Appodeal.INTERSTITIAL) && !PersistentManagerScript.Instance.NoAds)
                {
                    PersistentManagerScript.Instance.TimesFailed = 0;
                    Appodeal.show(Appodeal.INTERSTITIAL);
                }
            }

            if (!PersistentManagerScript.Instance.NormalMode)
            {

                ScoreOnPause.text = "Total Score: " + Convert.ToString(PersistentManagerScript.Instance.Points);

            }

        }
    }


    public void OnLevelComplete()
    {
        Analytics.CustomEvent("TimetoCompleteLevel", new Dictionary<string, object>
        {
            { "Level", PersistentManagerScript.Instance.CurrentSceneNumber },
            { "time", Time.timeSinceLevelLoad }
        });
    }

    public void Facebook()
    {
        Application.OpenURL("https://www.facebook.com/EnterRavement/");
    }

    public void Twitter()
    {
        Application.OpenURL("https://twitter.com/EnterRavement");
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
        if (PersistentManagerScript.Instance.NormalMode)
        {
            CurrentLevel.text = SceneManager.GetActiveScene().name;
            PlayersLifes = 2;
            Life.text = Convert.ToString(PlayersLifes);
            if (Stars.GetComponent<Animator>().isInitialized)
            {
                Stars.GetComponent<Animator>().SetBool("Star2", false);
                Stars.GetComponent<Animator>().SetBool("Star3", false);
            }
        }
        else
        {
            string Points = "Score " + Convert.ToString(PersistentManagerScript.Instance.Points);
            PointBanner.text = Points;
            ScoreOnPause.text = Points;
        }
        swipegame.Playerdropped = false;
        WallsGoFaster = false;
        WallsGoSlower = false;
        PauseScreen.SetActive(false);
        NextLevelScreen.SetActive(false);
        FailedWithAd.SetActive(false);
        FailedNoAd.SetActive(false);


        if (PersistentManagerScript.Instance.musicsource.mute)
        {
            Muted.enabled = true;
            PlayerPrefs.SetInt("Mute", 1);
            SoundMute.GetComponentInChildren<Text>().text = "Umute Sound";
        }
        else
        {
            Muted.enabled = false;
            PlayerPrefs.SetInt("Mute", 0);
            SoundMute.GetComponentInChildren<Text>().text = "Mute Sound";
        }



        FillPercentage.text = Convert.ToString(ManageUI.percentageDiff) + "% Filled";
        Life.text = Convert.ToString(PlayersLifes);

    }
}
