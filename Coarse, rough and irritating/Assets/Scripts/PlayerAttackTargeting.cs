using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTargeting : MonoBehaviour
{
    public float minTargetDistance;
    public GameObject model;
    public CharacterController player;

    [SerializeField]
    private float dps;
    public float weak = 1;

    private float oldMovementRotation = 0;
    
    //find nearest target enemy 
    void Update()
    {
        TurnCharacter();
        PlayerAttack();
    }

    private void PlayerAttack()
    {
        GameObject target = FindClosestEnemy();

        if (target == null || player.velocity.magnitude > 0)
            return;

        target.GetComponent<EnemyHealth>().TakeDamage(dps * Time.deltaTime * weak);
    }

    private GameObject FindClosestEnemy()
    {
        //get all enemies end return closest game object
        GameObject target = null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        var distance = minTargetDistance;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(this.transform.position, enemies[i].transform.position) < distance)
                target = enemies[i];
        }

        return target;
    }

    private void TurnCharacter()
    {
        //turn character towards enemy or in movement direction
        float rotation = TargetEnemy(FindClosestEnemy());

        //slowly turn character towards intended rotation
        float lerpedRotation = Mathf.LerpAngle(rotation, player.transform.rotation.eulerAngles.y, 0.5f);

        player.transform.rotation = Quaternion.AngleAxis(lerpedRotation, Vector3.up);
    }

    
    private float TargetEnemy(GameObject target)
    {
        //if ther are no enemies nearby, player turns back towards movement direction
        if (target == null)
        {
            //calculate rotation of current movement direction
            if (Mathf.Abs(player.velocity.x) > 0 || Mathf.Abs(player.velocity.z) > 0)
            {
                oldMovementRotation = Mathf.Rad2Deg * Mathf.Atan2(player.velocity.x, player.velocity.z);
            }
            return oldMovementRotation;
        }
        else
        {
            //calculate rotation towards target
            Vector3 targetVector = target.transform.position - this.transform.position;
            return Mathf.Rad2Deg * Mathf.Atan2(targetVector.x, targetVector.z);
        }
    }

    private void OnDrawGizmos()
    {
        //debug gizmos to visualize targeting enemies
        GameObject target = FindClosestEnemy();

        if (target == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, target.transform.position);
    }
}
