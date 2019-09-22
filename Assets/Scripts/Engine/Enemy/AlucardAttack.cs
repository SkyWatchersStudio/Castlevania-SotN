using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlucardAttack : MonoBehaviour
{
    private Enemy m_Enemy;
    private Alucard m_Alucard;

    private void Start()
    {
        m_Enemy = GetComponentInParent<Enemy>();
        m_Alucard = GetComponentInParent<Alucard>();
    }

    public void Hit()
    {
        var opponent = m_Alucard.CheckRange();
        if (opponent)
            if (opponent.CompareTag("Player"))
                m_Enemy.AttackPlayer();
    }
}
