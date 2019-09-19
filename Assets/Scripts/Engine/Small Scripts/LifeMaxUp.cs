using UnityEngine;

public class LifeMaxUp : MonoBehaviour
{
    public float increaseAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.m_Instance.health += increaseAmount;
            Player.m_Instance.CurrentHealth = Player.m_Instance.health;
        }
    }
}
