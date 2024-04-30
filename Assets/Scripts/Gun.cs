using UnityEngine;
using TMPro;
public class Gun : MonoBehaviour
{
    //bullet
    public GameObject bullet;
    public GameObject missile;

    public Transform airsupport;
    //bullet force
    public float shootForce, upwardForce;

    //Gun states
    public float spread, reloadTime, timeBetweenShots,timeBetweenAirSupport;
    public int magazineSize, bulletsPerTap,airSupportNum;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //Recoil
   // public float recoilForce;

    //bools
    bool isShooting, isReadyToShoot, isReloading,isReadyToAirSupport;

    //reference
    public Camera fpsCam;
    public Transform attackPoint;

    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI ariSupportDisplay;
    public LineRenderer lineRenderer;

    public bool allowInvoke = true;
    public bool allowInvokeAirSupport = true;

    private  Vector3 targetPoint;
    
    private void Awake()
    {
        bulletsLeft = magazineSize;
        isReadyToShoot = true;
        isReadyToAirSupport = true;
    }

    private void Update()
    {
        MyInput();
        if(fpsCam.targetDisplay==0)
        {
            Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(75);
            }
            lineRenderer.SetPosition(0, attackPoint.position-attackPoint.forward*5-attackPoint.right*1);
            lineRenderer.SetPosition(1, targetPoint);
        }
        else
        {
            lineRenderer.SetPosition(0,new Vector3(0,0,0));
            lineRenderer.SetPosition(1, new Vector3(0, 0, 0));
        }
        if(ammunitionDisplay!=null)
        {
            ammunitionDisplay.SetText("Bullets: "+bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
        if(ariSupportDisplay!=null)
        {
            ariSupportDisplay.SetText("Air Support: " + airSupportNum);
        }
    }

    private void MyInput()
    {
        if(allowButtonHold)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        //Mark position for air support
        if(isReadyToAirSupport && Input.GetKeyDown(KeyCode.Mouse1) && airSupportNum>0)
        {
            MarkPosition();
        }

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft<magazineSize && !isReloading)
        {
            Reload();
        }
        if(isReadyToShoot && isShooting && !isReloading &&bulletsLeft<=0)
        {
            Reload();
        }

        if(isReadyToShoot && isShooting && !isReloading && bulletsLeft>0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        isReadyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
       // currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

     
        if(muzzleFlash!=null)
        {
            GameObject particle=Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            Destroy(particle, 1);
        }
        
        bulletsLeft--;
        bulletsShot++;

        AudioSource machineGun = this.GetComponent<AudioSource>();
        if (!machineGun.isPlaying)
        {
            machineGun.Play();
        }
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShots);
            allowInvoke = false;
        }
    }
    
    private void ResetShot()
    {
        isReadyToShoot = true;
        allowInvoke = true;
    }

    private void ResetAirSupport()
    {
        isReadyToAirSupport= true;
        allowInvokeAirSupport = true;
    }


    private void Reload()
    {
        isReloading = true;
        Invoke("ReloadFinished", reloadTime);
    }


    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void MarkPosition()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            isReadyToAirSupport = false;
            targetPoint = hit.point;
            Vector3 missileDir = targetPoint - airsupport.position;
            for (int i = 0; i < 5; i++)
            {
                Invoke("shootMissile",0+i*0.1f);
            }
            airSupportNum--;
            if (allowInvokeAirSupport)
            {
                Invoke("ResetAirSupport",timeBetweenAirSupport);
                allowInvokeAirSupport = false;
            }
        }
        else //mark in the air
        {
            return;
        }
    }

    private void shootMissile()
    {
        GameObject currentMissile;
        Vector3 missileDir = targetPoint - airsupport.position;
        float x = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);
        Vector3 spreadPos = new Vector3(airsupport.position.x + x, airsupport.position.y, airsupport.position.z + z);
        currentMissile = Instantiate(missile, spreadPos, Quaternion.identity);
        currentMissile.transform.forward = missileDir.normalized;
        currentMissile.GetComponent<Rigidbody>().AddForce(missileDir.normalized * shootForce, ForceMode.Impulse);
    }
}
