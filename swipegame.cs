using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class swipegame : MonoBehaviour
{

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    public static GameObject Leftwall;
    public static GameObject RightWall;
    public static GameObject TopWall;
    public static GameObject DownWall;
    public LayerMask PossibleWalls;

    public static GameObject _Character;

    public GameObject _Characterprefab;
    public GameObject Wallmodul;

    public static bool Horizontal;

    public static float swipeheight;

    public GameObject _Previewprefab;
    public static GameObject _preview;
    float lerpamount;
    public float lerpspeed = 6;

    public LayerMask touchInputMask;

    public GameObject BombObj;
    public GameObject MissileObj;
    public GameObject IceObj;
    public static GameObject _Bomb;
    public static GameObject _Missile;
    public static GameObject _Ice;
    public static GameObject WallModul;
    public float distancebonus;

    public static bool Playerdropped = false;

    public static bool Bomb = false;
    public static bool Missile = false;
    public static bool Ice = false;
    float timetouchbegan;
    float timetouchend;

    public Image DeleteZone;
    Color Active;
    Color Notactive;
    float loopin;
    float loopout;

    void Start()
    {
        Active = Color.red;
        Notactive = Color.red;
        Notactive.a = 0;
    }
    void Update()
    {



        if (lp.y > 250 && _Character != null)
        {
            loopout = 0;
            DeleteZone.color = Color.Lerp(Notactive, Active, loopin);
            loopin += 0.08f * lp.y / 306;
        }
        else if (lp.y < 250 && _Character != null && loopin > 0.04)
        {
            loopin = 0.05f;
            // Color Newcolor = DeleteZone.GetComponent<Image>().color;
            // Newcolor.a -= 10 * Time.deltaTime;
            DeleteZone.color = Color.Lerp(Active, Notactive, loopout);
            loopout += 0.08f * lp.y / 306;
        }



        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchposition = Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D hitUI = Physics2D.Raycast(touchposition, Vector2.zero, touchInputMask);
            if (hitUI.collider != null)
            {
                PowerUps powerUps = hitUI.transform.gameObject.GetComponent<PowerUps>();

                if (powerUps != null)
                {
                    powerUps.ActivatePowerUP();
                    return;
                }
            }
            // get the touch
            if (!Playerdropped)
            {
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touchposition;
                    lp = fp;
                    timetouchbegan = Time.timeSinceLevelLoad;

                    WallstoFill.LeftCollided = false;
                    WallstoFill.RightCollided = false;

                    if (Bomb)
                    {
                        _Bomb = Instantiate(BombObj, new Vector2(lp.x, lp.y + 5), Quaternion.identity);
                    }
                    else if (Missile)
                    {
                        _Missile = Instantiate(MissileObj, new Vector2(lp.x, lp.y + 5), Quaternion.identity);
                    }
                    else if (Ice)
                    {
                        _Ice = Instantiate(IceObj, new Vector2(lp.x, lp.y + 5), Quaternion.identity);
                    }

                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touchposition;
                    float fDeltaX = Math.Abs(fp.x - lp.x);
                    float fDeltaY = Math.Abs(fp.y - lp.y);
                    if (Bomb)
                    {
                        if (_Bomb != null)
                        {
                            _Bomb.transform.position = new Vector2(lp.x, lp.y + 5);
                        }
                    }
                    else if (Missile)
                    {
                        if (_Missile != null)
                        {
                            _Missile.transform.position = new Vector2(lp.x, lp.y + 5);
                        }
                    }
                    else if (Ice)
                    {
                        if (_Ice != null)
                        {
                            _Ice.transform.position = new Vector2(lp.x, lp.y + 5);
                        }
                    }
                    else if (fDeltaX > 2 || fDeltaY > 2)
                    {
                        if (_Character == null)
                        {

                            RaycastHit2D[] hitLR = Physics2D.BoxCastAll(new Vector2(200, fp.y), new Vector2(1.5f, 1.5f), 0, Vector2.right, 250, PossibleWalls);

                            foreach (RaycastHit2D wall in hitLR)
                            {
                                if (string.Equals(wall.transform.tag, "LeftWalls", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Leftwall == null)
                                    {
                                        Leftwall = wall.transform.gameObject;
                                    }
                                    else
                                    {
                                        if (touch.position.x - wall.transform.position.x < touch.position.x - Leftwall.transform.position.x)
                                        {
                                            Leftwall = wall.transform.gameObject;
                                        }
                                    }
                                }
                                else if (string.Equals(wall.transform.tag, "RightWalls", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (RightWall == null)
                                    {
                                        RightWall = wall.transform.gameObject;
                                    }
                                    else
                                    {
                                        if (wall.transform.position.x - touch.position.x < RightWall.transform.position.x - touch.position.x)
                                        {
                                            RightWall = wall.transform.gameObject;
                                        }
                                    }

                                }
                            }


                            RaycastHit2D[] hitTD = Physics2D.BoxCastAll(new Vector2(fp.x, 300), new Vector2(1.5f, 1.5f), 0, Vector2.down, 250, PossibleWalls);
                            foreach (RaycastHit2D wall in hitTD)
                            {
                                if (string.Equals(wall.transform.tag, "TopWalls", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (TopWall == null)
                                    {
                                        TopWall = wall.transform.gameObject;
                                    }
                                    else
                                    {
                                        if (wall.transform.position.y - touch.position.y < TopWall.transform.position.y - touch.position.y)
                                        {
                                            TopWall = wall.transform.gameObject;
                                        }
                                    }
                                    TopWall = wall.transform.gameObject;
                                }
                                else if (string.Equals(wall.transform.tag, "DownWalls", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (DownWall == null)
                                    {
                                        DownWall = wall.transform.gameObject;
                                    }
                                    else
                                    {
                                        if (touch.position.y - wall.transform.position.y < touch.position.y - DownWall.transform.position.y)
                                        {
                                            DownWall = wall.transform.gameObject;
                                        }
                                    }
                                }

                            }
                        }


                        if (!Playerdropped && hitUI.collider == null)
                        {

                            if (fDeltaX > fDeltaY)
                            {
                                Horizontal = true;

                                lerpamount = fDeltaX;

                                if (_Character == null && _preview == null)
                                {
                                    Vector2 middleposition = new Vector2((Leftwall.transform.position.x + RightWall.transform.position.x) / 2, fp.y);
                                    _preview = Instantiate(_Previewprefab, middleposition, Quaternion.Euler(0, 0, 90));
                                    _Character = Instantiate(_Characterprefab, middleposition, Quaternion.identity);
                                }
                                _preview.transform.rotation = Quaternion.Euler(0, 0, 90);
                                _Character.transform.rotation = Quaternion.Euler(0, 0, 0);
                            }
                            else
                            {
                                Horizontal = false;

                                lerpamount = fDeltaY;

                                if (_Character == null && _preview == null)
                                {
                                    Vector2 middleposition = new Vector2(fp.x, (TopWall.transform.position.y + DownWall.transform.position.y) / 2);
                                    _preview = Instantiate(_Previewprefab, middleposition, Quaternion.identity);
                                    _Character = Instantiate(_Characterprefab, middleposition, Quaternion.Euler(0, 0, -90));
                                }
                                _preview.transform.rotation = Quaternion.Euler(0, 0, 0);
                                _Character.transform.rotation = Quaternion.Euler(0, 0, -90);
                            }

                            _preview.transform.localScale = new Vector3(0.8f, lerpamount, 0);
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    float fDeltaX = Math.Abs(fp.x - lp.x);
                    float fDeltaY = Math.Abs(fp.y - lp.y);
                    if (lp.y > 270)
                    {
                        Destroy(_Character);
                        Destroy(_preview);
                        swipegame.Playerdropped = false;
                        Leftwall = null;
                        RightWall = null;
                        TopWall = null;
                        DownWall = null;
                        Color NewColor2 = DeleteZone.color;
                        NewColor2.a = 0;
                        DeleteZone.color = NewColor2;
                        loopin = 0;
                        loopout = 0;
                    }
                    timetouchend = Time.timeSinceLevelLoad;
                    if (Bomb && _Bomb != null)
                    {
                        BombScript.Bombdropped = true;
                        Bomb = false;
                    }
                    else if (Missile && _Missile != null)
                    {
                        MissileScript.MissileDropped = true;
                        Missile = false;
                    }
                    else if (Ice && _Ice != null)
                    {
                        IceScript.Icedropped = true;
                        Ice = false;
                    }
                    if (_preview != null && (previewcheckLT.previewtouchwallLT && previewcheckRD.previewtouchwallRD || timetouchend - timetouchbegan < 0.3f && timetouchend - timetouchbegan > 0.12f))
                    {
                        StopWallLT.IsSplittableLT = true;
                        StopWallRD.IsSplittableRD = true;
                        ManageUI.Subtractable = true;

                        if (fDeltaX > fDeltaY)
                        {
                            _preview.transform.position = new Vector2((Leftwall.transform.position.x + RightWall.transform.position.x) / 2, _preview.transform.position.y);
                            _preview.transform.rotation = Quaternion.Euler(0, 0, 90);
                            _Character.transform.rotation = Quaternion.Euler(0, 0, 0);
                            _preview.transform.localScale = new Vector3(0.8f, (Leftwall.transform.position.x - RightWall.transform.position.x) / 7.4f, 0);
                            Horizontal = true;

                            Playerdropped = true;
                            Color PlayerColor = _Character.GetComponent<SpriteRenderer>().color;
                            PlayerColor.a = 1;
                            _Character.GetComponent<SpriteRenderer>().color = PlayerColor;
                            WallModul = Instantiate(Wallmodul, _Character.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            _preview.transform.position = new Vector2(_preview.transform.position.x, (TopWall.transform.position.y + DownWall.transform.position.y) / 2);
                            _preview.transform.rotation = Quaternion.Euler(0, 0, 0);
                            _Character.transform.rotation = Quaternion.Euler(0, 0, -90);
                            _preview.transform.localScale = new Vector3(0.8f, (TopWall.transform.position.y - DownWall.transform.position.y) / 7.4f, 0);
                            Horizontal = false;



                            Playerdropped = true;
                            Color PlayerColor = _Character.GetComponent<SpriteRenderer>().color;
                            PlayerColor.a = 1;
                            _Character.GetComponent<SpriteRenderer>().color = PlayerColor;
                            WallModul = Instantiate(Wallmodul, _Character.transform.position, Quaternion.Euler(0, 0, -90));
                        }

                    }
                    else
                    {
                        Destroy(_preview);
                        Destroy(_Character);
                        swipegame.Playerdropped = false;
                        Leftwall = null;
                        RightWall = null;
                        TopWall = null;
                        DownWall = null;
                    }

                }
            }
        }
    }
}