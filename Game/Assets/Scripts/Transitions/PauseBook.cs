using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBook : MonoBehaviour {

	public void loadControls () {

        GameObject.Find("BookPopUp").GetComponent<Image>().enabled = true;
        bool status = GameObject.Find("BookPopUp").GetComponent<Image>().enabled;

        if (Input.GetKeyDown(KeyCode.Space))
            GameObject.Find("BookPopUp").GetComponent<Image>().enabled = !status;
    }
}
