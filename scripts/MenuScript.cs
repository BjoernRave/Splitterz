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


    private void Update()
    {
        Score.text = Convert.ToString(PlayerPrefs.GetInt("HighScore"));
        if (PersistentManagerScript.Instance.SoundMuted)
        {
            Muted.enabled = true;
        }
        else
        {
            Muted.enabled = false;
        }
    }


    public void LoadSceneSelection()
    {
        SceneManager.LoadScene("SceneManager");
    }

    public void Sound(bool value)
    {
        PersistentManagerScript.Instance.SoundMuted = !PersistentManagerScript.Instance.SoundMuted;


    }

    public void StartGame()
    {
        SceneManager.LoadScene(PersistentManagerScript.Instance.CurrentSceneNumber);
    }

}
