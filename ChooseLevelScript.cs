using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class ChooseLevelScript : MonoBehaviour
{

public bool Side1 = true;
    public void ReturntoHome()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void SwitchScreen(){
        GetComponent<Animator>().SetTrigger("Switch");
    }

    public void CurrentSide(){
        Side1 = !Side1;
    }

    public void LoadLevelX(GameObject button)
    {
        if (button.GetComponent<UpdateNumber>().LevelLoadable)
        {
            int level = Convert.ToInt32(button.name);
            PersistentManagerScript.Instance.CurrentSceneNumber = level;
            SceneManager.LoadScene(level);
        }
    }

}
