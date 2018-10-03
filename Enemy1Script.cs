using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Enemy1Script : MonoBehaviour
{
    public bool SceneChanging = true;
    public GameObject Particles;
    ParticleSystem ps;
    Rigidbody2D rb;
    public bool Destroyobject = false;

    public GameObject BonusPoints;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(1, 360));
        ps = Particles.GetComponent<ParticleSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(10, 10));

    }
    private void FixedUpdate()
    {
        if (Destroyobject)
        {
            Destroy(gameObject);
        }
        if (transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(1, 1), ForceMode2D.Force);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        rb.velocity = 21 * (rb.velocity.normalized);


    }
    void OnApplicationQuit()
    {
        SceneChanging = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.GetComponent<Animator>().SetTrigger("collided");
    }

    private void OnDestroy()
    {
        if (SceneChanging)
        {
            PersistentManagerScript.Instance.musicsource.PlayOneShot(PersistentManagerScript.Instance.EnemieKilled);
            PlayerPrefs.SetInt("EnemiesKilled", PlayerPrefs.GetInt("EnemiesKilled") + 1);
            PersistentManagerScript.Instance.CheckAchievements();
            var col = ps.colorOverLifetime;
            col.enabled = true;

            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

            col.color = grad;



            Instantiate(Particles, transform.position, Quaternion.identity);
            ManageUI Interfacscript = GameObject.Find("Interface").GetComponent<ManageUI>();
            Interfacscript.SpawnBonus(gameObject.transform.position);

            if (!PersistentManagerScript.Instance.NormalMode)
            {
                GameObject bonus = Instantiate(BonusPoints, gameObject.transform.position, Quaternion.identity);
                bonus.GetComponent<MeshRenderer>().sortingOrder = 500;
                PersistentManagerScript.Instance.Points += 400;
                string Points = "Score " + Convert.ToString(PersistentManagerScript.Instance.Points);
                Interfacscript.PointBanner.text = Points;
                Interfacscript.ScoreOnPause.text = Points;
            }




        }
    }
}


