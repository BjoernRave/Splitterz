using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class star : MonoBehaviour
{



    public float OneStarTime;
    public float TwoStarTime;
    public float ThreeStarTime;
    public GameObject RatePopup;
    // Use this for initialization
    void Start()
    {


        if (Time.timeSinceLevelLoad < ThreeStarTime)
        {

            PlayerPrefs.SetInt(Convert.ToString(SceneManager.GetActiveScene().name), 3);
            GetComponent<Animator>().SetBool("Star2", true);
            GetComponent<Animator>().SetBool("Star3", true);

            PlayerPrefs.SetInt("FullStar", PlayerPrefs.GetInt("FullStar") + 1);
        }
        else if (Time.timeSinceLevelLoad < TwoStarTime)
        {
            PlayerPrefs.SetInt(Convert.ToString(SceneManager.GetActiveScene().name), 2);
            GetComponent<Animator>().SetBool("Star2", true);
        }
        else if (Time.timeSinceLevelLoad < OneStarTime)
        {
            PlayerPrefs.SetInt(Convert.ToString(SceneManager.GetActiveScene().name), 1);
        }
        else
        {
            Destroy(gameObject);
            PlayerPrefs.SetInt(Convert.ToString(SceneManager.GetActiveScene().name), 0);

        }
        PersistentManagerScript.Instance.CheckAchievements();
    }
    public void ShowRatePopup()
    {



        if (PlayerPrefs.GetInt("RateMenu") == 2)
        {
            if (PlayerPrefs.GetInt("TimesClosed") > 1)
            {
                Instantiate(RatePopup, RatePopup.transform.position, Quaternion.identity);
            }
        }

        else if (PersistentManagerScript.Instance.CurrentSceneNumber > 6 && PlayerPrefs.GetInt("RateMenu") != 1 && PlayerPrefs.GetInt("RateMenu") != 2)
        {
            Instantiate(RatePopup, RatePopup.transform.position, Quaternion.identity);
        }

    }
    public void EmitKonfetti()
    {
        GetComponentInChildren<ParticleSystem>().Play();
    }

    public void PlaySound()
    {
        PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.Stars);
    }

    public void Cheer()
    {
        PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.cheering);
    }

}

