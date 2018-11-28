using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossHandler : MonoBehaviour {

    public GameObject miniBoss, creature;
    public int count;
    public Transform[] spawnPoints;
    public GameObject[] enemys;
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

    // Use this for initialization
    void Start () {
        deadlist = new List<int>();
        enemys = new GameObject[count];

        enemys[0] = Instantiate(miniBoss, spawnPoints[0].position, spawnPoints[0].rotation);
        EnemyMovement move = enemys[0].GetComponent<EnemyMovement>();
        move.spawnpoint = spawnPoints[0];


        for (int i = 1; i < count; i++)
        {
            enemys[i] = Instantiate(creature, spawnPoints[i].position, spawnPoints[i].rotation);
            EnemyMovement temp = enemys[i].GetComponent<EnemyMovement>();
            temp.spawnpoint = spawnPoints[i];
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
