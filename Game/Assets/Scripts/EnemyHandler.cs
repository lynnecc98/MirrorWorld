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


    private void Start()
    {
        elementType curElement = GetComponent<elementType>();
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

        }
    }

}
