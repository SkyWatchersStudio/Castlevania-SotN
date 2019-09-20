using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

[RequireComponent(typeof(PlayerCommonAbilities))]
public sealed class Player : Characters
{
    public float jumpSaveTime;
    public float mistForce;
    [Space(10)]
    public Image healthImage;
    public Image manaImage;
    [Space(10)]
    public Rigidbody2D dagger;
    public float daggerSpeed;
    [Space(10)]
    public float mana;
    public float mistManaBurn;
    public float manaRegen;
    [Space(10)]
    public float sensitiveHealth;
    public PostProcessProfile[] profiles;
    public PostProcessVolume volume;
    [Space(10)]
    public AnimationClip[] swordAnimations;
    [Space(10)]
    public TextMeshProUGUI damageStat;

    [Space(10)]
    public Image[] cubeOfZoeStatImages;
    public Image[] souldOfWindStatImages;
    public Image[] mistFormStatImages;

    private Color m_AbilityStatColor;

    [System.NonSerialized]
    public AnimatorOverrideController m_OverrideAnimator;

    private bool m_IsSensitive;
    private Animator m_PostProcessAnim;

    private float m_HorizontalInput;
    private float m_JumpSaveTime;
    private bool m_MistTransform;
    private bool m_Dagger;
    private AudioManager m_AudioManager;

    public static PlayerCommonAbilities m_Abilities;

    public  static Player m_Instance;

    private float m_Health;
    public float CurrentHealth
    {
        get => m_Health;
        set
        {
            m_Health = value;

            if (m_Health > health)
                m_Health = health;

            healthImage.fillAmount = m_Health / health;

            if (m_Health <= sensitiveHealth && !m_IsSensitive)
            {
                m_IsSensitive = true;
                volume.profile = profiles[1];
                m_AudioManager.Play("HeartBeating");
                m_PostProcessAnim.enabled = true;
            }
            else if (m_Health > sensitiveHealth && m_IsSensitive)
            {
                volume.profile = profiles[0];
                m_IsSensitive = false;
                m_AudioManager.Stop("HeartBeating");
                m_PostProcessAnim.enabled = false;
            }
        }
    }
    private float m_Mana;
    public float CurrentMana
    {
        get => m_Mana;
        set
        {
            m_Mana = value;
            manaImage.fillAmount = m_Mana / mana;
        }
    }
    public float CurrentDamage
    {
        get => attackDamage;
        set
        {
            attackDamage = value;
            damageStat.text = attackDamage.ToString();
        }
    }

    public static bool CubeOfZoe
    {
        get => m_CubeOfZoe;
        set
        {
            m_CubeOfZoe = value;

            if (m_CubeOfZoe)
                foreach (var img in m_Instance.cubeOfZoeStatImages)
                    img.color = m_Instance.m_AbilityStatColor;
        }
    }
    public static bool SoulOfWind
    {
        get => m_DashAbility;
        set
        {
            m_DashAbility = value;

            if (m_DashAbility)
                foreach (var img in m_Instance.souldOfWindStatImages)
                    img.color = m_Instance.m_AbilityStatColor;
        }
    }
    public static bool MistForm
    {
        get => m_MistAbility;
        set
        {
            m_MistAbility = value;

            if (m_MistAbility)
                foreach (var img in m_Instance.mistFormStatImages)
                    img.color = m_Instance.m_AbilityStatColor;
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

    private static bool m_MistAbility, m_CubeOfZoe, m_DashAbility;

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

    private ParticleSystem m_MistForm;
    private SpriteRenderer m_Sprite;
    private bool m_IsMist;
    private void MistShifting()
    {
        m_MistTransform = false;
        m_Rigidbody.velocity = Vector2.zero;

        // cause mist to not interact with enemies
        gameObject.layer = gameObject.layer == 8 ? 12 : 8;
        
        if (m_IsMist || !m_Abilities.m_Grounded)
            m_Rigidbody.gravityScale = (m_Rigidbody.gravityScale + 1) % 2;

        if (m_IsMist)
            m_MistForm.Stop();
        else
            m_MistForm.Play();

        m_Sprite.enabled = !m_Sprite.enabled;

        m_AudioManager.Play("MistSwap");

        m_IsMist = !m_IsMist;
    }
    private void MistMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 vector = new Vector2(horizontal, vertical);
        vector.Normalize();

        m_Rigidbody.AddForce(vector * mistForce);

        CurrentMana -= mistManaBurn * Time.fixedDeltaTime;
        if (CurrentMana <= 0)
            MistShifting();
    }

    public override void Start()
    {
        base.Start();
        m_Instance = this;

        ColorUtility.TryParseHtmlString("#00C3FF", out m_AbilityStatColor);

        m_Abilities = GetComponent<PlayerCommonAbilities>();
        m_Animator = GetComponentInChildren<Animator>();
        m_MistForm = GetComponentInChildren<ParticleSystem>();
        m_Sprite = GetComponentInChildren<SpriteRenderer>();
        m_AudioManager = FindObjectOfType<AudioManager>();
        m_PostProcessAnim = volume.GetComponent<Animator>();

        m_OverrideAnimator = new AnimatorOverrideController(m_Animator.runtimeAnimatorController);
        m_Animator.runtimeAnimatorController = m_OverrideAnimator;

        CurrentHealth = health;
        CurrentMana = mana;
        CurrentDamage = attackDamage;

        GameManager.PlayerCurrentLevel = GameManager.PlayerCurrentLevel;
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
        else
        {
            CurrentMana += manaRegen * Time.fixedDeltaTime;
            if (CurrentMana > mana)
                CurrentMana = mana;
        }

        if (m_Dagger && GameManager.Hearts > 0)
        {
            m_Dagger = false;

            GameManager.Hearts -= 1;

            Rigidbody2D d =
                Instantiate(dagger, m_Abilities.attackPosition.position, Quaternion.identity);

            Vector2 direction = Vector2.right;
            if (!m_FacingRight)
            {
                d.transform.rotation = Quaternion.Euler(0, 180, 0);
                direction = Vector2.left;
            }
            d.velocity = direction * daggerSpeed;

            m_AudioManager.Play("DaggerThrow");
        }
        m_Dagger = false;

        m_Abilities.PhysicUpdate();
        if (m_Abilities.IsLock)
            return;

        if (m_MistTransform && CurrentMana >= mistManaBurn && m_MistAbility)
            MistShifting(); // transform to mist

        base.FixedUpdate();

        m_MistTransform = false;
    }
    public override void Move() => m_Abilities.Move(m_HorizontalInput);
    public override void TakeDamage(float damage)
    {
        m_AudioManager.Play("PlayerHit");

        m_Animator.SetTrigger("Hit");
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
            GameManager.Loading(this.transform);
    }
}
