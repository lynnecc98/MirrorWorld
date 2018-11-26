using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyType : MonoBehaviour {

    public Material redM, greenM, blueM;

    // Use this for initialization
    void Start () {
        elementType element = GetComponent<elementType>();
        if (element.element == Element.Red)
        {
            GetComponent<MeshRenderer>().material = redM;
        }
        if (element.element == Element.Blue)
        {
            GetComponent<MeshRenderer>().material = blueM;
        }
        if (element.element == Element.Green)
        {
            GetComponent<MeshRenderer>().material = greenM;
        }

    }
	
    
}
