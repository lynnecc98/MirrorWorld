using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCreator : MonoBehaviour {

    public GameObject reflection;
    public Transform spawnPoint;
    GameObject currentObject;

    // Use this for initialization
    void Start () {
        int random = Random.Range(0, 3);
        currentObject = Instantiate(reflection, spawnPoint.position + new Vector3(0f, .85f, 0f), spawnPoint.rotation);
    }
	
}
