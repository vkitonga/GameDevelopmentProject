using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This script manages a health value and holds a damage function that removes health and checks for death
/// When damaging a character, always call the TakeDamage function. Do not manual reduce health
/// </summary>
public class Health : MonoBehaviour
{
    public float health;
    public bool dead = false;
    [SerializeField] private bool damageAble = true; //used for buffer times between taking damage
    [SerializeField] private float damageBuffer; //the amount of time after being damaged to wait before damage can be re-applied;

    public UnityEvent onDamage; //use to extend the TakeDamage functions uses
    public UnityEvent onDeath; //use to extend the Die functions uses

    /// <summary>
    /// Call this function to remove health from the character
    /// </summary>
    /// <param name="damageAmount"> the amount of damage to remove from a characters health value</param>
    public void TakeDamage(float damageAmount)
    {
        if (!damageAble) return; //do nothing if recently damaged
        if (dead) return; //do nothing if already dead

        health-= damageAmount;

        onDamage.Invoke();

        if(health <= 0) //check whether to kill the object
        {
            Die();
        }

        damageAble = false;
        Invoke("ResetDamageable", damageBuffer);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual void Die()
    {
        dead = true;
        onDeath.Invoke();
    }

    public void ResetDamageable()
    {
        damageAble = true;
    }
}
