using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommonAbilities : MonoBehaviour
{
    public float jumpForce;
    [Space(10)]
    public float attackForce;
    public float timeBetweenAttack;
    public Transform attackPosition;
    public float attackRange;
    public LayerMask m_AttackLayer = 1 << 11 | 1 << 8;
    [Space(10)]
    public float dashForce;
    public float timeBetweenDash;
    [Space(10)]
    public float dodgeForce;

    private Characters m_Base;
    private float m_TimeBtwAttack, m_TimeBtwDash;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    [System.NonSerialized]
    public int m_AttackCounts;

    [System.NonSerialized]
    public bool m_Grounded, m_Attack, m_Dash,
        m_Dodge, m_InterruptJump, m_ShouldJump, m_PreviousDash;
    [System.NonSerialized]
    public Collider2D[] m_GroundColliders;

    public bool IsLock { get; private set; }
    public bool IsJumping { get; private set; }

    enum WhichAnimation { dodge, dash}
    private WhichAnimation m_AnimDD;
    private int m_AttackID, m_SpeedID, m_IsGroundID;


    private void Awake()
    {
        m_Base = GetComponent<Characters>();
    }
    private void Start()
    {
        //override this cause animator component is different than base class thought
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

        //Animator parameter Ids
        m_AttackID = Animator.StringToHash("Attack");
        m_SpeedID = Animator.StringToHash("Speed");
        m_IsGroundID = Animator.StringToHash("isGround");
    }
    private void Update()
    {
        m_TimeBtwAttack -= Time.deltaTime;
        m_TimeBtwDash -= Time.deltaTime;
    }

    public void PhysicUpdate()
    {
        Debug.DrawRay(transform.position, m_Rigidbody.velocity, Color.cyan);

        m_Animator.SetBool(m_IsGroundID, m_Grounded);
        m_Animator.SetFloat("vSpeed", m_Rigidbody.velocity.y);

        if (m_Attack && m_TimeBtwAttack < 0)
        {
            m_Attack = false;
            m_TimeBtwAttack = timeBetweenAttack;
            m_Animator.SetTrigger(m_AttackID);

            m_AttackCounts++;
        }

        if (IsLock)
        {
            if (Mathf.Abs(m_Rigidbody.velocity.x) <= 6)
            {
                IsLock = false;
                switch (m_AnimDD)
                {
                    case WhichAnimation.dash:
                        m_Animator.SetBool("Dash", false);
                        break;
                    case WhichAnimation.dodge:
                        m_Animator.SetBool("Doudge", false);
                        break;
                }
            }
            return;
        }

        if (m_Grounded)
            m_Rigidbody.gravityScale = 0;
        else
            m_Rigidbody.gravityScale = 1;

        JumpStatus();

        if (m_Dash)
        {
            if (Player.SoulOfWind && m_TimeBtwDash < 0)
            {
                Dash(ref m_Dash, dashForce, true);

                m_PreviousDash = true;

                m_Animator.SetBool("Dash", true);
                m_AnimDD = WhichAnimation.dash;

                m_TimeBtwDash = timeBetweenDash;

                FindObjectOfType<AudioManager>().Play("Dash");
            }
        }
        else if (m_Dodge && m_Grounded)
        {
            Dash(ref m_Dodge, dodgeForce, false);

            m_Animator.SetBool("Doudge", true);
            m_AnimDD = WhichAnimation.dodge;

            FindObjectOfType<AudioManager>().Play("Dash");
        }

        // if we didn't execute these we don't want to anymore
        m_Dash = m_Dodge = m_Attack = false;
    }

    private float m_LastMoveDirection;
    public void Move(float direction)
    {
        if (direction != m_LastMoveDirection && !IsJumping)
        {
            m_LastMoveDirection = direction;
            m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y);
        }

        var slope = Vector2.right;
        if (m_Grounded)
            foreach (var groundCollider in m_GroundColliders)
            {
                if (groundCollider == null)
                    continue;

                slope = groundCollider.transform.right;
            }

        Vector2 movement = slope * direction * m_Base.moveSpeed;
        m_Animator.SetFloat(m_SpeedID, Mathf.Abs(direction));

        m_Rigidbody.AddForce(movement);
    }

    private void JumpStatus()
    {
        if (IsJumping && m_Grounded)
        {
            IsJumping = false;
            FindObjectOfType<AudioManager>().Play("Jumplanding");
        }
           

        if (m_ShouldJump && m_Grounded)
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
            m_Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            IsJumping = true;
            m_ShouldJump = false;
        }
        else if (m_InterruptJump && m_Rigidbody.velocity.y > 0 && IsJumping)
        {
            m_InterruptJump = false;

            Vector2 vel = m_Rigidbody.velocity;
            vel.y = 0;
            m_Rigidbody.velocity = vel;
        }

        m_InterruptJump = m_ShouldJump = false;
    }
    private void Dash(ref bool job, float force, bool isForwardDir)
    {
        job = false;

        m_Rigidbody.velocity = Vector2.zero;
        m_Rigidbody.gravityScale = 0;
        IsLock = true;

        var forceDir = Vector2.right * force;
        if ((isForwardDir && !m_Base.m_FacingRight) || (!isForwardDir && m_Base.m_FacingRight))
            forceDir = -forceDir; //whether we should be forced forward or not

        m_Rigidbody.AddForce(forceDir, ForceMode2D.Impulse);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
#endif
}
