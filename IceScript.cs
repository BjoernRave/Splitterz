using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : MonoBehaviour
{

    // Use this for initialization
    public static bool Icedropped = false;
    bool iceable = false;
    GameObject Enemy;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Icedropped)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100, ForceMode2D.Impulse);
            Icedropped = false;
            iceable = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && iceable)
        {
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0);
            Icedropped = false;
            gameObject.GetComponent<Animator>().SetBool("Freeze", true);
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Enemy = other.gameObject;
            iceable = false;
            gameObject.transform.position = Enemy.transform.position;
            Enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if (other.GetComponent<Enemy3Script>() != null)
            {
                other.GetComponent<Enemy3Script>().enabled = false;
            }


        }
    }
    public void Unfreeze()
    {
        if (Enemy != null)
        {
            Enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            Enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 10));
            if (Enemy.GetComponent<Enemy3Script>() != null)
            {
                Enemy.GetComponent<Enemy3Script>().enabled = true;
            }
        }

        GetComponent<Animator>().SetBool("Unfreeze", true);
        Destroy(gameObject, 1.1f);

    }
}
