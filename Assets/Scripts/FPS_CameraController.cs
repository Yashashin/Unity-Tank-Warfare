using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_CameraController : MonoBehaviour
{
    float x, y;
    public float sensitivity, distance;
    public Vector2 xminmax;
    public GameObject turret;
    public GameObject barrel;
    private void LateUpdate()
    {
        x += Input.GetAxis("Mouse Y") * sensitivity * -1;
        y += Input.GetAxis("Mouse X") * sensitivity;
        x = Mathf.Clamp(x, xminmax.x, xminmax.y);
        transform.eulerAngles = new Vector3(x, y, 0);
        turret.transform.eulerAngles = new Vector3(turret.transform.eulerAngles.x, y, turret.transform.eulerAngles.z);
        // barrel.transform.eulerAngles = new Vector3(x + 90, barrel.transform.eulerAngles.y, barrel.transform.eulerAngles.z);


    }
    
}