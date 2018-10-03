using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTutorial : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 29);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        swipegame._Character = null;
        GameObject.Find("Enemy 1").GetComponent<Enemy1Script>().enabled = true;
    }
}
