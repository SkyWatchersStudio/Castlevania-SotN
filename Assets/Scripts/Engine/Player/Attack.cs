using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Player m_PlayerScript;
    private const int m_NotGroundLayer = 1 << 8;

    private void Start() => m_PlayerScript = GetComponentInParent<Player>();

    public void Hit()
    {
        //get everything in the attack radius and detect if it is enemy
        var enemies = Physics2D.OverlapCircleAll(
            m_PlayerScript.attackPosition.position,
            m_PlayerScript.attackRange,
            m_NotGroundLayer);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Interact"))
                enemy.GetComponent<Interactable>().OnInteract();
            else if (enemy.CompareTag("Enemy"))
            {
                //add force to the opposite direction of enemy
                var rigidbody = enemy.GetComponent<Rigidbody2D>();
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                rigidbody.AddForce(direction * m_PlayerScript.attackForce, ForceMode2D.Impulse);

                //apply damage to enemy
                var enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                    enemyScript.TakeDamage();
            }
        }
    }
}
