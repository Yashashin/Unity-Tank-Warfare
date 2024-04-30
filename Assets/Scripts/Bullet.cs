using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem explosionPrefab;
    public LayerMask whatIsEnemies;

    [Range(0f, 1f)]
    public float bounciness;

    public int damage;
    public bool isExplodeRange;
    public float explosionRange;

    public float maxLifetime;
    public bool isExplodeOnTouch = true;

    PhysicMaterial physics_mat;

    public AudioClip explo;

    private void Update()
    {
        maxLifetime -= Time.deltaTime;
        if(maxLifetime<=0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        GetComponent<BoxCollider>().material = physics_mat;
    }

    private void Explode()
    {
        ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.Play();
        GetComponent<AudioSource>().Play();
        GetComponent<MeshRenderer>().enabled = false;
       
    }


    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.collider.CompareTag("bullet"))
        {
            return;
        }
        else if(collision.collider.CompareTag("enemy") && isExplodeOnTouch)
        {   
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                collision.collider.gameObject.GetComponent<EnemyTakeDamagedPart>().TakeDamage(damage);
            this.GetComponent<BoxCollider>().enabled = false;
            Explode();
        }
        else if(collision.collider.CompareTag("player") && isExplodeOnTouch)
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
           collision.collider.gameObject.GetComponent<PlayerTakeDamagedPart>().TakeDamage(damage);
            this.GetComponent<BoxCollider>().enabled = false;
            Explode();
        }
        else //other collider
        {
            
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            if (isExplodeRange)
            {
                Collider[] part = Physics.OverlapSphere(transform.position, explosionRange);
                for(int i=0;i<part.Length;i++)
                {
                    if(part[i].CompareTag("enemy"))
                    {
                        part[i].gameObject.GetComponent<EnemyTakeDamagedPart>().TakeDamage(damage);
                        break;
                    }
                }
         
            }
            this.GetComponent<BoxCollider>().enabled = false;
            Explode();
           
        }
        Debug.Log(collision.collider.gameObject.name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
