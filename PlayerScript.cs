using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{



    ParticleSystem ps;
    public GameObject Particles;
    // Use this for initialization
    void Start()
    {

        gameObject.GetComponent<SpriteRenderer>().color = color.ColorPalette[UnityEngine.Random.Range(0, 11)];
        Color NewColor = gameObject.GetComponent<SpriteRenderer>().color;
        NewColor.a = 0.6f;
        gameObject.GetComponent<SpriteRenderer>().color = NewColor;
        ps = Particles.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mask")
        {
            print("maskcollided");
            Destroy(gameObject);
            swipegame.Leftwall = null;
            swipegame.RightWall = null;
            swipegame.TopWall = null;
            swipegame.DownWall = null;
            Destroy(swipegame._preview);
        }

        if (collision.gameObject.tag == "Enemy" && swipegame.WallModul != null)
        {

            var col = ps.colorOverLifetime;
            col.enabled = true;

            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(GetComponent<SpriteRenderer>().color, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

            col.color = grad;

            Instantiate(Particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
            swipegame.Playerdropped = false;
            Destroy(swipegame.WallModul);
            Destroy(swipegame._preview);
            swipegame.Leftwall = null;
            swipegame.RightWall = null;
            swipegame.TopWall = null;
            swipegame.DownWall = null;
            GameObject.Find("Interface").GetComponent<ManageUI>().SubtractLife();

        }

        if (collision.tag == "LeftWalls" || collision.tag == "RightWalls" || collision.tag == "TopWalls" || collision.tag == "DownWalls")
        {

            if (gameObject.layer != 4 && swipegame.Playerdropped == false)
            {
                Destroy(gameObject);
                Destroy(swipegame._preview);
                Destroy(swipegame.WallModul);
                swipegame.Leftwall = null;
                swipegame.RightWall = null;
                swipegame.TopWall = null;
                swipegame.DownWall = null;
            }

        }
    }
    void OnDestroy()
    {
        swipegame.Playerdropped = false;
        GameObject.Find("Interface").GetComponent<ManageUI>().CheckLevelComletion();

    }


}
