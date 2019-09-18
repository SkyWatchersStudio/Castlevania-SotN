using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerCommonAbilities))]
public sealed class Player : Characters
{
    public float jumpSaveTime;
    public float mistForce;
    [Space(10)]
    public Image healthImage;
    [Space(10)]
    public Rigidbody2D dagger;
    public float daggerSpeed;

    private float m_HorizontalInput;
    private float m_JumpSaveTime;
    private bool m_MistTransform;
    private bool m_Dagger;

    private PlayerCommonAbilities m_Abilities;

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

    private void Update()
    {
        m_JumpSaveTime -= Time.deltaTime;
        if (m_JumpSaveTime < 0)
            m_Abilities.m_ShouldJump = false;

        //if (m_MistAbility)
        if (Input.GetButtonDown("Mist"))
            m_MistTransform = true;

        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        Flip(m_HorizontalInput);

        if (m_IsMist)
            return;

        InputDetection();
    }

    public static bool m_MistAbility, m_CubeOfZoe;

    private void InputDetection()
    {
        //jump input determiner
        if (Input.GetButtonDown("Jump"))
        {
            m_JumpSaveTime = jumpSaveTime;
            m_Abilities.m_ShouldJump = true;
        }
        //we don't want to apply force when we are falling or we are not jumping ofcourse!
        else if (Input.GetButtonUp("Jump"))
            m_Abilities.m_InterruptJump = true;

        if (Input.GetButtonDown("Attack"))
            m_Abilities.m_Attack = true;
        else if (Input.GetButtonDown("Dodge"))
            m_Abilities.m_Dodge = true;
        else if (Input.GetButtonDown("Dash"))
            m_Abilities.m_Dash = true;
        else if (Input.GetButtonDown("Throw"))
            m_Dagger = true;
    }

    private bool m_IsMist;
    private void MistShifting()
    {
        m_MistTransform = false;
        m_Rigidbody.velocity = Vector2.zero;

        // cause mist to not interact with enemies
        gameObject.layer = gameObject.layer == 8 ? 12 : 8;
        
        if (m_IsMist || !m_Abilities.m_Grounded)
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

        m_Abilities = GetComponent<PlayerCommonAbilities>();
        m_Animator = GetComponentInChildren<Animator>();

        CurrentHealth = health;
    }
    public override void FixedUpdate()
    {
        m_Abilities.m_Grounded = CheckGround(out m_Abilities.m_GroundColliders);

        if (m_IsMist)
        {
            if (m_MistTransform) // transform back to normal
                MistShifting();
            MistMove();
            return;
        }

        if (m_Dagger && GameManager.Hearts > 0)
        {
            m_Dagger = false;

            GameManager.Hearts -= 1;

            Rigidbody2D d =
                Instantiate(dagger, m_Abilities.attackPosition.position, Quaternion.identity);

            Vector2 direction = Vector2.right;
            if (!m_FacingRight)
                direction = Vector2.left;
            d.velocity = direction * daggerSpeed;
        }
        m_Dagger = false;

        m_Abilities.PhysicUpdate();
        if (m_Abilities.IsLock)
            return;

        if (m_MistTransform)
            MistShifting(); // transform to mist

        base.FixedUpdate();
    }
    public override void Move() => m_Abilities.Move(m_HorizontalInput);
    public override void TakeDamage()
    {
        m_Animator.SetTrigger("Hit");
        CurrentHealth -= 1;
        if (CurrentHealth <= 0)
            GameManager.Loading(this.transform);
    }
}
