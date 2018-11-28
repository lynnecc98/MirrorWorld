using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(this.CompareTag("Crystal"))
                 GameObject.Find("BridgePopUp").GetComponent<Image>().enabled = true;
            if (this.CompareTag("Obstacle") && !other.GetComponent<KeyCollector>().hasKey)
                GameObject.Find("NeedKeyPopUp").GetComponent<Image>().enabled = true;
            if (this.CompareTag("Reflection"))
                GameObject.Find("ReflectionPopUp").GetComponent<Image>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.CompareTag("Crystal"))
                GameObject.Find("BridgePopUp").GetComponent<Image>().enabled = false;
            if (this.CompareTag("Obstacle") && !other.GetComponent<KeyCollector>().hasKey)
                GameObject.Find("NeedKeyPopUp").GetComponent<Image>().enabled = false;
            if (this.CompareTag("Reflection"))
                GameObject.Find("ReflectionPopUp").GetComponent<Image>().enabled = false;
        }
    }
}
