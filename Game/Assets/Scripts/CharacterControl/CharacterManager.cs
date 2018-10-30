using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour {


    public CharacterInputController[] ControllableCharacters = {}; 

    public ThirdPersonCamera thirdPersonCamera;

    public string CameraPositionMarkerName = "CamPos";


    protected int nextCharacterIndex = 0;

    protected void disableAllCharacters() {

        foreach (CharacterInputController c in ControllableCharacters)
        {
            c.enabled = false;
        }
            
    }

    protected void setCharacter(int charIndex) {

        disableAllCharacters();

        if (charIndex < 0)
            charIndex = 0;

        if (charIndex >= ControllableCharacters.Length)
            charIndex = ControllableCharacters.Length - 1;

        ControllableCharacters[charIndex].enabled = true;

        thirdPersonCamera.desiredPose = ControllableCharacters[charIndex].transform.Find(CameraPositionMarkerName);

        Debug.Log("Character " + ControllableCharacters[charIndex].Name + " was selected.");
   
    }

    protected void incrementCharacterIndex() {

        ++nextCharacterIndex;

        if (nextCharacterIndex < 0 || nextCharacterIndex >= ControllableCharacters.Length)
            nextCharacterIndex = 0;
    }

    protected void toggleCharacter() {

        setCharacter(nextCharacterIndex);

        incrementCharacterIndex();
    }

	
	void Start () {
		
        if (thirdPersonCamera == null)
            Debug.LogError("camera must be set");

        setCharacter(nextCharacterIndex);
        incrementCharacterIndex();

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.T))
        {
            Debug.Log("Character toggled");
            toggleCharacter();

        }
		
	}
}
