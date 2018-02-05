using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Script : MonoBehaviour
{

    public float ChargeLevel = new float();
    Vector2 ChargeDirection = new Vector2();
    public bool StandStill = false;
    public bool SceneChanging = true;
    public GameObject Particles;
    ParticleSystem ps;
    // Use this for initialization
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(1, 360));
        ps = Particles.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 LastPos = gameObject.transform.position;
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1.8f)
        {
            StandStill = true;
        }


        if (ChargeLevel <= 38f && StandStill)
        {
            ChargeLevel += 10 * Time.deltaTime;
            gameObject.transform.Rotate(new Vector3(0, 0, ChargeLevel / 2));
            // gameObject.GetComponent<Rigidbody2D>().AddTorque(ChargeLevel);

        }
        if (ChargeLevel >= 37)
        {
            StandStill = false;
            ChargeDirection = new Vector2(250, 70);
            gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(ChargeDirection, ForceMode2D.Impulse);
            ChargeLevel = 0;
            gameObject.transform.Rotate(new Vector3(0, 0, 30));

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

            var col = ps.colorOverLifetime;
            col.enabled = true;

            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

            col.color = grad;


            Instantiate(Particles, transform.position, Quaternion.identity);

            GameObject.Find("Interface").GetComponent<ManageUI>().SpawnBonus(gameObject.transform.position);
        }
    }
}
