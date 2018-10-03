using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class Share : MonoBehaviour
{
    public GameObject CanvasShareObj;
    private bool IsProcessing = false;
    bool Isfocus = false;
    public GameObject Sound;


    public GameObject Canvas;
    GameObject ShareScreen;

    public void ShareBtnPress()
    {
        if (!IsProcessing)
        {
            if (Sound != null)
            {
                Sound.SetActive(false);
            }

            ShareScreen = Instantiate(CanvasShareObj, CanvasShareObj.transform.position, Quaternion.identity);
            ShareScreen.transform.SetParent(Canvas.transform, false);
            if (PersistentManagerScript.Instance.NormalMode)
            {
                ShareScreen.transform.GetChild(0).GetComponent<Text>().text = "I already completed " + Convert.ToString(PlayerPrefs.GetInt("FinishedLevel") + " Level");
            }
            else
            {
                ShareScreen.transform.GetChild(0).GetComponent<Text>().text = "My HighScore: " + Convert.ToString(PlayerPrefs.GetInt("HighScore"));
            }


            StartCoroutine(ShareScreenshot());
        }
    }

    IEnumerator ShareScreenshot()
    {
        IsProcessing = true;
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot("screenshot.png");
        string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");

        yield return new WaitForSecondsRealtime(0.3f);

        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "https://play.google.com/store/apps/details?id=com.EnterRavement.Splitterz Check out my Highscore in Splitterz ;)");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your score with your friends :)");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1);
            yield return new WaitUntil(() => Isfocus);
            Destroy(ShareScreen);
            if (Sound != null)
            {
                Sound.SetActive(true);
            }
            IsProcessing = false;
        }

    }

    void OnApplicationPause(bool focus)
    {
        Isfocus = focus;
        Destroy(ShareScreen);
        if (Sound != null)
        {
            Sound.SetActive(true);
        }
        IsProcessing = false;
    }



}
