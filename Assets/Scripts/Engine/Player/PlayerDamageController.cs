using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageController : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    [Space(10)]
    //number of attack need to destroy player
    public int health;

    public Transform attackPos;
    public LayerMask WhatIsEnemies; 
    public float attackRange;

    void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, WhatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }

            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
            SceneManager.LoadScene(0);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
