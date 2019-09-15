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
    public float mistForce;
    [Space(10)]
    public Image healthImage;

    private float m_HorizontalInput;
    private float m_JumpSaveTime;
    private bool m_Grounded, m_IsJumping;
    private float m_TimeBtwAttack, m_TimeBtwDash;
    private bool m_Attack, m_Dash, m_Dodge, m_Lock;
    private Collider2D[] m_GroundColliders;
    private bool m_MistTransform;

    private float m_Health;
    public float CurrentHealth
    {
        get => m_Health;
        set
        {
            m_Health = value;
            healthImage.fillAmount = m_Health / health;
        }
    }

    int m_AttackID, m_SpeedID, m_IsGroundID;

    private WhichAnimation m_AnimDD; //for tracking dodge anim or dash anim is playing
    enum WhichAnimation { dodge, dash };

    #region FrameUpdate
    private void Update()
    {
        m_JumpSaveTime -= Time.deltaTime;
        m_TimeBtwAttack -= Time.deltaTime;
        m_TimeBtwDash -= Time.deltaTime;

        //if (m_MistAbility)
        if (Input.GetButtonDown("Mist"))
            m_MistTransform = true;

        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        Flip(m_HorizontalInput);

        if (m_IsMist)
            return;

        InputDetection();
        m_Animator.SetFloat(m_SpeedID, Mathf.Abs(m_HorizontalInput));
    }

    public static bool m_DashAbility, m_MistAbility;

    private void InputDetection()
    {
        //jump input determiner
        if (Input.GetButtonDown("Jump"))
            m_JumpSaveTime = jumpSaveTime;
        //we don't want to apply force when we are falling or we are not jumping ofcourse!
        else if (Input.GetButtonUp("Jump") && m_Rigidbody.velocity.y > 0 && m_IsJumping)
        {
            Vector2 vel = m_Rigidbody.velocity;
            vel.y = 0;
            m_Rigidbody.velocity = vel;
        }

        if (Input.GetButtonDown("Attack") && m_TimeBtwAttack < 0)
        {
            m_TimeBtwAttack = timeBetweenAttack;
            m_Attack = true;
        }
        else if (Input.GetButtonDown("Dodge") && m_Grounded)
            m_Dodge = true;

        if (m_DashAbility)
        {
            if (Input.GetButtonDown("Dash") && m_TimeBtwDash < 0)
            {
                m_Dash = true;
                m_TimeBtwDash = timeBetweenDash;
            }
        }
    }
    #endregion
    private void JumpStatus()
    {
        if (m_IsJumping && m_Grounded)
            m_IsJumping = false;

        if (m_JumpSaveTime > 0 && m_Grounded)
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
            m_Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            m_IsJumping = true;
        }
    }
    private void Dash(ref bool job, float force, bool isForwardDir)
    {
        job = false;

        m_Rigidbody.velocity = Vector2.zero;
        m_Rigidbody.gravityScale = 0;
        m_Lock = true;

        var forceDir = Vector2.right * force;
        if ((isForwardDir && !m_FacingRight) || (!isForwardDir && m_FacingRight))
            forceDir = -forceDir; //whether we should be forced forward or not

        m_Rigidbody.AddForce(forceDir, ForceMode2D.Impulse);
    }

    private bool m_IsMist;

    private void MistShifting()
    {
        m_MistTransform = false;
        m_Rigidbody.velocity = Vector2.zero;

        // cause mist to not interact with enemies
        gameObject.layer = gameObject.layer == 8 ? 12 : 8;
        
        if (m_IsMist || !m_Grounded)
            m_Rigidbody.gravityScale = (m_Rigidbody.gravityScale + 1) % 2;

        m_IsMist = !m_IsMist;
    }
    private void MistMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 vector = new Vector2(horizontal, vertical);
        vector.Normalize();

        m_Rigidbody.AddForce(vector * mistForce);
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

        m_Health = health;
    }
    public override void FixedUpdate()
    {
        //debuger...
        Debug.DrawRay(transform.position, m_Rigidbody.velocity, Color.cyan);

        if (m_IsMist)
        {
            if (m_MistTransform) // transform back to normal
                MistShifting();
            MistMove();
            return;
        }

        m_Grounded = CheckGround(out m_GroundColliders);

        m_Animator.SetBool(m_IsGroundID, m_Grounded);
        m_Animator.SetFloat("vSpeed", m_Rigidbody.velocity.y);

        if (m_Attack)
        {
            m_Attack = false;
            m_Animator.SetTrigger(m_AttackID);
        }
        if (!m_Lock)
        {
            if (m_Grounded)
                m_Rigidbody.gravityScale = 0;
            else
                m_Rigidbody.gravityScale = 1;

            if (m_Dash)
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
            else if (m_MistTransform)
                MistShifting(); // transform to mist
        }
        else if (m_Lock)
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
        CurrentHealth -= 1;

        if (CurrentHealth <= 0)
            GameManager.Loading(this.transform);
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
