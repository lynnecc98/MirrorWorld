using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableKey : MonoBehaviour {

    private void OnTriggerEnter(Collider c)
    {
        //.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);

        if (c.attachedRigidbody && c.attachedRigidbody.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            KeyCollector bc = c.attachedRigidbody.gameObject.GetComponent<KeyCollector>();
            bc.ReceiveKey();
            Destroy(this.gameObject);
        }
    }
}
