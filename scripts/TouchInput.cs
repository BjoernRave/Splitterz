using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{

    public static bool _blnCanSpawnWalls = false;
    public static bool _blnHasDropped = true;
    public static bool Bomb = false;
    public GameObject _Character;
    public GameObject _CharacterPrefab;
    public GameObject _WallPrefab;
    public static Vector3 Playerposition;
    bool OverUI = false;
    public LayerMask touchInputMask;
    public GameObject PreviewHoriz;
    public GameObject BombObject;
    GameObject BombPowerUP;
    GameObject HorizPreview;
    bool FingerLifted = false;

    void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero, touchInputMask);
            if (hit.collider != null)
            {
                Debug.Log("Touched it");
                OverUI = true;
                if (hit.transform.gameObject.GetComponent<PowerUps>() != null)
                {
                    hit.transform.gameObject.GetComponent<PowerUps>().ActivatePowerUP();
                }
            }
            else
            {
                OverUI = false;
            }
        }

        if (!OverUI && _Character == null && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {

            if (Bomb)
            {
                BombPowerUP = Instantiate(BombObject, new Vector2(Playerposition.x, Playerposition.y + 5), Quaternion.identity);
                _Character = new GameObject();
                _blnCanSpawnWalls = true;
            }
            else
            {
                _Character = Instantiate(_CharacterPrefab, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Quaternion.identity);
                HorizPreview = Instantiate(PreviewHoriz, _Character.transform.position, Quaternion.identity);
                HorizPreview.transform.rotation = Quaternion.Euler(_Character.transform.rotation.eulerAngles + new Vector3(0, 0, 90));
                StopWallLT.IsSplittableLT = true;
                StopWallRD.IsSplittableRD = true;
                ManageUI.Subtractable = true;
            }

            FingerLifted = false;
            _blnHasDropped = false;
            Playerposition = _Character.transform.position;



        }

        // If character is spawned, can spawn walls and left mouse button is no longer pressed.
        if (_Character != null && _blnCanSpawnWalls && !_blnHasDropped && FingerLifted == true)
        {
            if (_Character != null && HorizPreview != null)
            {
                HorizPreview.transform.rotation = Quaternion.Euler(_Character.transform.rotation.eulerAngles + new Vector3(0, 0, 90));
            }
            if (Bomb)
            {
                BombScript.Bombdropped = true;

                Bomb = false;
            }
            else
            {
                Instantiate(_WallPrefab, _Character.transform.position, _Character.transform.rotation);
                GameObject.Find("PreviewHoriz(Clone)").layer = 10;
            }

            _blnCanSpawnWalls = false;
            _blnHasDropped = true;

        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            FingerLifted = true;
        }

        if (!OverUI && _Character != null && !_blnHasDropped)
        {
            // Update character position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Bomb)
            {
                BombPowerUP.transform.position = new Vector2(mousePosition.x, mousePosition.y + 5);
            }
            else
            {
                HorizPreview.transform.rotation = Quaternion.Euler(_Character.transform.rotation.eulerAngles + new Vector3(0, 0, 90));
                _Character.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }


            // HorizPreview.transform.position = _Character.transform.position;


        }

    }
}


