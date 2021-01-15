using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackPoint = 20;
    public static bool isBlock;
    public bool canAttack = true;
    public float attackCooldown;
    
    public Animator anim;

    void Update()
    {
        playerAttack();
        playerBlock();
    }

    void playerAttack()
    {
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
    }

    void playerBlock()
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
}
