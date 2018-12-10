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
                triggered = !triggered;
                AudioSource sparkle = GetComponent<AudioSource>();
                sparkle.Play();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
