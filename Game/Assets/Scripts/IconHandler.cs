using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
               this.transform.Find("Icon").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

}
