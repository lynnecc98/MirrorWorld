using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : MonoBehaviour {
    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();
	}

    private void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody && c.attachedRigidbody.gameObject == GameObject.FindGameObjectWithTag("Player")) {
            anim.SetBool("InBoat", true);
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody && c.attachedRigidbody.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            anim.SetBool("InBoat", false);
        }
    }

}
