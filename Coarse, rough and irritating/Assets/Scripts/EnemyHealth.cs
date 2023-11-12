using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Entity
{

    //display vistory screen on death
    public override void EntityDeath()
    {
        Destroy(this.gameObject, 2);
    }

    //update heatlhbar when taking damage
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }
}
