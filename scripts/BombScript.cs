using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public bool SceneChanging = true;
    public static bool Bombdropped = false;
    public LayerMask BombableObhjects;
    Collider2D[] ObjectsinRange;
    bool lerp = false;
    public GameObject Explosion;

    float initializationTime;
    float timeSinceInitialization;

    void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;
    }



    // Update is called once per frame
    void Update()
    {
        if (timeSinceInitialization > 3)
        {
            ObjectsinRange = Physics2D.OverlapCircleAll(gameObject.transform.position, 20, BombableObhjects);
            foreach (Collider2D Enemy in ObjectsinRange)
            {
                Destroy(Enemy.gameObject);
            }
            GameObject.Find("Interface").GetComponent<TouchInput>()._Character = null;
            Destroy(gameObject);
        }



        if (Bombdropped)
        {
            GameObject.Find("fuse").GetComponent<ParticleSystem>().Play();
            lerp = true;
            Bombdropped = false;

        }
        if (lerp)
        {
            GameObject.Find("fuse").transform.position = Vector2.Lerp(GameObject.Find("fuse").transform.position, new Vector2(GameObject.Find("fuse").transform.position.x, GameObject.Find("fuse").transform.position.y - 1), Time.deltaTime * 0.5f);
            timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;
        }
    }
    void OnApplicationQuit()
    {
        SceneChanging = false;
    }

    private void OnDestroy()
    {
        if (SceneChanging)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, 20);
    }
}
