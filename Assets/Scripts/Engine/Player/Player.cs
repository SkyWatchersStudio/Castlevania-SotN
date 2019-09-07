using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class Player : Characters
{
    public float jumpSaveTime;
    public float jumpForce;
    [Space(10)]
    public float attackForce;
    public float timeBetweenAttack;
    public Transform attackPosition;
    public float attackRange;
    [Space(10)]
    public float dashForce;
    public float timeBetweenDash;
    [Space(10)]
    public float dodgeForce;
    [Space(10)]
    public Image healthImage;

    private float m_HorizontalInput;
    private float m_JumpSaveTime;
    private bool m_Grounded, m_IsJumping, m_InterruptJumping;
    private float m_TimeBtwAttack, m_TimeBtwDash;
    private bool m_Attack, m_Dash, m_Dodge, m_Lock;
    private Collider2D[] m_GroundColliders;
    private float m_GravityScale;

    public float m_MaxHealth;

    private WhichAnimation m_AnimDD; //for tracking dodge anim or dash anim is playing

    int m_AttackID, m_SpeedID, m_IsGroundID;

    enum WhichAnimation { dodge, dash };

    #region FrameUpdate
    private void Update()
    {
        m_JumpSaveTime -= Time.deltaTime;
        m_TimeBtwAttack -= Time.deltaTime;
        m_TimeBtwDash -= Time.deltaTime;

        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        Flip(m_HorizontalInput);

        InputDetection();

        m_Animator.SetFloat(m_SpeedID, Mathf.Abs(m_HorizontalInput));
    }
    private void InputDetection()
    {
        //jump input determiner
        if (Input.GetButtonDown("Jump"))
            m_JumpSaveTime = jumpSaveTime;
        //we don't want to apply force when we are falling or we are not jumping ofcourse!
        else if (Input.GetButtonUp("Jump"))
            if (m_Rigidbody.velocity.y > 0 && m_IsJumping)
                m_InterruptJumping = true;

        if (Input.GetButtonDown("Fire2") && m_TimeBtwAttack < 0)
        {
            m_TimeBtwAttack = timeBetweenAttack;
            m_Attack = true;
        }
        else if (Input.GetButtonDown("Fire1") && m_TimeBtwDash < 0)
        {
            m_Dash = true;
            m_TimeBtwDash = timeBetweenDash;
        }
        else if (Input.GetButtonDown("Dodge") && !m_Lock && m_Grounded)
            m_Dodge = true;
    }
    #endregion
    private void JumpStatus()
    {
        if (m_IsJumping && m_Grounded)
            m_IsJumping = false;

        m_Animator.SetFloat("vSpeed", m_Rigidbody.velocity.y);

        if (m_JumpSaveTime > 0 && m_Grounded)
        {
            m_Rigidbody.AddForce(Vector2.up * jumpForce);
            m_IsJumping = true;
        }
        else if (m_InterruptJumping)
        {
            m_Rigidbody.AddForce(Vector2.up * -m_Rigidbody.velocity.y, ForceMode2D.Impulse);
            m_InterruptJumping = m_IsJumping = false;
        }
    }
    private void Dash(ref bool job, float force, bool forward)
    {
        job = false;

        m_Rigidbody.velocity = Vector2.zero;
        m_Rigidbody.gravityScale = 0;
        m_Lock = true;

        var forceDir = Vector2.right * force;
        if ((forward && !m_FacingRight) || (!forward && m_FacingRight))
            forceDir = -forceDir;

        m_Rigidbody.AddForce(forceDir, ForceMode2D.Impulse);
    }

    public override void Start()
    {
        base.Start();

        //override this cause animator component is different than base class thought
        m_Animator = GetComponentInChildren<Animator>();

        //Animator parameter Ids
        m_AttackID = Animator.StringToHash("Attack");
        m_SpeedID = Animator.StringToHash("Speed");
        m_IsGroundID = Animator.StringToHash("isGround");

        m_GravityScale = m_Rigidbody.gravityScale;

        m_MaxHealth = health;
    }
    public override void FixedUpdate()
    {
        //debuger...
        Debug.DrawRay(transform.position, m_Rigidbody.velocity, Color.cyan);

        m_Grounded = CheckGround(out m_GroundColliders);
        m_Animator.SetBool(m_IsGroundID, m_Grounded);

        if (m_Grounded && m_Rigidbody.gravityScale != 0)
            m_Rigidbody.gravityScale = 0;
        else if (!m_Grounded && !m_Lock)
            m_Rigidbody.gravityScale = m_GravityScale;

        if (m_Attack)
        {
            m_Attack = false;
            m_Animator.SetTrigger(m_AttackID);
        }
        else if (m_Dash)
        {
            Dash(ref m_Dash, dashForce, true);

            m_Animator.SetBool("Dash", true);
            m_AnimDD = WhichAnimation.dash;
        }
        else if (m_Dodge)
        {
            Dash(ref m_Dodge, dodgeForce, false);

            m_Animator.SetBool("Doudge", true);
            m_AnimDD = WhichAnimation.dodge;
        }

        if (m_Lock)
        {
            if (Mathf.Abs(m_Rigidbody.velocity.x) <= 6)
            {
                m_Lock = false;
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

        //anything about jumping stuff
        JumpStatus();

        base.FixedUpdate();
    }
    public override void Move()
    {
        var direction = Vector2.right;
        if (m_Grounded)
            foreach (var groundCollider in m_GroundColliders)
            {
                if (groundCollider == null)
                    continue;

                direction = groundCollider.transform.right;
            }

        Vector2 movement = direction * m_HorizontalInput * moveSpeed;
        m_Rigidbody.AddForce(movement);
    }
    public override void TakeDamage()
    {
        health -= 1;
        var currentHp = (float)health / m_MaxHealth;
        healthImage.fillAmount = currentHp;

        if (health <= 0)
        {
            GameManager.Loading();
            GameManager.frame2.SetActive(false);
        }
    }

    #region tools
#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
#endif
    #endregion
}
