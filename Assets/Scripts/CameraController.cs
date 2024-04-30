using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float x, y;
    public float sensitivity,distance;
    public Vector2 xminmax;
    public Transform target;
    public GameObject turret;
    public GameObject barrel;
    private void LateUpdate()
    {
        x += Input.GetAxis("Mouse Y") * sensitivity * -1;
        y += Input.GetAxis("Mouse X") * sensitivity;
        x = Mathf.Clamp(x, xminmax.x, xminmax.y);
        transform.eulerAngles = new Vector3(x, y , 0);
        turret.transform.eulerAngles = new Vector3(turret.transform.eulerAngles.x, y, turret.transform.eulerAngles.z);
       // barrel.transform.eulerAngles = new Vector3(x + 90, barrel.transform.eulerAngles.y, barrel.transform.eulerAngles.z);

        transform.position = target.position - transform.forward * distance;
    }

    private void OnEnable()
    {
      //  transform.eulerAngles = new Vector3(0, 0, 0);
       // turret.transform.eulerAngles = new Vector3(-90,0,0);
    }
}
