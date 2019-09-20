using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public bool debug;
    public GameObject lifeMaxUI;
    [Space(10)]
    public GameObject pause;
    public GameObject map;
    public GameObject inventory;
    public GameObject abilityTree;
    [Space(10)]
    public EventSystem eventSystem;
    [Space(10)]
    public float potionHealthRestore;
    [Space(10)]
    public int heartMax = 10;
    [Space(10)]
    public TextMeshProUGUI manaStat;
    public TextMeshProUGUI heartStat;
    public TextMeshProUGUI attackSpeedStat;

    [Space(10)]
    public Image experienceImage;
    public Image experienceStat;

    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI currentLevelStat;

    public TextMeshProUGUI coins;
    public TextMeshProUGUI hearts;
    public TextMeshProUGUI m_PotionCount;

    private GameObject m_PauseFirstButton;
    private GameObject m_InventoryFirstButton;

    private static int m_Experience;
    private static int m_PlayerCurrentLevel;
    private static int m_NextLevelPoint = 100;
    private static int m_Money;
    private static int m_Hearts;
    private static int m_Potions;

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
            m_Instance.experienceStat.fillAmount =
                m_Instance.experienceImage.fillAmount;
        }
    }
    public static int Potions
    {
        get => m_Potions;
        set
        {
            m_Potions = value;
            m_Instance.m_PotionCount.text = m_Potions.ToString();
        }
    }
    public static int Hearts
    {
        get => m_Hearts;
        set
        {
            m_Hearts = value;
            if (m_Hearts > m_Instance.heartMax)
                m_Hearts = m_Instance.heartMax;
            m_Instance.hearts.text = m_Hearts.ToString();
        }
    }
    public static int PlayerCurrentLevel
    {
        get => m_PlayerCurrentLevel;
        set
        {
            m_PlayerCurrentLevel = value;
            string currentLevelText = m_PlayerCurrentLevel.ToString();
            m_Instance.currentLevel.text = currentLevelText;
            m_Instance.currentLevelStat.text = currentLevelText;

            m_Instance.heartMax += 6;
            m_Instance.heartStat.text = m_Instance.heartMax.ToString();

            Player.m_Instance.mana += 10;
            m_Instance.manaStat.text = Player.m_Instance.CurrentMana.ToString();

            Player.m_Abilities.timeBetweenAttack -= .02f;
            m_Instance.attackSpeedStat.text = Player.m_Abilities.timeBetweenAttack.ToString();
            Player.m_Instance.CurrentDamage += 2;
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

        m_PauseFirstButton = pause.GetComponentInChildren<Button>().gameObject;
        m_InventoryFirstButton = inventory.GetComponentInChildren<Button>().gameObject;

        // give default values foreach
        currentLevel.text = m_PlayerCurrentLevel.ToString();
        coins.text = m_Money.ToString();
        hearts.text = m_Hearts.ToString();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            MenuActivator(pause, m_PauseFirstButton, true);
        else if (Input.GetButtonDown("Map"))
            MenuActivator(map, true);
        else if (Input.GetButtonDown("Inventory"))
            MenuActivator(inventory, m_InventoryFirstButton, true);
        else if (Input.GetButtonDown("AbilityTree"))
            MenuActivator(abilityTree, true);
    }
    private void MenuActivator(GameObject obj, bool time)
    {
        obj.SetActive(!obj.activeSelf);
        if (!time)
            return;
        Time.timeScale = (Time.timeScale + 1) % 2;
    }
    public static void MenuActivator(GameObject obj, GameObject firstSelected, bool time)
    {
        m_Instance.MenuActivator(obj, time);
        m_Instance.eventSystem.SetSelectedGameObject(null);
        m_Instance.eventSystem.SetSelectedGameObject(firstSelected);
    }

    public static void SavingData(Transform playerTransform)
    {
        //assign player Hp to its maximum health
        RestorePlayer();
        var frameIndex = CurrentFrame();

        SaveData data;
        data.position = new float[3];
        for (int i = 0; i < data.position.Length; i++)
            data.position[i] = playerTransform.position[i];

        data.money = m_Money;
        data.nextLevelPoint = m_NextLevelPoint;
        data.experience = m_Experience;
        data.playerLevel = m_PlayerCurrentLevel;
        data.saveRoomIndex = frameIndex;

        data.damage = Player.m_Instance.CurrentDamage;
        data.maxHealth = Player.m_Instance.health;
        data.maxMana = Player.m_Instance.mana;
        data.hearts = Hearts;
        data.maxHearts = m_Instance.heartMax;
        data.potions = Potions;
        data.iceSword = InventoryItem.IceSword;
        data.fireSword = InventoryItem.FireSword;
        data.cubeOfZoe = Player.CubeOfZoe;
        data.mist = Player.MistForm;
        data.dash = Player.SoulOfWind;

        SaveSystem.SaveState(data);
    }
    private static void RestorePlayer()
    {
        var playerScript = Player.m_Instance;
        playerScript.CurrentHealth = playerScript.health;
        playerScript.CurrentMana = playerScript.mana;
    }
    public static void Loading()
    {
        SaveData? save = SaveSystem.LoadState();
        if (!save.HasValue)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            return;
        }

        SaveData data = save.Value;

        //assign player health to its maximum...
        RestorePlayer();

        int[] frames = { CurrentFrame(), data.saveRoomIndex };

        var playerTransform = Player.m_Instance.transform;
        //Destory the current frame and active the index given
        FramesList.SwitchFrames(frames, playerTransform);

        //Assign player position
        Vector3 newPos = new Vector3();
        for (int i = 0; i < 3; i++)
            newPos[i] = data.position[i];
        playerTransform.position = newPos;

        //reassigning player stats
        m_NextLevelPoint = data.nextLevelPoint;
        m_PlayerCurrentLevel = data.playerLevel;
        ExperiencePoint = data.experience;
        Coin = data.money;

        Player.m_Instance.CurrentDamage = data.damage;
        Player.m_Instance.health = data.maxHealth;
        Player.m_Instance.mana = data.maxMana;
        Hearts = data.hearts;
        m_Instance.heartMax = data.maxHearts;
        Potions = data.potions;
        InventoryItem.IceSword = data.iceSword;
        InventoryItem.FireSword = data.fireSword;
        Player.CubeOfZoe = data.cubeOfZoe;
        Player.MistForm = data.mist;
        Player.SoulOfWind = data.dash;
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
    public void OnExit() => Application.Quit();
}
