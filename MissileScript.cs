using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{

    public GameObject Explosion;
    public static bool MissileDropped = false;
    bool explodeable = false;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (MissileDropped)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 120, ForceMode2D.Impulse);
            GetComponent<ParticleSystem>().Play();
            explodeable = true;

            MissileDropped = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && explodeable)
        {
            PersistentManagerScript.Instance.musicsource.PlayOneShot(PersistentManagerScript.Instance.Explosion);
            GameObject Boom = Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
            Boom.transform.localScale = new Vector3(4, 4, 0);
            Destroy(other.gameObject);
            Destroy(gameObject);

        }
    }
}
