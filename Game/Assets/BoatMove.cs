using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour {

    public GameObject player;
    bool onBoat = false;
    Vector3 oldpos;
    //Quaternion oldrot;

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
            /*player.transform.rotation = new Quaternion( 
                player.transform.rotation.x + (this.transform.rotation.x - oldrot.x), 
                player.transform.rotation.y + (this.transform.rotation.y - oldrot.y), 
                player.transform.rotation.z + (this.transform.rotation.z - oldrot.z),
                player.transform.rotation.w + (this.transform.rotation.w - oldrot.w)
                );*/
        }
        oldpos = this.transform.position;
        //oldrot = this.transform.rotation;
    }
}
