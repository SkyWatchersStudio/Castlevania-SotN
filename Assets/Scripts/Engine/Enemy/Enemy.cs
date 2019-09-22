using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Characters
{
    public float collisionForce;
    [Space(10)]
    public float maxDistance;
    [Space(10)]
    public int experiencePoint = 10;

    protected Vector2 m_TargetDirection;
    protected Transform m_PlayerTransform;
    protected bool m_IsPlayerFound;

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            AttackPlayer();
    }

    public void AttackPlayer()
    {
        Player.m_Instance.TakeDamage(attackDamage);

        var attackDirection = m_TargetDirection.normalized;
        attackDirection.y *= collisionForce * .25f;
        attackDirection.x *= collisionForce * .75f;

        var rigid = Player.m_Instance.GetComponent<Rigidbody2D>();
        rigid.AddForce(
            attackDirection, ForceMode2D.Impulse); //push player back
    }

    public virtual Vector2 GetPlayerDirection()
    {
        var deltaPosition = m_PlayerTransform.position - transform.position;

        if (deltaPosition.magnitude > maxDistance)
        {
            m_IsPlayerFound = false;
            m_PlayerTransform = null;
        }

        return deltaPosition.normalized;
    }

    public override void FixedUpdate()
    {
        if (!m_IsPlayerFound)
            return;

        m_TargetDirection = GetPlayerDirection();
        Flip(m_TargetDirection.x); //Flip Enemy if needed

        base.FixedUpdate();
    }
    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            GameManager.ExperiencePoint += experiencePoint;
        }
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (!m_IsPlayerFound)
            return;
        Gizmos.DrawRay(transform.position, m_TargetDirection * maxDistance);
    }
#endif
}
