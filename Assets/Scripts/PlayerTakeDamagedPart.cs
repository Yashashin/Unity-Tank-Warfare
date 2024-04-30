using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamagedPart : MonoBehaviour
{
    public TankController player;
    public void TakeDamage(int damage)
    {
        Debug.Log(name);
        player.TakeDamage(damage);
    }

   
}
