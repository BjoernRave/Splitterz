using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine.Audio;

public class PersistentManagerScript : MonoBehaviour, IRewardedVideoAdListener
{

    public static PersistentManagerScript Instance { get; private set; }
    bool BannerLoaded = false;
    public GameObject NoInternet;
    public int Points;
    public int PointsAdded;
    public int PercentageAdded;
    public int CurrentSceneNumber = 1;
    public int FinishedLevel = 0;
    public AudioClip music;
    public AudioSource musicsource;
    public bool SoundMuted = false;
    public int TimesFailed = 0;

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Appodeal.onResume();
        }
    }

    private void Update()
    {
        if (SoundMuted)
        {
            musicsource.mute = enabled;
        }
        else
        {
            musicsource.mute = !enabled;
        }


        if (Points > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", Points);
        }

        if (Appodeal.isLoaded(Appodeal.BANNER) && BannerLoaded == false && PlayerPrefs.GetInt("NoAds") != 1)
        {
            Appodeal.show(Appodeal.BANNER_BOTTOM);
            BannerLoaded = true;
        }

        if (TimesFailed >= 5)
        {
            if (Appodeal.isLoaded(Appodeal.INTERSTITIAL) && PlayerPrefs.GetInt("NoAds") != 1)
            {
                TimesFailed = 0;
                Appodeal.show(Appodeal.INTERSTITIAL);
            }
        }

    }


    private void Start()
    {

        Appodeal.disableWriteExternalStoragePermissionCheck();
        musicsource = gameObject.AddComponent<AudioSource>();
        musicsource.clip = music;
        musicsource.Play();
        Screen.orientation = ScreenOrientation.Portrait;
        string appKey = "b3176284bbd040c623719148d67075d690b29bb9a2ac0730";
        Appodeal.disableLocationPermissionCheck();

        Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            PlayerPrefs.SetInt("NoAds", 1);
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

    public void onRewardedVideoLoaded()
    {

    }

    public void onRewardedVideoFailedToLoad()
    {
        GameObject.Find("NoInternet").SetActive(true);
    }

    public void onRewardedVideoShown()
    {


    }

    public void onRewardedVideoFinished(int amount, string name)
    {
    }

    public void onRewardedVideoClosed(bool finished)
    {

    }
}
