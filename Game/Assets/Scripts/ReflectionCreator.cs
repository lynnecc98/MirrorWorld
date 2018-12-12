using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCreator : MonoBehaviour {

    public GameObject reflection;
    public Transform spawnPoint;

    // Use this for initialization
    void Start () {
        Instantiate(reflection, spawnPoint.position + new Vector3(0f, .85f, 0f), spawnPoint.rotation);
    }
	
}
