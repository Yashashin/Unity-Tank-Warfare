using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamagedPart : MonoBehaviour
{
    public Enemy enemy;

   public void TakeDamage(int damage)
    {
        Debug.Log(this.name);
        enemy.TakeDamage(damage);
    } 
}
