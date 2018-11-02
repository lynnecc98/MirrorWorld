using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCreator : MonoBehaviour {

    public GameObject reflection;
    public Transform spawnPoint;
    GameObject currentObject;

    // Use this for initialization
    void Start () {
        currentObject = Instantiate(reflection, spawnPoint.position, spawnPoint.rotation);
    }
	
}
