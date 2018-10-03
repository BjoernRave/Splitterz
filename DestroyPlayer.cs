using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestroyPlayer : MonoBehaviour
{
    public GameObject Particles;


    private void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (string.Equals(collision.gameObject.tag, "Enemy", StringComparison.OrdinalIgnoreCase) && swipegame.WallModul != null)
        {
            Instantiate(Particles, swipegame._Character.transform.position, Quaternion.identity);
            Destroy(swipegame._Character);
            swipegame.Playerdropped = false;
            Destroy(swipegame.WallModul);
            Destroy(swipegame._preview);
            Destroy(swipegame._Ice);
            swipegame.Leftwall = null;
            swipegame.RightWall = null;
            swipegame.TopWall = null;
            swipegame.DownWall = null;
            GameObject.Find("Interface").GetComponent<ManageUI>().SubtractLife();

        }

    }

}
