using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInteraction : MonoBehaviour {

    private void OnTriggerEnter(Collider c)
    {
        //EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
       
        if (c.attachedRigidbody)
        {
            KeyCollector bc = c.attachedRigidbody.gameObject.GetComponent<KeyCollector>();
            if (bc.hasBall){
                Destroy(this.gameObject);
            }
        }
    }
}
