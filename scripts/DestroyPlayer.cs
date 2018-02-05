using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayer : MonoBehaviour
{
    public GameObject Particles;


    private void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !ManageWalls.AssignmentComplete && TouchInput._blnCanSpawnWalls)
        {
            Instantiate(Particles, GameObject.Find("Player(Clone)").transform.position, Quaternion.identity);
            Destroy(GameObject.Find("Player(Clone)"));
            Destroy(GameObject.Find("WallModul(Clone)"));
            Destroy(GameObject.Find("PreviewHoriz(Clone)"));
            GameObject.Find("Interface").GetComponent<ManageUI>().SubtractLife();




        }

    }

}
