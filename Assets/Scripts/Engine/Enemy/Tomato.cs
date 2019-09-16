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
        m_Rigidbody.AddForce(movement);
    }
    public override void TakeDamage()
    {
        base.TakeDamage();
        m_Animator.SetTrigger("HitEnemy");
    }
}
