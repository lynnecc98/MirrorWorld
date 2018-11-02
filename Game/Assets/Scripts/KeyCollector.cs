using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Animator))]
public class KeyCollector : MonoBehaviour {
    public bool hasKey = false;
   // public Rigidbody ballPrefab;

    private Animator anim;
    private Transform handHold;
    private Rigidbody currBall;

    private void Awake()
    {
        //handHold = this.transform.Find("mixamorig:Hips/mixamorig:Spine/" +
                                       //"mixamorig:Spine1/mixamorig:Spine2/" +
                                       //"mixamorig:LeftShoulder/mixamorig:LeftArm/" +
                                       //"mixamorig:LeftForeArm/mixamorig:LeftHand/" +
                                       //"BallHoldSpot");

        //if (ballPrefab == null)
        //{
        //    Debug.Log("Ball prefab could not be found");
        //}
        //if (handHold == null)
        //{
        //    Debug.Log("Hand hold could not be found");
        //} 

        //anim = GetComponent<Animator>();
    }

    public void ReceiveKey() {
        hasKey = true;
        //currBall = Instantiate<Rigidbody>(ballPrefab);
        //currBall.transform.parent = handHold;
        //currBall.transform.localPosition = Vector3.zero;
        //currBall.isKinematic = true;
    }

    public void ThrowBall()
    {
        //currBall.transform.parent = null;
        //currBall.isKinematic = false;
        //currBall.velocity = Vector3.zero;
        //currBall.angularVelocity = Vector3.zero;
        //currBall.AddForce(this.transform.forward * 5f, ForceMode.VelocityChange);
        //currBall = null;
        //hasBall = false;
    }

    void FixedUpdate()
    {
        //if (currBall != null && Input.GetButtonDown("Fire1"))
        //{
        //    anim.SetBool("throw", true);
        //}
        //else {
        //    anim.SetBool("throw", false);
        //}
    }
}
