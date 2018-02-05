using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerUps : MonoBehaviour
{
    bool crrun = false;
    bool GreenActive = true;
    GameObject LeftText;
    GameObject RightText;
    IEnumerator ChangePowerUp()
    {
        crrun = true;
        if (GreenActive)
        {

            GetComponent<SpriteRenderer>().enabled = true;

            GreenActive = false;
            yield return new WaitForSecondsRealtime(1.2f);
        }
        else if (!GreenActive)
        {

            GetComponent<SpriteRenderer>().enabled = false;

            GreenActive = true;
            yield return new WaitForSecondsRealtime(0.8f);
        }

        crrun = false;

    }


    // Use this for initialization
    void Start()
    {
        LeftText = GameObject.Find("Interface").GetComponent<ManageUI>().LeftText;
        RightText = GameObject.Find("Interface").GetComponent<ManageUI>().RightText;

        Destroy(gameObject, 8);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name != "PowerUpBomb(Clone)")
        {
            if (!crrun)
            {
                StartCoroutine(ChangePowerUp());
            }
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
                RightText.GetComponent<Text>().color = Color.green;
                LeftText.GetComponent<Text>().color = Color.green;
                Time.timeScale = 0.6f;
                RightText.GetComponent<Text>().text = "Slowdown";
                ManageUI.WallsGoFaster = true;
            }
            else
            {
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
                LeftText.GetComponent<Text>().color = Color.green;
                RightText.GetComponent<Text>().color = Color.green;
                ManageUI.WallsGoFaster = true;
                RightText.GetComponent<Text>().text = "Speed-Up";
            }
            else
            {
                LeftText.GetComponent<Text>().color = Color.red;
                RightText.GetComponent<Text>().color = Color.red;
                RightText.GetComponent<Text>().text = "Slowdown";
                ManageUI.WallsGoSlower = true;
            }
        }
        if (gameObject.name == "PowerUpBomb(Clone)")
        {
            LeftText.GetComponent<Text>().color = Color.green;
            RightText.GetComponent<Text>().color = Color.green;
            LeftText.GetComponent<Text>().text = "Tactical";
            RightText.GetComponent<Text>().text = "Bomb";
            TouchInput.Bomb = true;
        }


        Destroy(gameObject);
    }
}
