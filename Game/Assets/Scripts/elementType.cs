using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { Red, Green, Blue, Neutral };

public class elementType : MonoBehaviour {

    
    public Element element;

    // Use this for initialization
    void Start () {
        element = Element.Neutral;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
