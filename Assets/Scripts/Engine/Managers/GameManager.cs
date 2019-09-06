using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pause;
    public Image experienceImage;
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI coins;

    public static GameObject saveRoom;

    private static int m_Experience;
    private static int m_PlayerCurrentLevel;
    private static int m_NextLevelPoint = 100;
    private static int m_Money;
    private static GameManager m_Instance;

    public static int ExperiencePoint
    {
        get => m_Experience;
        set
        {
            m_Experience = value;

            if (m_Experience >= m_NextLevelPoint)
            {
                m_Experience -= m_NextLevelPoint;
                m_PlayerCurrentLevel++;
                m_NextLevelPoint *= 2;
            }

            m_Instance.experienceImage.fillAmount =
                (float)m_Experience / (float)m_NextLevelPoint;

            m_Instance.currentLevel.text = m_PlayerCurrentLevel.ToString();
        }
    }
    public static int Coin
    {
        get => m_Money;
        set
        {
            m_Money = value;
            m_Instance.coins.text = m_Money.ToString();
        }
    }

    void Start()
    {
        Cursor.visible = false;
        m_Instance = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentLevel.text = m_PlayerCurrentLevel.ToString();
        coins.text = m_Money.ToString();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = (Time.timeScale + 1) % 2;
            pause.SetActive(!pause.activeSelf);
        }
    }

    public static void SavingData()
    {
        var playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        var playerHP = playerTransform.GetComponent<Player>().health;

        SaveData data = new SaveData(m_Experience, m_PlayerCurrentLevel, m_NextLevelPoint,
                                     m_Money, playerTransform.position, playerHP);

        SaveSystem.SaveState(data);
    }
    public static void Loading()
    {
        if (saveRoom == null)
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        SaveData data = SaveSystem.LoadState();

        saveRoom.SetActive(true);

        Transform playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        var playerScript = playerTrans.GetComponent<Player>();
        playerScript.health = data.health;
        playerScript.healthImage.fillAmount = 
            (float)playerScript.health / playerScript.m_MaxHealth;

        Vector3 newPos = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            newPos[i] = data.position[i];
        }
        playerTrans.position = newPos;

        m_NextLevelPoint = data.nextLevelPoint;
        m_PlayerCurrentLevel = data.playerLevel;
        ExperiencePoint = data.experience;

        Coin = data.money;
    }

    public void OnExit() => Application.Quit();
}
