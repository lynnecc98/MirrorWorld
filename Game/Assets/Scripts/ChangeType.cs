using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeType : MonoBehaviour
{
    public class Type : IComparable<Type>
    {
        public string type;
        public int points;
        public Type(string type, int points)
        {
            type = this.type;
            points = this.points;
        }
        public void addPoints(int point)
        {
            points += point;
        }

        public int CompareTo(Type other)
        {
            if (other == null)
            {
                return 1;
            }
            return points - other.points;
        }
    }

    public Type red, green, blue;
    public Slider redSlider, blueSlider, greenSlider;
    public Material currentMaterial;
    public Material redM, greenM, blueM;
    elementType element;
    List<Material> mats;
    public GameObject Object;
    // Use this for initialization
    void Start()
    {
        red = new Type("Red", 0);
        green = new Type("Green", 0);
        blue = new Type("Blue", 0);
        element = GetComponent<elementType>();
        // define the materials
        mats = new List<Material>(new Material[] { redM, greenM, blueM });
        Object.GetComponent<SkinnedMeshRenderer>().material = currentMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        redSlider.value = red.points;
        blueSlider.value = blue.points;
        greenSlider.value = green.points;

        if (red.points > blue.points && red.points > green.points)
        {
            Object.GetComponent<SkinnedMeshRenderer>().material = redM;
            element.element = Element.Red;
            currentMaterial = redM;
        }
        else if (green.points > blue.points && green.points > red.points)
        {
            Object.GetComponent<SkinnedMeshRenderer>().material = greenM;
            element.element = Element.Green;
            currentMaterial = greenM;
        }
        else if (blue.points > red.points && blue.points > green.points)
        {
            // blue material
            Object.GetComponent<SkinnedMeshRenderer>().material = blueM;
            element.element = Element.Blue;
            currentMaterial = blueM;
        }
        else {
            Object.GetComponent<SkinnedMeshRenderer>().material = currentMaterial;
        }
    }
}