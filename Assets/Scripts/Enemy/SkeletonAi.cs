using UnityEngine;

public class SkeletonAi : EnemyBase
{
    public Animator anim;

    public float enemyAttack = 20;
    
    protected override void Update()
    {
        base.Update();
        
        if (CurrentState != EnemyState.Idle)
        {           
            Vector3 rot = transform.eulerAngles;
            transform.LookAt(agent.steeringTarget);
            transform.eulerAngles = new Vector3(rot.x, transform.eulerAngles.y, rot.z);            
        }

    }

    protected override void ManageStateChange(EnemyState oldState, EnemyState newState)
    {
        base.ManageStateChange(oldState, newState);
        if (newState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);

        }
        else if (newState == EnemyState.Chasing)
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);

        }
        else if (newState == EnemyState.Patroling)
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);

        }
    }
    public override void Attack()
    {


        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);
       //Idk if this is a good way of damaging
        DealDamage();
    }

    
    public override void DealDamage()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, AttackRange, WhatCanThisEnemyAttack);
        if (cols.Length <= 0)
            return;
        foreach (Collider col in cols)
        {
            if (col.transform == this.transform)
                continue;

            if (!PlayerCombat.isBlock)
            {
                col.gameObject.GetComponentInParent<Health>().Damage(enemyAttack);
            }
        }
    }
}
