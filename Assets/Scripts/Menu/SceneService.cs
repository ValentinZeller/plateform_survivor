using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService : MonoBehaviour
{
    public List<string> stages;

    public void SwapScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public int GetCurrentSceneID()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
