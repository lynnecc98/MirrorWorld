using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomType : MonoBehaviour
{

    Renderer rend;
    Color[] colors = new Color[3];
    elementType myElement;

    void Start()
    {
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();

        //Fetch element type script attached to character
        myElement = GetComponent<elementType>();

        //Color types
        colors[0] = Color.red;
        colors[1] = Color.green;
        colors[2] = Color.blue;

        //Set the main Color of the Material to green
        rend.material.SetColor("_Color", colors[Random.Range(0, colors.Length)]);

        //Set the enum character type in other script
        if (rend.material.color == Color.red)
        {
            myElement.element = Element.Red;
        }
        else if (rend.material.color == Color.green)
        {
            myElement.element = Element.Green;
        }
        else if (rend.material.color == Color.blue)
        {
            myElement.element = Element.Blue;
        }
    }
}