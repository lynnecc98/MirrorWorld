using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSkip : MonoBehaviour
{

    // Use this for initialization
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
        Time.timeScale = 1f;
    }

}