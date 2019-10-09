using UnityEngine;

public class AbilityUnlocker : MonoBehaviour
{
    public AbilityTree abilityUnlock;
    public GameObject abilityIntro;

    public enum AbilityTree { Dash, Mist, CubeOfZoe };

    private bool m_Triggered;
    private bool m_Job;
    private Animator m_PlayerAnimator;
    private PlayerAbility m_AbilityBehaviour;

    private void Start()
    {
        m_PlayerAnimator = Player.m_Instance.GetComponentInChildren<Animator>();
        m_AbilityBehaviour = m_PlayerAnimator.GetBehaviour<PlayerAbility>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (abilityUnlock == AbilityTree.Dash)
                Player.SoulOfWind = true;
            else if (abilityUnlock == AbilityTree.Mist)
                Player.MistForm = true;
            else if (abilityUnlock == AbilityTree.CubeOfZoe)
                Player.CubeOfZoe = true;

            m_Triggered = true;

            m_AbilityBehaviour.AbilityIntro = this.abilityIntro;
        }
    }

    private void Update()
    {
        if (m_Triggered)
        {
            if (!m_Job)
            {
                m_PlayerAnimator.SetTrigger("Abilities");
                AudioManager.Play("AbilityGain");
                m_Job = true;
            }

            if (m_AbilityBehaviour.SkipAllowed)
            {
                if (Input.anyKeyDown)
                {
                    Destroy(abilityIntro);
                    m_AbilityBehaviour.Reset();
                    Destroy(gameObject);
                }
            }
        }
    }
}
