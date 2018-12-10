using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOnOff : MonoBehaviour {

    public List<Material> screensMaterials;

    public bool isOn = true;

    List<Color> originalAlbedoColors = new List<Color>(), originalEmissionColors = new List<Color>();
    List<Texture> originalAlbedoTextures = new List<Texture>(), originalEmissionTextures = new List<Texture>();


    void Awake()
    {
        foreach (Material screenMaterial in screensMaterials)
        {
            originalAlbedoColors.Add(screenMaterial.color);
            originalAlbedoTextures.Add(screenMaterial.mainTexture);
            originalEmissionColors.Add(screenMaterial.GetColor("_EmissionColor"));
            originalEmissionTextures.Add(screenMaterial.GetTexture("_EmissionMap"));
        }

        if(!isOn)
        {
            turnOff();
        }

		gameObject.SetActive (false);
    }


	void OnEnable()
	{
		if (isOn)
		{
			turnOff();
		}
		else
		{
			turnOn();
		}
	}


	void OnDestroy()
	{
		turnOn();
	}


	void turnOn()
	{
		for(int i=0; i<screensMaterials.Count; i++)
		{
			screensMaterials[i].color = originalAlbedoColors[i];
			screensMaterials[i].mainTexture = originalAlbedoTextures[i];
			screensMaterials[i].SetColor("_EmissionColor", originalEmissionColors[i]);
			screensMaterials[i].SetTexture("_EmissionMap", originalEmissionTextures[i]);
		}
	}
		

    void turnOff()
    {
        foreach(Material screenMaterial in screensMaterials)
        {
            screenMaterial.color = Color.black;
            screenMaterial.mainTexture = null;
            screenMaterial.SetColor("_EmissionColor", Color.black);
            screenMaterial.SetTexture("_EmissionMap", null);
        }
    }

}
