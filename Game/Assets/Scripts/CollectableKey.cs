using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableKey : MonoBehaviour {

    private void OnTriggerEnter(Collider c)
    {
        //.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
        Destroy(this.gameObject);
        if (c.attachedRigidbody){
            KeyCollector bc = c.attachedRigidbody.gameObject.GetComponent<KeyCollector>();
            bc.ReceiveKey();
        }
    }
}
