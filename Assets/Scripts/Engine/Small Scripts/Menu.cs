using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void NewGame()
    {
        SaveSystem.DeleteSave();
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
