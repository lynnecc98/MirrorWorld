using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableHallway : MonoBehaviour {
    public GameObject connectedTrigger;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HallwayTrigger ht = connectedTrigger.GetComponent<HallwayTrigger>();
        if (ht.triggered){
            this.GetComponent<MeshRenderer>().enabled = true;
        } else {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
	}
}
