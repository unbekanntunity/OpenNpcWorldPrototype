using System.Collections; 
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{    
    public Animator anim;
    public AttackDefinition demoAttack;
    public CharacterStats stats;

    void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public void AttackTarget(GameObject target)
    {
        /*
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            anim.SetBool("isAttacking", true);
            StartCoroutine(AttackCooldown());
            
            if (Physics.Raycast(ray, out hit, 3))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<Health>().Damage(attackPoint);
                }
            }
        }
        */
        var attack = demoAttack.CreateAttack(stats, target.GetComponent<CharacterStats>());

        var attackables = target.GetComponentsInChildren(typeof(IAttackable));

        foreach (IAttackable attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
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
