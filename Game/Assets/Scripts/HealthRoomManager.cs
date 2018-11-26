using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRoomManager : MonoBehaviour {

    float timer = 0;
    bool inRoom = false;

	void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            inRoom = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            inRoom = false;
        }
    }


    private void Update()
    {
        if(inRoom)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player").gameObject;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            timer += Time.deltaTime;
            if (timer > 1)
            {
                timer = 0;
                if (playerHealth.currentHealth < playerHealth.startingHealth)
                {
                    playerHealth.heal(5);
                }
            }

        }
        
    }
}
