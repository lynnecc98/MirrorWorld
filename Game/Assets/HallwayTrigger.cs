using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayTrigger : MonoBehaviour {
    public bool triggered = false;
    private bool inTrigger = false;
    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("triggered");
                triggered = !triggered;
            }
        }
    }

    //private void OnTriggerStay(Collider player)
    //{
    //    Debug.Log("inside");
    //    if (player.attachedRigidbody)
    //    {

    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
