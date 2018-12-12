using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInteraction : MonoBehaviour {
    public AudioClip door;

    private void OnTriggerEnter(Collider c)
    {
        //EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
       
        if (c.attachedRigidbody && c.attachedRigidbody.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            KeyCollector bc = c.attachedRigidbody.gameObject.GetComponent<KeyCollector>();
            if (bc.hasKey){
                AudioSource door_open = GetComponent<AudioSource>();
                //door_open.Play();
                door_open.PlayOneShot(door, 2F);
                Destroy(this.gameObject, 1F);
            }
        }
    }
}
