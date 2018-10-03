using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy4Script : MonoBehaviour
{
    public bool SceneChanging = true;
    public float Charge;
    Rigidbody2D rb;
    public GameObject Particles;
    ParticleSystem ps;
    public GameObject BonusPoints;

    // Use this for initialization
    void Start()
    {
        ps = Particles.GetComponent<ParticleSystem>();
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(1, 360));
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(10, 10));
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            rb.AddRelativeForce(new Vector2(1, 1), ForceMode2D.Impulse);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Charge < 8)
        {
            Charge += 8 * Time.deltaTime;
        }
        if (Charge > 8)
        {
            Charge = 0;

            rb.AddForce(rb.velocity.normalized * 24f, ForceMode2D.Impulse);
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
            PersistentManagerScript.Instance.musicsource.PlayOneShot(PersistentManagerScript.Instance.EnemieKilled);
            var col = ps.colorOverLifetime;
            col.enabled = true;
            PlayerPrefs.SetInt("EnemiesKilled", PlayerPrefs.GetInt("EnemiesKilled") + 1);
            PersistentManagerScript.Instance.CheckAchievements();
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

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
