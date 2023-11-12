using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//parent script for all game entities, IEffecable interface to allow interaction with status effects
public class Entity : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public virtual void Update()
    {
        
    }

    //base behaviour of on death function
    public virtual void EntityDeath()
    {
        throw new System.NotImplementedException();
    }

    //base function of take damage function
    public virtual void TakeDamage(float _damage)
    {
        health -= _damage;

        if (health <= 0)
        {
            EntityDeath();
        }
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}

