using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public GameObject bullet;

    public Transform attackPoint;

    public GameObject muzzleFlash;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float  walkPointRange;

    //Attacking
    public float timeBwtweenAttacks;
    bool alreadyAttacked;


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    
        if(!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }

        if(playerInSightRange && !playerInAttackRange)
        {
            Looking();
        }

        if(playerInAttackRange && playerInSightRange)
        {
            Attacking();
        }
    
    }

    private void Awake()
    {
        player = GameObject.Find("tank").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patroling()
    {
        if((!walkPointSet))
        {
            SearchWalkPoint();
        }
        if(walkPointSet)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(walkPoint);
            }
            else
            {
                agent.Warp(transform.position);
            }
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude<1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
    }   

    private void Attacking()
    {
        Patroling();
        Looking();
        if (!alreadyAttacked)
        {
            //agent.SetDestination(transform.position);

            //transform.LookAt(player);
            Vector3 lookdir = player.position - transform.position;
            //transform.Find("turret").gameObject.transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z), new Vector3(0, 1, 0));
            //transform.Find("turret").gameObject.transform.eulerAngles = transform.Find("turret").gameObject.transform.eulerAngles + new Vector3(-90, 0, 0);

            GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            currentBullet.transform.forward = lookdir.normalized;

            currentBullet.GetComponent<Rigidbody>().AddForce(lookdir.normalized * 200, ForceMode.Impulse);

            if (muzzleFlash != null)
            {
                GameObject particle = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
                Destroy(particle, 1);
            }

            AudioSource []audio = this.GetComponents<AudioSource>();
            if (!audio[1].isPlaying)
            {
               audio[1].Play();
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBwtweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void Looking()
    {
        Patroling();
        Vector3 lookdir = player.position - transform.position;
        transform.Find("turret").gameObject.transform.LookAt(new Vector3(player.position.x,transform.position.y,player.position.z), new Vector3(0, 1, 0));
        transform.Find("turret").gameObject.transform.eulerAngles = transform.Find("turret").gameObject.transform.eulerAngles + new Vector3(-90, 0, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
