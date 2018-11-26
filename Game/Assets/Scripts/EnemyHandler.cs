using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{

    public GameObject enemy;
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
            enemys[i] =  Instantiate(enemy, spawnPoints[i].position, spawnPoints[i].rotation);
            elementType element = enemys[i].GetComponent<elementType>();
            element.element = curElement.element;

        }
    }

}
