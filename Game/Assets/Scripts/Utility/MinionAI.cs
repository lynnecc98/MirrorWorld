using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(UnityEngine.AI.NavMeshAgent)))]
public class MinionAI : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent navMesh;
    private Animator minionAnimator;

    public GameObject[] waypoints;
    int currWaypoint;
    Vector3 finalDes;
    Vector3 m;
    enum AIState { Stationary, Moving };
    AIState aiState;
    float t;

    void Start()
    {
        navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
        minionAnimator = GetComponent<Animator>();
        currWaypoint = -1;
        SetNextWaypoint();
        aiState = AIState.Stationary;
    }

    private void SetNextWaypoint() {
        if (waypoints.Length == 0){
            Debug.Log("There are no waypoints");
        }

        //if (currWaypoint > waypoints.Length)
        //{
        //    currWaypoint = 0;
        //}
        //else {
        //    currWaypoint++;
        //}
        //navMesh.SetDestination(waypoints[currWaypoint].transform.position);

        currWaypoint += 1;
        if (currWaypoint <= waypoints.Length - 1)
        {
            navMesh.SetDestination(waypoints[currWaypoint].transform.position);
        }
    }

    void Update()
    {
        minionAnimator.SetFloat("vely", navMesh.velocity.magnitude / navMesh.speed);
        switch (aiState)
        {
            case AIState.Stationary:
                if (navMesh.remainingDistance <= 0 && navMesh.pathPending != true)
                {
                    SetNextWaypoint();
                }
                if (currWaypoint == waypoints.Length)
                {
                    aiState = AIState.Moving;
                    currWaypoint = 0;
                }
                break;
            case AIState.Moving:
                predictWayPoint();
                if(Vector3.Distance(this.transform.position, GameObject.FindGameObjectWithTag("MovingWaypoint").transform.position) < 1.0f)
                {
                    aiState = AIState.Stationary;
                    currWaypoint = -1;
                    SetNextWaypoint();
                }
                break;
        }
    }

    private void predictWayPoint()
    {
        GameObject movingWaypoint = GameObject.FindGameObjectWithTag("MovingWaypoint");
        VelocityReporter v = movingWaypoint.GetComponent<VelocityReporter>();
        Vector3 minionVelcoty = this.GetComponent<Rigidbody>().velocity;
        Vector3 waypointVelocity = v.velocity;
        Vector3 waypointPosition = movingWaypoint.transform.position;
        Vector3 minionPosition = this.transform.position;

        Vector3 pos = (minionPosition - waypointPosition);
        Vector3 vel = (waypointVelocity - minionVelcoty);
        t = pos.magnitude / vel.magnitude;
        m = waypointVelocity * t;
        finalDes = new Vector3(m.x + waypointPosition.x, m.y + waypointPosition.y, m.z + waypointPosition.z);
        navMesh.SetDestination(finalDes);
    }
}
