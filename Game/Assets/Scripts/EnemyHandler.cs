using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyHandler : MonoBehaviour
{

    public GameObject enemyR, enemyG, enemyB;
    public int count;
    public Transform[] spawnPoints;
    public GameObject[] enemys;
    public Material redM, greenM, blueM;
    List<int> deadlist;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                if (!deadlist.Contains(i))
                {
                    EnemyMovement move = enemys[i].GetComponent<EnemyMovement>();
                    if (move != null)
                    {
                        move.enemyState = AIStates.attack;
                    }
                }
                
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                if (!deadlist.Contains(i))
                {
                    EnemyMovement move = enemys[i].GetComponent<EnemyMovement>();
                    if (move != null)
                    {
                        move.enemyState = AIStates.idle;
                    }
                }
            }
        }
    }

    private void Start()
    {
        elementType curElement = GetComponent<elementType>();
        deadlist = new List<int>();
        enemys = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            if (curElement.element == Element.Red)
            {
                enemys[i] = Instantiate(enemyR, spawnPoints[i].position, spawnPoints[i].rotation);
            }
            if (curElement.element == Element.Blue)
            {
                enemys[i] = Instantiate(enemyB, spawnPoints[i].position, spawnPoints[i].rotation);
            }
            if (curElement.element == Element.Green)
            {
                enemys[i] = Instantiate(enemyG, spawnPoints[i].position, spawnPoints[i].rotation);
            }
            elementType element = enemys[i].GetComponent<elementType>();
            element.element = curElement.element;
            EnemyMovement move = enemys[i].GetComponent<EnemyMovement>();
            move.spawnpoint = spawnPoints[i];
        }
    }

    private void Update()
    {
        for (int i = 0; i < count; i++)
        {
            if (!deadlist.Contains(i))
            {
                EnemyHealth health = enemys[i].GetComponent<EnemyHealth>();
                if (health.isDead)
                {
                    deadlist.Add(i);
                }
            }
            
        }
    }

}
