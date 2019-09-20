using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Tomato : Enemy
{
    public float patrolRange = 4;

    private Vector3[] m_PatrolPositions = new Vector3[2];
    private bool m_PatrolSet; //is patrol positions found?
    private int m_CurrentTarget;

    public override void Start()
    {
        base.Start();
        GetPatrolPositions();
    }

    private void GetPatrolPositions()
    {
        m_PatrolSet = true;

        m_PatrolPositions[0] = transform.position;
        m_PatrolPositions[0].x += patrolRange;

        m_PatrolPositions[1] = transform.position;
        m_PatrolPositions[1].x -= patrolRange;
    }

    private void Patrol()
    {
        var targetPosition = m_PatrolPositions[m_CurrentTarget];
        Vector2 deltaPos = targetPosition - transform.position;

        Flip(deltaPos.normalized.x);

        m_Rigidbody.AddForce(Vector2.right * deltaPos.normalized * moveSpeed);

        float distance = deltaPos.magnitude;
        if (distance < .1f)
            m_CurrentTarget = (m_CurrentTarget + 1) % 2;
    }
    public override void FixedUpdate()
    {
        if (!m_IsPlayerFound)
        {
            if (!m_PatrolSet)
                GetPatrolPositions();

            Patrol();
            return;
        }

        m_TargetDirection = GetPlayerDirection();
        Flip(m_TargetDirection.x);

        Move();
    }
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
            m_PatrolSet = false;
        }
    }
    public override void TakeDamage(float damage)
    {
        m_Animator.SetTrigger("HitEnemy");
        base.TakeDamage(damage);
    }
}
