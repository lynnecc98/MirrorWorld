using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class Attack : MonoBehaviour {
    public Animator anim;

    public int damagePerShot = 25;
    public float timeBetweenBullets = 0.15f;

    //public AudioClip laser;

    public bool animShoot = false;

    public GameObject[] obj;

    int shootableMask;
    float timer;
    float shootingTimer;

    elementType element;
    Element curElement;
    Vector3 shift = new Vector3(0f, 0.5f, 0f);
    bool shooting = false;

    


    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        element = GetComponent<elementType>();
        curElement = element.element;
        anim = GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update() {

        timer += Time.deltaTime;
        shootingTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {

            //Shoot();
            animShoot = true;
            anim.SetBool("attack", animShoot);
            AudioSource laser = GetComponent<AudioSource>();
            laser.Play();
            shooting = true;
            shootingTimer = 0;
        } else
        {
            animShoot = false;
            anim.SetBool("attack", animShoot);
        }
        if (shootingTimer >= 0.1 && shooting) { 
            Shoot();
            shooting = false;
        } 
        element = GetComponent<elementType>();
        curElement = element.element;
        //print(element.element);

    }

    void Shoot()
    {
   
        timer = 0f;
        Vector3 curPos = this.transform.position;
        Quaternion curRot = this.transform.rotation;
        Vector3 curFor = this.transform.forward;
        GameObject select = obj[0];
        switch (curElement)
        {
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
        GameObject objCur = (GameObject)Instantiate(select, curPos + curFor + shift, curRot);
        Rigidbody objRig = objCur.GetComponent<Rigidbody>();
        objRig.velocity = Vector3.Normalize(curFor) * 20;



    }
}
