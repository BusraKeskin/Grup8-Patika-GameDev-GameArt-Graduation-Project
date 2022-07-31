using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.loadSameScene += loadSameScene;
    }

    private void OnDisable()
    {
        EventManager.loadSameScene -= loadSameScene;
    }

    private void loadSameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
