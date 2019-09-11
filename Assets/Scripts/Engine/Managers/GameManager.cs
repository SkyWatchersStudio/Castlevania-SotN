using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pause;
    public Image experienceImage;
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI coins;

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
                PlayerCurrentLevel++;
                m_NextLevelPoint *= 2;
            }

            m_Instance.experienceImage.fillAmount =
                (float)m_Experience / (float)m_NextLevelPoint;

        }
    }
    public static int PlayerCurrentLevel
    {
        get => m_PlayerCurrentLevel;
        set
        {
            m_PlayerCurrentLevel = value;
            m_Instance.currentLevel.text = m_PlayerCurrentLevel.ToString();

            //upgrade player stats here.
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
        m_Instance = this;
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

    public static void SavingData(Transform playerTransform)
    {
        //assign player Hp to its maximum health
        IncreasePlayerHP(playerTransform);
        var frameIndex = CurrentFrame();

        SaveData data = new SaveData(m_Experience, m_PlayerCurrentLevel, m_NextLevelPoint,
                                     m_Money, playerTransform.position, frameIndex);
        SaveSystem.SaveState(data);
    }
    private static void IncreasePlayerHP(Transform player)
    {
        var playerScript = player.GetComponent<Player>();
        playerScript.CurrentHealth = playerScript.health;
    }
    public static void Loading(Transform playerTransform)
    {
        SaveData data = SaveSystem.LoadState();
        if (data == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        //assign player health to its maximum...
        IncreasePlayerHP(playerTransform);

        int[] frames = { CurrentFrame(), data.saveRoomIndex };
        //Destory the current frame and active the index given
        FramesList.SwitchFrames(frames, playerTransform);

        //Assign player position
        Vector3 newPos = new Vector3();
        for (int i = 0; i < 3; i++)
        {
            newPos[i] = data.position[i];
        }
        playerTransform.position = newPos;

        //reassigning player stats
        m_NextLevelPoint = data.nextLevelPoint;
        m_PlayerCurrentLevel = data.playerLevel;
        ExperiencePoint = data.experience;
        Coin = data.money;
    }
    private static int CurrentFrame()
    {
        var frames = FramesList.m_Instance.frames;
        int currentFrame = 0;

        for (int i = 0; i < frames.Length; i++)
            if (frames[i].activeSelf)
            {
                currentFrame = i;
                break;
            }

        return currentFrame + 1; //frame switcher will reduce the number by 1
    }
    private void OnApplicationQuit()
    {
        SaveSystem.DeleteSave();
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
