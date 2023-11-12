using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Entity
{
    public bool controlled;

    public override void Update()
    {
        base.Update();

        if (!controlled)
        {
            AIMovement();
        }
    }

    private void AIMovement()
    {
        throw new NotImplementedException();
    }
}
