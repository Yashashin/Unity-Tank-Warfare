using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Transform[] WheelMeshes;
    public WheelCollider[] WheelColls;

    public ParticleSystem dust;

    public ParticleSystem smoke;
    public GameObject DestroyedPrefab;
    public ParticleSystem explosionPrefab;

    public GameController gameOver;

    Vector3 pos,rotation;
    Quaternion quat;
    public float Force,RotSpeed;
    public GameObject tpsCAM;
    public GameObject fpsCAM;
    public GameObject tpsRainScript;
    public GameObject fpsRainScript;
    public GameObject lineRenderer;
    public UnityEngine.UI.Slider hpSlider;
    public Transform[] backMissiles;
    public GameObject bulletPrefab;
    public GameObject flashLight;

    public int health = 100;

    private bool isBackMissileShot = false;
    private void Start()
    {
        rotation = transform.eulerAngles;
        Cursor.visible = false;
        tpsCAM.SetActive(true);
        tpsRainScript.SetActive(true);
        fpsRainScript.SetActive(false);
        fpsCAM.GetComponent<Camera>().targetDisplay = 1;
        fpsCAM.GetComponent<AudioListener>().enabled = false;
        hpSlider.value = health / 100.0f;
        flashLight.SetActive(false);
    }
    private void Update()
    {
        transform.eulerAngles = rotation;
        for(int i=0;i<WheelColls.Length;i++)
        {
            WheelColls[i].GetWorldPose(out pos, out quat);
            WheelMeshes[i].rotation = quat;
            WheelMeshes[i].Rotate(new Vector3(0, -90, 0));
            if(i==WheelColls.Length-1) 
            {
                WheelMeshes[i+1].rotation = quat; //Gears
                WheelMeshes[i+1].Rotate(new Vector3(0, -90, 0));
                WheelMeshes[i+2].rotation = quat; //Gears
                WheelMeshes[i+2].Rotate(new Vector3(0, -90, 0));
                WheelMeshes[i+3].rotation = quat; //Gears
                WheelMeshes[i+3].Rotate(new Vector3(0, -90, 0));
                WheelMeshes[i+4].rotation = quat; //Gears
                WheelMeshes[i+4].Rotate(new Vector3(0, -90, 0));
            }
        }

        foreach(var wheelcols in WheelColls)
        {
            wheelcols.motorTorque = Input.GetAxis("Vertical") * Force*Time.deltaTime;
        }

        rotation.y += Input.GetAxis("Horizontal")*RotSpeed;

       if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > 0)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity - GetComponent<Rigidbody>().velocity.normalized * 0.01f;
            }
        }


        AudioSource moveSound = this.GetComponent<AudioSource>();
        if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.7)
        {
            while(moveSound.volume <1)
            {
                moveSound.volume += 0.001f*Time.deltaTime;
            }
            while (moveSound.pitch < 1)
            {
                moveSound.pitch += 0.001f * Time.deltaTime;
            }
            if (!dust.isPlaying)
                dust.Play();
        }
        else
        {
            moveSound.volume = 0.5f;
            moveSound.pitch = 0.75f;
            if (dust.isPlaying)
                dust.Stop();
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            tpsCAM.GetComponent<Camera>().targetDisplay = 0;
            tpsCAM.GetComponent<AudioListener>().enabled = true;
            tpsRainScript.SetActive(true);
            fpsRainScript.SetActive(false);
            fpsCAM.GetComponent<Camera>().targetDisplay=1;
            fpsCAM.GetComponent<AudioListener>().enabled = false;
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            tpsCAM.GetComponent<Camera>().targetDisplay = 1;
            tpsCAM.GetComponent<AudioListener>().enabled = false;
            tpsRainScript.SetActive(false);
            fpsRainScript.SetActive(true);
            fpsCAM.GetComponent<Camera>().targetDisplay = 0;
            fpsCAM.GetComponent<AudioListener>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lineRenderer.SetActive(!lineRenderer.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (!isBackMissileShot)
            {
                for (int i = 0; i < backMissiles.Length; i++)
                {
                    backMissiles[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                    GameObject currentBullet = Instantiate(bulletPrefab,backMissiles[i].position, Quaternion.identity);
                    currentBullet.transform.forward = backMissiles[i].forward.normalized;

                    currentBullet.GetComponent<Rigidbody>().AddForce(backMissiles[i].forward.normalized * 100, ForceMode.Impulse);
                }
                isBackMissileShot = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.SetActive(!flashLight.activeSelf);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("water"))
        {
            Debug.Log("enter water");
            TakeDamage(200);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        hpSlider.value = health / 100.0f;
        if (health < 100)
        {
            smoke.Play();
        }

        if (health < 0)
        {
            hpSlider.value = 0.0f;
            GetComponent<AudioSource>().Play();
            //Create destroyed tank
            ParticleSystem explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explosion.Play();
            Instantiate(DestroyedPrefab, transform.position, transform.rotation);

            //Game Over
            gameOver.GameOver();
            this.gameObject.SetActive(false);
        }

    }
}
