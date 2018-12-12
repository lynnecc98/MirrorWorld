using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsBook : MonoBehaviour {

	public void loadCredits () {

        GameObject.Find("CreditsBook").GetComponent<Image>().enabled = true;
        bool status = GameObject.Find("CreditsBook").GetComponent<Image>().enabled;

        if (Input.GetKeyDown(KeyCode.Space))
            GameObject.Find("CreditsBook").GetComponent<Image>().enabled = !status;
    }


}
