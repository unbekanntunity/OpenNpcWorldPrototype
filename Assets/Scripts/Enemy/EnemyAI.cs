using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;

    public float lookRadius = 10f;
    public Transform player;

    static Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction * _speed * Time.deltaTime, this.transform.forward);
        if (Vector3.Distance(player.position, this.transform.position) < lookRadius && angle < 30)
        {

            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

            anim.SetBool("isIdle", false);
            if (direction.magnitude > 5)
            {
                this.transform.Translate(0, 0, 0.05f);
                anim.SetBool("isRunning", true);
                anim.SetBool("isAttacking", false);
            }
            else
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", true);
            }
        }
        else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", false);            
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
