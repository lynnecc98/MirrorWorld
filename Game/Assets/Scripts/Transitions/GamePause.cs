using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]

public class GamePause : MonoBehaviour
{

    private CanvasGroup canvasGroup;


    // Use this for initialization
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
            }
            else
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
            }
        }
    }
}

