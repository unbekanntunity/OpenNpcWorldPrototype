using System.Collections; 
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{    
    public Animator anim;
    public AttackDefinition Attack;
    public CharacterStats stats;

    void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public void AttackTarget(GameObject target)
    {
        if (stats.GetWeapon() != null)
        {
            stats.GetWeapon().ExecuteAttack(gameObject, target);
        }
        else 
        {
            var attack = Attack.CreateAttack(stats, target.GetComponent<CharacterStats>());

            var attackables = target.GetComponentsInChildren(typeof(IAttackable));

            foreach (IAttackable attackable in attackables)
            {
                attackable.OnAttack(gameObject, attack);
            }
        }
    }

   /* void playerBlock()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isBlock = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            isBlock = false;
        }
    }
    
    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        anim.SetBool("isAttacking", false);
    }
   */
}
