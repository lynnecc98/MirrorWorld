using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour {

    [SerializeField] private Image customImage;
    [SerializeField] private Image customImage2;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
                customImage.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                customImage.enabled = false;
        }
    }
}
