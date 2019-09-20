using UnityEngine;

public class LifeMaxUp : MonoBehaviour
{
    public float increaseAmount = 10;

    private bool m_IsTriggered;
    private float m_Timer;

    private static AudioManager m_audio;

    private void Start()
    {
        if(m_audio == null)
           m_audio = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.m_Instance.health += increaseAmount;
            Player.m_Instance.CurrentHealth = Player.m_Instance.health;
            GameManager.m_Instance.lifeMaxUI.SetActive(true);
            m_IsTriggered = true;
            m_audio.Play("Potionpickup");
        }
    }
    private void Update()
    {
        if (m_IsTriggered)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > 2)
            {
                GameManager.m_Instance.lifeMaxUI.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
