using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorPopUpUI : MonoBehaviour {

    [SerializeField] private Image customImage;
    [SerializeField] private Image customImage2;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (other.GetComponent<KeyCollector>().hasKey)
                customImage.enabled = true;
            else
                customImage2.enabled = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<KeyCollector>().hasKey)
                customImage.enabled = false;
            else
                customImage2.enabled = false;

        }
    }
}
