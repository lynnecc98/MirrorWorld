using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInteraction : MonoBehaviour {

    private void OnTriggerEnter(Collider c)
    {
        //EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
       
        if (c.attachedRigidbody && c.attachedRigidbody.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            KeyCollector bc = c.attachedRigidbody.gameObject.GetComponent<KeyCollector>();
            if (bc.hasKey){
                Destroy(this.gameObject);
            }
        }
    }
}
