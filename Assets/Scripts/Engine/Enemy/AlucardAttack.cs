using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlucardAttack : MonoBehaviour
{
    PlayerCommonAbilities m_Abilities;
    private Enemy m_Enemy;

    private void Start()
    {
        m_Enemy = GetComponentInParent<Enemy>();
        m_Abilities = GetComponentInParent<PlayerCommonAbilities>();
    }

    public Collider2D CheckRange()
    {
        Collider2D[] results = new Collider2D[1];

        Physics2D.OverlapCircleNonAlloc(
            m_Abilities.attackPosition.position,
            m_Abilities.attackRange, results, m_Abilities.m_AttackLayer);

        return results[0];
    }

    public void Hit()
    {
        var opponent = CheckRange();
        if (opponent.CompareTag("Player"))
            m_Enemy.AttackPlayer(opponent.transform);
    }
}
