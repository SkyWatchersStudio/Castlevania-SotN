using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public bool debug;
    [Space(10)]
    public GameObject pause;
    public GameObject map;
    public GameObject inventory;
    [Space(10)]
    public EventSystem eventSystem;

    [Space(10)]
    public Image experienceImage;
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI coins;
    public TextMeshProUGUI hearts;

    private GameObject m_PauseFirstButton, m_InventoryFirstButton;

    private static int m_Experience;
    private static int m_PlayerCurrentLevel;
    private static int m_NextLevelPoint = 100;
    private static int m_Money;
    private static int m_Hearts;

    public static GameManager m_Instance;

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
    public static int Hearts
    {
        get => m_Hearts;
        set
        {
            m_Hearts = value;
            m_Instance.hearts.text = m_Hearts.ToString();
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

    void Awake()
    {
        Cursor.visible = false;
        m_Instance = this;

        currentLevel.text = m_PlayerCurrentLevel.ToString();
        coins.text = m_Money.ToString();
        hearts.text = m_Hearts.ToString();

        m_PauseFirstButton = pause.GetComponentInChildren<Button>().gameObject;
        m_InventoryFirstButton = inventory.GetComponentInChildren<Button>().gameObject;
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            MenuActivator(pause, m_PauseFirstButton);
        else if (Input.GetButtonDown("Map"))
            MenuActivator(map);
        else if (Input.GetButtonDown("Inventory"))
            MenuActivator(inventory, m_InventoryFirstButton);
    }
    private void MenuActivator(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
        Time.timeScale = (Time.timeScale + 1) % 2;
    }
    private void MenuActivator(GameObject obj, GameObject firstSelected)
    {
        MenuActivator(obj);
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    public static void SavingData(Transform playerTransform)
    {
        //assign player Hp to its maximum health
        RestorePlayer(playerTransform);
        var frameIndex = CurrentFrame();

        SaveData data = new SaveData(m_Experience, m_PlayerCurrentLevel, m_NextLevelPoint,
                                     m_Money, playerTransform.position, frameIndex);
        SaveSystem.SaveState(data);
    }
    private static void RestorePlayer(Transform player)
    {
        var playerScript = player.GetComponent<Player>();
        playerScript.CurrentHealth = playerScript.health;
        playerScript.CurrentMana = playerScript.mana;
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
        RestorePlayer(playerTransform);

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

        return currentFrame; //frame switcher will reduce the number by 1
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
