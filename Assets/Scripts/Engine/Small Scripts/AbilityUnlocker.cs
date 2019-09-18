using System.Collections;
using UnityEngine;

public class AbilityUnlocker : MonoBehaviour
{
    public AbilityTree abilityUnlock;
    public GameObject abilityIntro;

    public enum AbilityTree { Dash, Mist, CubeOfZoe };

    private bool m_Triggered;
    private bool m_Job;
    private float m_Timer;
    private Animator m_PlayerAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (abilityUnlock == AbilityTree.Dash)
                PlayerCommonAbilities.m_DashAbility = true;
            else if (abilityUnlock == AbilityTree.Mist)
                Player.m_MistAbility = true;
            else if (abilityUnlock == AbilityTree.CubeOfZoe)
                Player.m_CubeOfZoe = true;

            m_PlayerAnimator = collision.GetComponentInChildren<Animator>();
            collision.attachedRigidbody.velocity = Vector2.zero;

            m_Triggered = true;
        }
    }

    private void Update()
    {
        if (m_Triggered)
        {
            if (!m_Job)
            {
                m_PlayerAnimator.SetTrigger("Abilities");
                FindObjectOfType<AudioManager>().Play("AbilityGain");
                m_Job = true;
            }

            m_Timer += Time.deltaTime;
            if (m_Timer >= 1)
                abilityIntro.SetActive(true);

            if (Input.anyKeyDown)
            {
                Destroy(abilityIntro);
                Destroy(gameObject);
            }
        }
    }
}
