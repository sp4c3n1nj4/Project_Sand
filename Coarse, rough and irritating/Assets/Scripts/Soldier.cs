using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum MovementState 
{ 
    follow,
    stay,
    defend
}

public enum AttackState 
{ 
    hold,
    search,
    attack
}

public enum TargetState
{   
    target,
    close,
    far,
    weak,
    strong
}

public class Soldier : Entity
{
    public bool controlled;

    public float attackRange;
    public float leashRange;

    private MovementState movementState = MovementState.stay;
    private AttackState attackState = AttackState.attack;
    private TargetState targetState = TargetState.close;

    private GameObject defendObjective;
    private Vector2 pathfindingTarget;
    private GameObject attackTarget;

    private PlayerController playerController;
    [SerializeField]
    private GameObject caravan;

    public void Awake()
    {
        playerController = GameObject.FindFirstObjectByType<PlayerController>();
        ChangeMovementState(MovementState.defend, caravan);
    }

    public override void Update()
    {
        base.Update();

        if (!controlled)
        {
            AIAttack();
            AIMovement();
            MoveTowards(pathfindingTarget);
            ShootAt(attackTarget);
        }
    }

    public void ChangeMovementState(MovementState state, GameObject objective = null)
    {
        movementState = state;
        switch (state)
        {
            case MovementState.defend:
                defendObjective = objective;
                break;
            default: break;
        }
    }

    private void AIMovement()
    {
        switch (movementState)
        {
            case MovementState.follow:
                if (playerController.pawnSelected)
                {
                    if ((gameObject.transform.position - playerController.pawn.transform.position).magnitude > leashRange)
                    {
                        pathfindingTarget = playerController.pawn.transform.position;
                    }
                }
                else
                {
                    ChangeMovementState(MovementState.defend, caravan);
                }
                break;
            case MovementState.stay:
                break;
            case MovementState.defend:
                if ((gameObject.transform.position - defendObjective.transform.position).magnitude > leashRange)
                {
                    pathfindingTarget = defendObjective.transform.position;
                }
                break;
        }
    }

    private void MoveTowards(Vector2 position)
    {
        //replace with pathfinding
        Vector3 direction = gameObject.transform.position;
        direction.x -= position.x;
        direction.y -= position.y;

        gameObject.transform.Translate(direction);
    }

    private void AIAttack()
    {
        GameObject target;
        switch (attackState)
        {
            case AttackState.hold:
                return;
            case AttackState.search:
                target = FindAttackTarget(attackRange * 2f);
                if (target == null)
                    return;

                if (Vector3.Distance(target.transform.position, gameObject.transform.position) > attackRange)
                {
                    pathfindingTarget = target.transform.position;
                }
                else
                {
                    attackTarget = target;
                }
                break;
            case AttackState.attack:
                target = FindAttackTarget(attackRange);
                if (target != null)
                {
                    attackTarget = target;
                }            
                break;
        }
    }

    private GameObject FindAttackTarget(float range)
    {
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Hostile"));

        GameObject[] enemiesArray = enemies.ToArray();
        for (int i = 0; i < enemiesArray.Length; i++)
        {
            if ((enemies[i].gameObject.transform.position - gameObject.transform.position).magnitude > range)
            {
                enemies.Remove(enemies[i]);
            }
        }

        GameObject target = null;
        float criteria = float.NegativeInfinity;
        for (int i = 0; i < enemies.Count; i++)
        {
            float compare = 0;
            switch (targetState)
            {
                case TargetState.close:
                    compare = -(gameObject.transform.position - enemies[i].transform.position).magnitude;
                    break;
                case TargetState.far:
                    compare = (gameObject.transform.position - enemies[i].transform.position).magnitude;
                    break;
                case TargetState.weak:
                    compare = -enemies[i].GetComponent<Entity>().health;
                    break;
                case TargetState.strong:
                    compare = enemies[i].GetComponent<Entity>().health;
                    break;
            }

            if (compare > criteria)
            {
                criteria = compare;
                target = enemies[i];
            }
        }
        return target;
    }

    private void ShootAt(GameObject target)
    {
        throw new NotImplementedException();
    }
}
