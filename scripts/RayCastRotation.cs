using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RayCastRotation : MonoBehaviour
{
    Ray2D RayUp;
    Ray2D RayDown;
    Ray2D RayLeft;
    Ray2D RayRight;
    public RaycastHit2D WallUp;
    public RaycastHit2D WallDown;
    public RaycastHit2D WallLeft;
    public RaycastHit2D WallRight;
    public LayerMask WallFilter;
    public float distance = 400;
    public static Axis LastAxis;
    Axis shortestAxis;
    public static float HorizontalDistance = new float();
    public static float VerticalDistance = new float();
    public LayerMask PossibleWalls;
    public static bool TooClosetoWall = false;
    int counter = 0;
    public GameObject Particles;
    ParticleSystem ps;

    void Start()
    {

        gameObject.GetComponent<SpriteRenderer>().color = color.ColorPalette[UnityEngine.Random.Range(0, 11)];
        ps = Particles.GetComponent<ParticleSystem>();
    }


    void Update()
    {
        RayUp = new Ray2D(transform.position, Vector2.up);
        RayDown = new Ray2D(transform.position, Vector2.down);
        RayLeft = new Ray2D(transform.position, Vector2.left);
        RayRight = new Ray2D(transform.position, Vector2.right);



        LastAxis = shortestAxis;

        WallUp = Physics2D.BoxCast(RayUp.origin, new Vector2(1.5f, 1.5f), 0, RayUp.direction, distance, PossibleWalls);
        WallDown = Physics2D.BoxCast(RayDown.origin, new Vector2(1.5f, 1.5f), 0, RayDown.direction, distance, PossibleWalls);
        WallLeft = Physics2D.BoxCast(RayLeft.origin, new Vector2(1.5f, 1.5f), 0, RayLeft.direction, distance, PossibleWalls);
        WallRight = Physics2D.BoxCast(RayRight.origin, new Vector2(1.5f, 1.5f), 0, RayRight.direction, distance, PossibleWalls);


        if (WallUp && WallDown && WallLeft && WallRight)
        {
            HorizontalDistance = Math.Abs(WallUp.point.y - WallDown.point.y);
            VerticalDistance = Math.Abs(WallRight.point.x - WallLeft.point.x);

            if (HorizontalDistance < VerticalDistance)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                shortestAxis = Axis.Vertical;
            }
            else if (HorizontalDistance > VerticalDistance)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                shortestAxis = Axis.Horizontal;
            }
        }
        else
        {
            shortestAxis = Axis.NoAxis;
        }


        if (counter == 3)
        {
            TouchInput._blnCanSpawnWalls = true;
        }
        if (counter < 3)
        {
            counter += 1;
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mask")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (!ManageWalls.AssignmentComplete && TouchInput._blnHasDropped)
            {
                var col = ps.colorOverLifetime;
                col.enabled = true;

                Gradient grad = new Gradient();
                grad.SetKeys(new GradientColorKey[] { new GradientColorKey(GetComponent<SpriteRenderer>().color, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

                col.color = grad;

                Instantiate(Particles, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(GameObject.Find("WallModul(Clone)"));
                Destroy(GameObject.Find("PreviewHoriz(Clone)"));
                GameObject.Find("Interface").GetComponent<ManageUI>().SubtractLife();
            }
        }

        if (collision.tag == "LeftWalls" || collision.tag == "RightWalls" || collision.tag == "TopWalls" || collision.tag == "DownWalls")
        {
            TooClosetoWall = true;
            if (gameObject.layer != 4)
            {
                Destroy(gameObject);
                Destroy(GameObject.Find("PreviewHoriz(Clone)"));
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag == "LeftWalls" || collision.tag == "RightWalls" || collision.tag == "TopWalls" || collision.tag == "DownWalls"))
        {

            TooClosetoWall = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Mask")
        {
            Destroy(gameObject);
        }
    }
}
public enum Axis
{
    Horizontal = 0,
    Vertical = 1,
    NoAxis = 2
}
