﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pause;

    private static int m_Experience;
    private static int m_PlayerCurrentLevel;
    private static int m_LevelReachPoint = 100;
    private static int m_Money;

    public static int ExperiencePoint
    {
        get => m_Experience;
        set
        {
            m_Experience += value;
            Debug.Log($"Experience: {m_Experience}");
            if (m_Experience >= m_LevelReachPoint)
            {
                m_Experience -= m_LevelReachPoint;
                m_PlayerCurrentLevel++;
                m_LevelReachPoint *= 2;
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
    }

    public void OnExit() => Application.Quit();
}
