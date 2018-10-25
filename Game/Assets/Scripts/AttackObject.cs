using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour {

    public int damagePerShot = 20;
    float timer = 0;
    public float alive = 2.5f;

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            EnemyHealth enemyHealth = c.attachedRigidbody.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot);
            }
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > alive)
        {
            Destroy(this.gameObject);
        }
    }
}
