using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour {

    public int damagePerShot = 20;
    float timer = 0;
    public float alive = 0.5f;

    //public AudioClip collision;

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            EnemyHealth enemyHealth = c.attachedRigidbody.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot);
            } else {
                AudioSource collision = GetComponent<AudioSource>();
                //collision_sound.PlayOneShot(collision, 1F);
                collision.Play();
            }
            
        }
        Destroy(this.gameObject, 0.1f);
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
