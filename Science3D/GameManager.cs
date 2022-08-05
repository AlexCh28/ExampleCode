using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float nextLevelDelay = 1f, restartLevelDelay = 2.5f;
    int currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        if (currentScene + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(currentScene + 1);
        else SceneManager.LoadScene(0);
    }

    public IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(restartLevelDelay);
        SceneManager.LoadScene(currentScene);
    }
}
