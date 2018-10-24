using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Normal,
    Fire,
    Water,
    Grass
};

public class Attack : MonoBehaviour {

    public int damagePerShot = 25;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    public AttackType state = AttackType.Normal;

    public GameObject[] obj;

    int shootableMask;
    float timer;


    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
    }


    // Update is called once per frame
    void Update() {

        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        if (Input.GetButton("Jump"))
        {
            if(state == AttackType.Normal)
            {
                state = AttackType.Fire;
            }
            else if (state == AttackType.Fire)
            {
                state = AttackType.Water;
            }
            else if (state == AttackType.Water)
            {
                state = AttackType.Grass;
            }
            else if (state == AttackType.Grass)
            {
                state = AttackType.Fire;
            }
        }




    }

    void Shoot()
    {
        timer = 0f;
        Vector3 curPos = this.transform.position;
        Quaternion curRot = this.transform.rotation;
        Vector3 curFor = this.transform.forward;
        GameObject select = obj[0];
        switch (state) {
            case AttackType.Normal:
                select = obj[0];
                break;
            case AttackType.Fire:
                select = obj[1];
                break;
            case AttackType.Water:
                select = obj[2];
                break;
            case AttackType.Grass:
                select = obj[3];
                break;
        }
        GameObject objCur = (GameObject)Instantiate(select, curPos, curRot);
        Rigidbody objRig = objCur.GetComponent<Rigidbody>();
        objRig.velocity = curFor * 5;



    }
}
