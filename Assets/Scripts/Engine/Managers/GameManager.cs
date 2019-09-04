using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pause;

    private static int m_Experience;
    private static int m_PlayerCurrentLevel;
    private static int m_NextLevelPoint = 100;
    private static int m_Money;

    public static int ExperiencePoint
    {
        get => m_Experience;
        set
        {
            m_Experience += value;
            Debug.Log($"Experience: {m_Experience}");
            if (m_Experience >= m_NextLevelPoint)
            {
                m_Experience -= m_NextLevelPoint;
                m_PlayerCurrentLevel++;
                m_NextLevelPoint *= 2;
            }
        }
    }
    public static int Coin
    {
        get => m_Money;
        set
        {
            m_Money += value;
            Debug.Log($"Coins: {m_Money}");
        }
    }
    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (pause.activeSelf)
                Time.timeScale = 1;
            else if (!pause.activeSelf)
                Time.timeScale = 0;
            pause.SetActive(!pause.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            SaveData data = new SaveData(m_Experience, m_PlayerCurrentLevel, m_NextLevelPoint,
                m_Money, playerTransform.position);
            SaveSystem.SaveState(data);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SaveData data = SaveSystem.LoadState();

            Transform playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

            Vector3 newPos = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                newPos[i] = data.position[i];
            }
            playerTrans.position = newPos;

            m_Experience = data.experience;
            m_Money = data.money;
            m_PlayerCurrentLevel = data.playerLevel;
            m_NextLevelPoint = data.nextLevelPoint;
        }
    }

    public void OnExit() => Application.Quit();
}
