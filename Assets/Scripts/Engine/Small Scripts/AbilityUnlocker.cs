using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocker : MonoBehaviour
{
    public AbilityTree abilityUnlock;

    public enum AbilityTree { Dash, Mist, CubeOfZoe };

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

            Destroy(gameObject);
        }
    }
}
