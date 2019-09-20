using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Tomato : Enemy
{
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        m_Animator.SetTrigger("AttackEnemy");
    }
    public override void Move()
    {
        bool frontGround = CheckGround(out _);
        if (!frontGround)
        {
            m_Rigidbody.velocity = Vector2.zero;
            return;
        }

        Vector2 movement = Vector2.right * m_TargetDirection * moveSpeed;
        Debug.DrawRay(transform.position, movement);
        m_Rigidbody.AddForce(movement);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_IsPlayerFound = true;
            m_PlayerTransform = collision.transform;
        }
    }
    public override void TakeDamage(float damage)
    {
        m_Animator.SetTrigger("HitEnemy");
        base.TakeDamage(damage);
    }
}
