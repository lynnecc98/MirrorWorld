using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour {

    public GameObject player;
    bool onBoat = false;
    float counter = 0;
    Vector3 oldpos;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        oldpos = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            onBoat = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            onBoat = false;
        }
    }

    // Update is called once per frame
    void Update () {
        if (onBoat)
        {
            player.transform.position = player.transform.position + (this.transform.position - oldpos);
        }
        oldpos = this.transform.position;
    }
}
