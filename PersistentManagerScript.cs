using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using AppodealAds.Unity.Android;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Analytics;


public class PersistentManagerScript : MonoBehaviour
{

    public static PersistentManagerScript Instance { get; private set; }

    public int Points;


    public int CurrentSceneNumber = 1;
    public int FinishedLevel = 0;
    public AudioClip music;
    public AudioClip music2;
    public AudioSource musicsource;
    public int TimesFailed = 0;

    public Sprite Enemies;
    public Sprite Filling;
    public Sprite Level;
    public Sprite FullStars;

    public GameObject AchievementUnlock;

    public AudioClip PowerUp;
    public AudioClip PowerDown;
    public AudioClip Collide;
    public AudioClip Stars;
    public AudioClip cheering;
    public AudioClip Fuse;
    public AudioClip Explosion;
    public AudioClip EnemieKilled;
    bool focus = false;
    public bool NoAds = false;

    bool BannerLoaded = false;

    public bool NormalMode = true;
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && !focus)
        {
            Appodeal.onResume();
            focus = true;


        }
        if (!hasFocus)
        {
            focus = false;

        }
    }

    private void Update()
    {
        if (!BannerLoaded)
        {
            if (Appodeal.isLoaded(Appodeal.BANNER) && !NoAds)
            {
                Appodeal.show(Appodeal.BANNER_BOTTOM);
                BannerLoaded = true;
            }
        }

    }




    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;


        if (PlayerPrefs.GetInt("NoAds") == 1)
        {
            NoAds = true;
        }
        if (Random.value < 0.5f)
        {
            musicsource.clip = music2;

        }
        else
        {
            musicsource.clip = music;
        }
        musicsource.Play();
        Screen.orientation = ScreenOrientation.Portrait;
        string appKey = "b3176284bbd040c623719148d67075d690b29bb9a2ac0730";
        Appodeal.disableLocationPermissionCheck();
        Appodeal.disableWriteExternalStoragePermissionCheck();

        Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO);
        if (PlayerPrefs.GetInt("FirstTime") != 1)
        {
            PlayerPrefs.SetInt("AreaFilled", 0);
            PlayerPrefs.SetInt("FilledAreaAch", 0);
            PlayerPrefs.SetInt("EnemiesKilled", 0);
            PlayerPrefs.SetInt("EnemiesKilledAch", 0);
            PlayerPrefs.SetInt("FullStar", 0);
            PlayerPrefs.SetInt("FullStarAch", 0);
            PlayerPrefs.SetInt("FinishedLevelAch", 0);
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.SetInt("NoAds", 0);
        }


    }

    private void Awake()
    {

        musicsource = gameObject.GetComponent<AudioSource>();
        if (Instance == null)
        {


            if (PlayerPrefs.GetInt("Mute") == 1)
            {
                musicsource.mute = true;

            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (PlayerPrefs.GetInt("FinishedLevel") != 0)
            {
                FinishedLevel = PlayerPrefs.GetInt("FinishedLevel");
            }
            CurrentSceneNumber = FinishedLevel + 1;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnApplicationQuit()
    {
        if (PlayerPrefs.GetInt("RateMenu") == 2)
        {
            PlayerPrefs.SetInt("TimesClosed", PlayerPrefs.GetInt("TimesClosed") + 1);
        }
        Analytics.CustomEvent("ClosedGame at Level" + System.Convert.ToString(CurrentSceneNumber));

    }

    public void CheckAchievements()
    {
        Color bronze = new Color(0.804f, 0.498f, 0.197f);
        Color silver = new Color(0.75f, 0.753f, 0.753f);
        Color gold = new Color(1, 0.843f, 0);


        if (PlayerPrefs.GetInt("EnemiesKilled") >= 50 && PlayerPrefs.GetInt("EnemiesKilledAch") < 1)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "Killed 50 Enemies";
            AchPic.sprite = Enemies;
            AchPic.color = bronze;

            PlayerPrefs.SetInt("EnemiesKilledAch", 1);

        }
        else if (PlayerPrefs.GetInt("EnemiesKilled") >= 100 && PlayerPrefs.GetInt("EnemiesKilledAch") < 2)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "Killed 100 Enemies";
            AchPic.sprite = Enemies;
            AchPic.color = silver;

            PlayerPrefs.SetInt("EnemiesKilledAch", 2);

        }
        else if (PlayerPrefs.GetInt("EnemiesKilled") >= 200 && PlayerPrefs.GetInt("EnemiesKilledAch") < 3)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "Killed 200 Enemies";
            AchPic.sprite = Enemies;
            AchPic.color = gold;

            PlayerPrefs.SetInt("EnemiesKilledAch", 3);

        }

        if (PlayerPrefs.GetInt("FullStar") >= 10 && PlayerPrefs.GetInt("FullStarAch") < 1)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = " 10 Times Full Star-Rating";
            AchPic.sprite = FullStars;
            AchPic.color = bronze;

            PlayerPrefs.SetInt("FullStarAch", 1);

        }
        else if (PlayerPrefs.GetInt("FullStar") >= 25 && PlayerPrefs.GetInt("FullStarAch") < 2)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = " 25 Times Full Star-Rating";
            AchPic.sprite = FullStars;
            AchPic.color = silver;

            PlayerPrefs.SetInt("FullStarAch", 2);

        }
        else if (PlayerPrefs.GetInt("FullStar") >= 50 && PlayerPrefs.GetInt("FullStarAch") < 3)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = " 50 Times Full Star-Rating";
            AchPic.sprite = FullStars;
            AchPic.color = gold;

            PlayerPrefs.SetInt("FullStarAch", 3);

        }



        if (PlayerPrefs.GetFloat("AreaFilled") >= 150000 && PlayerPrefs.GetInt("FilledAreaAch") < 1)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "650 cm² filled";
            AchPic.sprite = Filling;
            AchPic.color = bronze;

            PlayerPrefs.SetInt("FilledAreaAch", 1);

        }
        else if (PlayerPrefs.GetFloat("AreaFilled") >= 350000 && PlayerPrefs.GetInt("FilledAreaAch") < 2)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "1500 cm² filled";
            AchPic.sprite = Filling;
            AchPic.color = silver;

            PlayerPrefs.SetInt("FilledAreaAch", 2);

        }
        else if (PlayerPrefs.GetFloat("AreaFilled") >= 660000 && PlayerPrefs.GetInt("FilledAreaAch") < 3)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "3000 cm² filled";
            AchPic.sprite = Filling;
            AchPic.color = gold;

            PlayerPrefs.SetInt("FilledAreaAch", 3);

        }


        if (PlayerPrefs.GetInt("FinishedLevel") >= 10 && PlayerPrefs.GetInt("FinishedLevelAch") < 1)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "10 Level Completed";
            AchPic.sprite = Level;
            AchPic.color = bronze;

            PlayerPrefs.SetInt("FinishedLevelAch", 1);

        }
        else if (PlayerPrefs.GetInt("FinishedLevel") >= 25 && PlayerPrefs.GetInt("FinishedLevelAch") < 2)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "25 Level Completed";
            AchPic.sprite = Level;
            AchPic.color = silver;

            PlayerPrefs.SetInt("FinishedLevelAch", 2);

        }
        else if (PlayerPrefs.GetInt("FinishedLevel") >= 50 && PlayerPrefs.GetInt("FinishedLevelAch") < 3)
        {
            Instantiate(AchievementUnlock, AchievementUnlock.transform.position, Quaternion.identity);
            Text AchText = GameObject.Find("AchText").GetComponent<Text>();
            Image AchPic = GameObject.Find("AchPic").GetComponent<Image>();
            AchText.text = "50 Level Completed";
            AchPic.sprite = Level;
            AchPic.color = gold;

            PlayerPrefs.SetInt("FinishedLevelAch", 3);

        }

    }
}
