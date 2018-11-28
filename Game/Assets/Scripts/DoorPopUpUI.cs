using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorPopUpUI : MonoBehaviour {

    [SerializeField] private Image customImage;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!other.GetComponent<KeyCollector>().hasKey)
                customImage.enabled = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponent<KeyCollector>().hasKey)
                customImage.enabled = false;


        }
    }
}
