using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum roomType
{
    enemyRoomR, enemyRoomG, enemyRoomB, playerRoom, healthRoom, keyRoom, triggerRoom, goalRoom, bossRoom
};

public class EnvironmentSetUp : MonoBehaviour {

    public GameObject[] floor;
    public GameObject[] BridgeL;
    public GameObject [] EnemyRoom;
    public GameObject TriggerRoom;
    public GameObject HealthRoom;
    public GameObject StartingRoom;
    public GameObject endRoom;
    public GameObject keyRoom;
    public GameObject bossRoom;
    public GameObject bridge;
    public GameObject trig;
    public GameObject playr;

    List <roomType> rooms = new List<roomType>();
    roomType enemy1 = roomType.enemyRoomR;
    roomType enemy2 = roomType.enemyRoomG;
    roomType enemy3 = roomType.enemyRoomB;
    roomType player = roomType.playerRoom;
    roomType health = roomType.healthRoom;
    roomType trigger = roomType.triggerRoom;
    roomType key = roomType.keyRoom;
    roomType goal = roomType.goalRoom;
    roomType boss = roomType.bossRoom;

    int[] bridgeMap1 = {1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1};
    int[] bridgeMap2 = {1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1};

    List<GameObject> curBridges;

    bool trigg = false;

    GameObject curTrig;




    // Use this for initialization
    void Start () {
        curBridges = new List<GameObject>();
        rooms.Add(player);
        for (int i = 0; i< 3; i++)
        {
            rooms.Add(enemy1);
        }
        for (int i = 0; i < 3; i++)
        {
            rooms.Add(enemy2);
        }
        for (int i = 0; i < 3; i++)
        {
            rooms.Add(enemy3);
        }
        for (int i = 0; i < 2; i++)
        {
            rooms.Add(boss);
        }
        rooms.Add(health);
        rooms.Add(trigger);
        rooms.Add(goal);
        rooms.Add(key);


        for(int i = 1; i < rooms.Count; i++)
        {
            roomType temp = rooms[i];
            int index = Random.Range(i, rooms.Count);
            rooms[i] = rooms[index];
            rooms[index] = temp;
        }


        for(int i = 0; i < rooms.Count; i++)
        {
            if(rooms[i] == roomType.enemyRoomR || rooms[i] == roomType.enemyRoomB || rooms[i] == roomType.enemyRoomG) 
            {
                int random = Random.Range(0, 2);
                GameObject temp =  Instantiate(EnemyRoom[random], floor[i].transform.position, floor[i].transform.rotation);
                elementType tempElement = temp.GetComponent<elementType>();
                if (rooms[i] == roomType.enemyRoomR)
                {
                    tempElement.element = Element.Red;
                }
                if (rooms[i] == roomType.enemyRoomB)
                {
                    tempElement.element = Element.Blue;
                }
                if (rooms[i] == roomType.enemyRoomG)
                {
                    tempElement.element = Element.Green;
                }
                
            }
            if(rooms[i] == roomType.bossRoom)
            {
                Instantiate(bossRoom, floor[i].transform.position, floor[i].transform.rotation);
            }
            if ( rooms[i] == roomType.healthRoom)
            {
                Instantiate(HealthRoom, floor[i].transform.position, floor[i].transform.rotation);

            }
            if (rooms[i] == roomType.goalRoom)
            {
                Instantiate(endRoom, floor[i].transform.position, floor[i].transform.rotation);
                //playr.transform.position = floor[i].transform.position;
            }
            if (rooms[i] == roomType.playerRoom)
            {
                Instantiate(StartingRoom, floor[i].transform.position, floor[i].transform.rotation);
                playr.transform.position = floor[i].transform.position;
            }
            if (rooms[i] == roomType.triggerRoom)
            {
                Instantiate(TriggerRoom, floor[i].transform.position, floor[i].transform.rotation);
                curTrig = Instantiate(trig, floor[i].transform.position+ new Vector3(0, 0.5f, 0), floor[i].transform.rotation);
            }
            if (rooms[i] == roomType.keyRoom)
            {
                Instantiate(keyRoom, floor[i].transform.position, floor[i].transform.rotation);
            }
        }

        for (int i = 0; i < BridgeL.Length; i++)
        {
            if (bridgeMap1[i] == 1)
            {
                    GameObject temp = Instantiate(bridge, BridgeL[i].transform.position, BridgeL[i].transform.rotation);
                    curBridges.Add(temp);
            }
        }




    }


    private void Update()
    {
        HallwayTrigger hallway = curTrig.GetComponent<HallwayTrigger>();
        if (trigg == false && hallway.triggered == true)
        {
            for(int i = 0; i < curBridges.Count; i++)
            {
                Destroy(curBridges[i]); 
            }
            curBridges.Clear();
            for(int i = 0; i < bridgeMap2.Length; i++)
            {
                if (bridgeMap2[i] == 1)
                {
                    GameObject temp = Instantiate(bridge, BridgeL[i].transform.position, BridgeL[i].transform.rotation);
                    curBridges.Add(temp);
                }
            }
            trigg = hallway.triggered;
        }

        if (trigg == true && hallway.triggered == false)
        {
            for (int i = 0; i < curBridges.Count; i++)
            {
                Destroy(curBridges[i]);
            }
            curBridges.Clear();
            for (int i = 0; i < bridgeMap1.Length; i++)
            {
                if (bridgeMap1[i] == 1)
                {
                    GameObject temp = Instantiate(bridge, BridgeL[i].transform.position, BridgeL[i].transform.rotation);
                    curBridges.Add(temp);
                }
            }
            trigg = hallway.triggered;
        }

        

    }

}
