using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public int count = 1;
    public GameObject enemy;
    public Transform spawnPoint;
    GameObject currentObject;
    public bool making = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            if (count == 1 && making)
            {
                currentObject = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
                making = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            Destroy(currentObject);
            making = true;
        }
    }

    public void Update()
    {
        if (currentObject != null)
        {
            EnemyHealth health = currentObject.GetComponent<EnemyHealth>();
            if (health.isDead == true)
            {
                count--;
            }
        }
    }
 
    

}
