using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alucard : Enemy
{
    public float minDisatance;
    public float jumpDistance;
    public float jumpTimeDelay = .4f;

    PlayerCommonAbilities m_Abilities;
    Rigidbody2D m_PlayerRigid;
    bool m_MovePermission, m_ShouldJump;
    float m_LastHP, m_JumpDelayTime;

    public override void Start()
    {
        base.Start();

        m_Abilities = GetComponent<PlayerCommonAbilities>();
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_IsPlayerFound = true;
        m_PlayerRigid = m_PlayerTransform.GetComponent<Rigidbody2D>();

        m_LastHP = health;
    }
    public override void FixedUpdate()
    {
        m_Abilities.m_Grounded = CheckGround(out m_Abilities.m_GroundColliders);

        if (m_ShouldJump)
        {
            m_JumpDelayTime += Time.deltaTime;
            if (m_JumpDelayTime >= jumpTimeDelay)
            {
                m_JumpDelayTime = 0;
                m_Abilities.m_ShouldJump = true;
                m_ShouldJump = false;
            }
        }

        ChooseAction();

        m_Abilities.PhysicUpdate();

        if (m_Abilities.IsLock)
            return;

        if (m_MovePermission)
        {
            m_MovePermission = false;
            base.FixedUpdate();
        }
    }
    public override void Move() => m_Abilities.Move(m_TargetDirection.x);
    public override Vector2 GetPlayerDirection()
    {
        Vector2 deltaPosition = m_PlayerTransform.position - transform.position;
        return deltaPosition.normalized;
    }

    private void ChooseAction()
    {
        Collider2D inRange = CheckRange();
        if (inRange)
        {
            bool grounded = CheckGround(out _);
            if (grounded)
            {
                bool isComing = (m_PlayerRigid.velocity.x > 0 && !m_FacingRight) || 
                    (m_PlayerRigid.velocity.x < 0 && m_FacingRight);
                if (isComing)
                {
                    if (m_LastHP != health)
                    {
                        m_LastHP = health;
                        m_Abilities.m_Dodge = true;
                    }
                    else
                        m_Abilities.m_Attack = true;
                }
                else
                    m_Abilities.m_Attack = true;
            }
            else
                m_Abilities.m_Attack = true;
        }
        else
        {
            Vector2 deltaPosition = m_PlayerTransform.position - transform.position;
            if (deltaPosition.magnitude > maxDistance)
            {
                if (Mathf.Abs(deltaPosition.x) > minDisatance && Mathf.Abs(deltaPosition.y) > jumpDistance)
                {
                    m_ShouldJump = true;
                    m_Abilities.m_Dash = true;
                }
                else
                {
                    if (m_Abilities.m_PreviousDash)
                    {
                        m_Abilities.m_PreviousDash = false;
                        m_MovePermission = true;
                    }
                    else
                        m_Abilities.m_Dash = true;
                }
            }
            else
            {
                if (Mathf.Abs(deltaPosition.x) > minDisatance && Mathf.Abs(deltaPosition.y) > jumpDistance)
                {
                    m_MovePermission = true;
                    m_ShouldJump = true;
                }
                else
                {
                    if (Mathf.Abs(deltaPosition.x) > minDisatance)
                        m_MovePermission = true;
                    else if (Mathf.Abs(deltaPosition.y) > jumpDistance)
                        m_ShouldJump = true;
                }
            }
        }
    }

    public Collider2D CheckRange()
    {
        Collider2D[] results = new Collider2D[1];

        Physics2D.OverlapCircleNonAlloc(
            m_Abilities.attackPosition.position,
            m_Abilities.attackRange, results, m_Abilities.m_AttackLayer);

        if (results[0].CompareTag("Player"))
            return results[0];
        return null;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.right * minDisatance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector2.left * maxDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector2.up * jumpDistance);

        base.OnDrawGizmos();
    }
}
