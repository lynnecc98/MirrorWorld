using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableKey : MonoBehaviour {

    [SerializeField] private Image customImage;

    private void OnTriggerEnter(Collider c)
    {
        //.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);

        if (c.attachedRigidbody && c.attachedRigidbody.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            KeyCollector bc = c.attachedRigidbody.gameObject.GetComponent<KeyCollector>();
            bc.ReceiveKey();
            customImage.enabled = true;
            Destroy(this.gameObject);
        }
    }
}
