using System.Collections;
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
        //StartCoroutine(LoadingAsync());
    }
    IEnumerator LoadingAsync()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        while (!async.isDone)
            yield return null;

        if (async.isDone)
        {
            GameManager.Loading();
            SceneManager.UnloadSceneAsync(0);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
