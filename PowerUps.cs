using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PowerUps : MonoBehaviour
{
    bool crrun = false;
    bool GreenActive = false;
    GameObject LeftText;
    GameObject RightText;
    IEnumerator ChangePowerUp()
    {
        crrun = true;
        if (GreenActive)
        {
            GreenActive = false;
            yield return new WaitForSecondsRealtime(0.8f);
        }
        else if (!GreenActive)
        {
            GreenActive = true;
            yield return new WaitForSecondsRealtime(2f);
        }

        crrun = false;

    }


    // Use this for initialization
    void Start()
    {
        if (Random.value >= 0.5)
        {
            GreenActive = true;

        }
        LeftText = GameObject.Find("Interface").GetComponent<ManageUI>().LeftText;
        RightText = GameObject.Find("Interface").GetComponent<ManageUI>().RightText;

        Destroy(gameObject, 8);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name != "PowerUpIce(Clone)" && gameObject.name != "PowerUpBomb(Clone)" && gameObject.name != "PowerUpMissile(Clone)")
        {
            if (!crrun)
            {
                StartCoroutine(ChangePowerUp());
            }
        }
        if (GreenActive)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (!GreenActive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

    }
    public void ActivatePowerUP()
    {
        LeftText.SetActive(true);
        RightText.SetActive(true);
        if (gameObject.name == "PowerUpTime(Clone)")
        {
            LeftText.GetComponent<Text>().text = "Time";
            if (GetComponent<SpriteRenderer>().enabled == true)
            {
                PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.PowerUp);
                RightText.GetComponent<Text>().color = Color.green;
                LeftText.GetComponent<Text>().color = Color.green;
                Time.timeScale = 0.6f;
                RightText.GetComponent<Text>().text = "Slowdown";
                ManageUI.WallsGoFaster = true;
            }
            else
            {
                PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.PowerDown);
                LeftText.GetComponent<Text>().color = Color.red;
                RightText.GetComponent<Text>().color = Color.red;
                RightText.GetComponent<Text>().text = "Speed-Up";
                Time.timeScale = 1.5f;
                ManageUI.WallsGoSlower = true;
            }
        }
        if (gameObject.name == "PowerUpSpeed(Clone)")
        {
            LeftText.GetComponent<Text>().text = "Wall";
            if (GetComponent<SpriteRenderer>().enabled == true)
            {
                PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.PowerUp);
                LeftText.GetComponent<Text>().color = Color.green;
                RightText.GetComponent<Text>().color = Color.green;
                ManageUI.WallsGoFaster = true;
                RightText.GetComponent<Text>().text = "Speed-Up";
            }
            else
            {
                PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.PowerDown);
                LeftText.GetComponent<Text>().color = Color.red;
                RightText.GetComponent<Text>().color = Color.red;
                RightText.GetComponent<Text>().text = "Slowdown";
                ManageUI.WallsGoSlower = true;
            }
        }
        if (gameObject.name == "PowerUpBomb(Clone)")
        {
            PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.PowerUp);
            LeftText.GetComponent<Text>().color = Color.green;
            RightText.GetComponent<Text>().color = Color.green;
            LeftText.GetComponent<Text>().text = "Tactical";
            RightText.GetComponent<Text>().text = "Bomb";
            swipegame.Bomb = true;
        }
        if (gameObject.name == "PowerUpMissile(Clone)")
        {
            PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.PowerUp);
            LeftText.GetComponent<Text>().color = Color.green;
            RightText.GetComponent<Text>().color = Color.green;
            LeftText.GetComponent<Text>().text = "Tactical";
            RightText.GetComponent<Text>().text = "Missile";
            swipegame.Missile = true;
        }
        if (gameObject.name == "PowerUpIce(Clone)")
        {
            PersistentManagerScript.Instance.GetComponent<AudioSource>().PlayOneShot(PersistentManagerScript.Instance.PowerUp);
            LeftText.GetComponent<Text>().color = Color.green;
            RightText.GetComponent<Text>().color = Color.green;
            LeftText.GetComponent<Text>().text = "Tactical";
            RightText.GetComponent<Text>().text = "Ice-Cube";
            swipegame.Ice = true;
        }


        Destroy(gameObject);
    }
}
