using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Tomato : Enemy
{
    private CapsuleCollider2D m_TriggerCollider;

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
        m_Rigidbody.AddForce(movement);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_IsPlayerFound = true;
            m_TriggerCollider.enabled = false;
            m_PlayerTransform = collision.transform;
        }
    }
    public override void TakeDamage(float damage)
    {
        m_Animator.SetTrigger("HitEnemy");
        base.TakeDamage(damage);
    }
    public override void Start()
    {
        base.Start();

        var colliders = GetComponents<CapsuleCollider2D>();
        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
                m_TriggerCollider = collider;
        }
    }
}
