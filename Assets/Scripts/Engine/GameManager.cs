using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pause;

    private Button m_PauseButton;

    void Start()
    {
        Cursor.visible = false;
        m_PauseButton = GetComponent<Button>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            pause.SetActive(!pause.activeSelf);
    }
    public void OnExit() => Application.Quit();
}
