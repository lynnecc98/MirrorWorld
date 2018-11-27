using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionColor : MonoBehaviour {

    public Material redM, blueM, greenM;

    // Use this for initialization
    void Start () {
        elementType current = GetComponent<elementType>();
        if (current.element == Element.Red)
        {
            GetComponent<SkinnedMeshRenderer>().material = redM;
        }
        if (current.element == Element.Blue)
        {
            GetComponent<SkinnedMeshRenderer>().material = blueM;
        }
        if (current.element == Element.Green)
        {
            GetComponent<SkinnedMeshRenderer>().material = greenM;
        }
    }

    private void Update()
    {
        elementType current = GetComponent<elementType>();
        if (current.element == Element.Red)
        {
            GetComponent<SkinnedMeshRenderer>().material = redM;
        }
        if (current.element == Element.Blue)
        {
            GetComponent<SkinnedMeshRenderer>().material = blueM;
        }
        if (current.element == Element.Green)
        {
            GetComponent<SkinnedMeshRenderer>().material = greenM;
        }
    }

}
