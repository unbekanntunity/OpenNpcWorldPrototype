using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackPoint = 20;
    public static bool isBlock;

    void Update()
    {
        playerAttack();
        playerBlock();
    }

    void playerAttack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
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
}
