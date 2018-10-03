using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{



    bool finished1 = false;
    bool finished2 = false;

    public GameObject Death;
    public GameObject Player;

    public Text button;
    // Use this for initialization

    public void Explosion()
    {
        Instantiate(Death, Player.transform.position, Quaternion.identity);
        button.text = "Let me try it!";
    }
    public void NextStepTutorial()
    {
        if (!finished1)
        {
            GameObject.Find("Tutorial(Clone)").GetComponent<Animator>().SetTrigger("nextpart");
            finished1 = true;
        }
        else if (!finished2)
        {
            GameObject.Find("Tutorial(Clone)").GetComponent<Animator>().SetTrigger("nextpart");
            finished2 = true;
        }
        else if (finished1 && finished2)
        {
            Destroy(GameObject.Find("Tutorial(Clone)"));
            Time.timeScale = 1;

        }


    }
}
