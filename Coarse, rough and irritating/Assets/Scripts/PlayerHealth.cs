using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Entity
{
    //fixed values for the status effects

    [SerializeField]
    private PlayerController controller;

    //update heatlhbar when taking damage
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
    }
}
