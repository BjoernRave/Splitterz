using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{



    bool secondstep = false;
    bool thirdstep = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NextStepTutorial()
    {

        GameObject.Find("Tutorial(Clone)").GetComponent<Animator>().SetBool("Tutorial2", true);

        if (thirdstep == true)
        {
            Destroy(GameObject.Find("Tutorial(Clone)"));
            GameObject.Find("Enemy 1").GetComponent<Enemy1Script>().enabled = true;
            GameObject.Find("Interface").GetComponent<TouchInput>()._Character = null;
        }

        if (secondstep == true)
        {
            GameObject.Find("Tutorial(Clone)").GetComponent<Animator>().SetBool("Tutorial3", true);
            thirdstep = true;
        }
        secondstep = true;

    }
}
