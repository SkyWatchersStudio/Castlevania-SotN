using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public float attackForce = 15;

    public Transform attackPos;
    public float attackRange;

    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                m_Animator.SetTrigger("Attack");

                Collider2D[] enemiesToDamage = 
                    Physics2D.OverlapCircleAll(attackPos.position, attackRange);
                foreach (Collider2D enemy in enemiesToDamage)
                {
                    if (enemy.tag == "Enemy")
                    {
                        var rigidbody = enemy.GetComponent<Rigidbody2D>();
                        rigidbody.AddForce(Vector2.right * attackForce, ForceMode2D.Impulse);

                        enemy.SendMessage("TakeDamage");
                        break;
                    }
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
            timeBtwAttack -= Time.deltaTime;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
