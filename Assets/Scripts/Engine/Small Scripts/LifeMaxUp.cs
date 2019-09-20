using UnityEngine;

public class LifeMaxUp : MonoBehaviour
{
    public float increaseAmount = 10;
    public GameObject healthUI;

    private bool m_IsTriggered;
    private float m_Timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.m_Instance.health += increaseAmount;
            Player.m_Instance.CurrentHealth = Player.m_Instance.health;
            healthUI.SetActive(true);
            m_IsTriggered = true;
        }
    }
    private void Update()
    {
        if (m_IsTriggered)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > 2)
            {
                Destroy(gameObject);
                healthUI.SetActive(false);
            }
        }
    }
}
