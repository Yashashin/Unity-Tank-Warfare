using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    public GameObject DestroyedPrefab;
    public ParticleSystem explosionPrefab;
    public ParticleSystem smoke;
    public GameController gameController;
    public void Start()
    {
        health = 100;
    }
    public void TakeDamage(int damage)
    {
        this.health -= damage;
        if(health<50)
        {
            smoke.Play();
        }
      
        if (this.health<=0)
        {
            GetComponent<AudioSource>().Play();
           
            //Create destroyed tank
            ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explosion.Play();
            gameController.DestroyEnemy();
            Instantiate(DestroyedPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void Delay()
    {
        
    }
}
