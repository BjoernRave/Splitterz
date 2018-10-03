using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class MenuScript : MonoBehaviour
{
    public Camera cam;
    public Text Score;
    public Image Muted;
    public GameObject RatePopup;
    public bool Exitactive = false;
    public GameObject ExitMenu;
    public GameObject Mute;
    public GameObject NoAdsbtn;

    public GameObject ChooseSceneBtn;

    public Text ModeText;

    void Start()
    {
        if (PlayerPrefs.GetInt("Mode") == 0)
        {
            PersistentManagerScript.Instance.NormalMode = true;
            ModeText.text = "Normal";
            ChooseSceneBtn.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("Mode") == 1)
        {
            PersistentManagerScript.Instance.NormalMode = false;
            ModeText.text = "Endless";
            ChooseSceneBtn.SetActive(false);
        }

        if (PlayerPrefs.GetInt("NoAds") == 1)
        {
            NoAdsbtn.GetComponent<Image>().enabled = false;
            NoAdsbtn.GetComponentInChildren<Text>().text = "Disabled Ads";
            NoAdsbtn.transform.GetChild(0).GetComponent<Image>().enabled = true;
            NoAdsbtn.GetComponent<Button>().enabled = false;
        }


        if (PersistentManagerScript.Instance.musicsource.mute)
        {
            Muted.enabled = true;
            PlayerPrefs.SetInt("Mute", 1);
            Mute.GetComponentInChildren<Text>().text = "Umute Sound";
        }
        else
        {
            Muted.enabled = false;
            PlayerPrefs.SetInt("Mute", 0);
            Mute.GetComponentInChildren<Text>().text = "Mute Sound";
        }

    }


    private void Update()
    {
        Score.text = Convert.ToString(PlayerPrefs.GetInt("HighScore"));
        if (Input.GetKey(KeyCode.Escape) && !Exitactive)
        {
            Instantiate(ExitMenu, ExitMenu.transform.position, Quaternion.identity);
            Exitactive = true;
        }
        else if (Input.GetKey(KeyCode.Escape) && Exitactive)
        {
            Application.Quit();
        }

    }

    public void ChangeMode()
    {
        PersistentManagerScript.Instance.NormalMode = !PersistentManagerScript.Instance.NormalMode;
        if (PersistentManagerScript.Instance.NormalMode)
        {
            ModeText.text = "Normal";
            PlayerPrefs.SetInt("Mode", 0);
            ChooseSceneBtn.SetActive(true);
        }
        else
        {
            ModeText.text = "Endless";
            PlayerPrefs.SetInt("Mode", 1);
            ChooseSceneBtn.SetActive(false);
        }
    }

    public void LoadProgress()
    {
        SceneManager.LoadScene("Progress");
    }

    public void LoadSceneSelection()
    {
        SceneManager.LoadScene("SceneManager");

    }

    public void Sound(bool value)
    {
        PersistentManagerScript.Instance.musicsource.mute = !PersistentManagerScript.Instance.musicsource.mute;

        if (PersistentManagerScript.Instance.musicsource.mute)
        {
            Muted.enabled = true;
            PlayerPrefs.SetInt("Mute", 1);
            Mute.GetComponentInChildren<Text>().text = "Umute Sound";
        }
        else
        {
            Muted.enabled = false;
            PlayerPrefs.SetInt("Mute", 0);
            Mute.GetComponentInChildren<Text>().text = "Mute Sound";
        }


    }

    public void StartGame()
    {
        if (PersistentManagerScript.Instance.NormalMode)
        {
            SceneManager.LoadScene(PersistentManagerScript.Instance.CurrentSceneNumber);
        }
        else
        {
            SceneManager.LoadScene("Endless");
        }


    }

}
