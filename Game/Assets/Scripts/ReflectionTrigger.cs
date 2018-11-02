using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReflectionTrigger : MonoBehaviour {
    public bool triggered = false;
    private bool inTrigger = false;
    elementType myElement;
    GameObject player;

    private void Start()
    {
        myElement = GetComponent<elementType>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("triggered reflection");
                triggered = !triggered;

                if (myElement.element == player.GetComponent<elementType>().element)
                {
                    SceneManager.LoadScene("WinScene");
                }
                else
                {
                    SceneManager.LoadScene("LoseScene");
                }

            }
        }
    }

    //private void OnTriggerStay(Collider player)
    //{
    //    Debug.Log("inside");
    //    if (player.attachedRigidbody)
    //    {

    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
