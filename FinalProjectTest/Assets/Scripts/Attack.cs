﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attack : MonoBehaviour {

    public int damagePerShot = 25;
    public float timeBetweenBullets = 0.15f;


    public GameObject[] obj;

    int shootableMask;
    float timer;

    elementType element;
    Element curElement;


    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        element = GetComponent<elementType>();
        curElement = element.element;
    }


    // Update is called once per frame
    void Update() {

        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }
        element = GetComponent<elementType>();
        curElement = element.element;
        print(element.element);

    }

    void Shoot()
    {
        timer = 0f;
        Vector3 curPos = this.transform.position;
        Quaternion curRot = this.transform.rotation;
        Vector3 curFor = this.transform.forward;
        GameObject select = obj[0];
        switch (curElement) {
            case Element.Neutral:
                select = obj[0];
                break;
            case Element.Red:
                select = obj[1];
                break;
            case Element.Blue:
                select = obj[2];
                break;
            case Element.Green:
                select = obj[3];
                break;
        }
        GameObject objCur = (GameObject)Instantiate(select, curPos, curRot);
        Rigidbody objRig = objCur.GetComponent<Rigidbody>();
        objRig.velocity = curFor * 10;



    }
}
