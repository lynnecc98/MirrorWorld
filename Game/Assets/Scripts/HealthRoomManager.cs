using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthRoomManager : MonoBehaviour {

    private Animator anim;
    float timer = 0;
    bool inRoom = false;
    public bool healing;
    public float flashSpeed = 1.0f;
    private float t = 0;
    public Color flashColour = new Color(0f, 1f, 0f, 0.1f);

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            inRoom = true;
            anim = other.GetComponent<Animator>();
            anim.SetBool("healing", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            inRoom = false;
            GameObject.Find("HealImage").GetComponent<CanvasGroup>().alpha = 0f;
            anim = other.GetComponent<Animator>();
            anim.SetBool("healing", false);
        }
    }


    private void Update()
    {
        if(inRoom)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player").gameObject;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            

            timer += Time.deltaTime;
            if (timer > 1)
            {
                timer = 0;
                if (playerHealth.currentHealth < playerHealth.startingHealth)
                {
                    healing = true;
                    playerHealth.heal(5);
                    AudioSource heal_audio = GetComponent<AudioSource>();
                    if (!heal_audio.isPlaying)
                    {
                        heal_audio.Play();
                    }
                    
                }
            }

        }

        if (healing)
        {
            
            GameObject.Find("HealImage").GetComponent<Image>().color = flashColour;
            GameObject.Find("HealImage").GetComponent<CanvasGroup>().alpha = 1f;

        }
        else
        {
            GameObject.Find("HealImage").GetComponent<Image>().color = Color.Lerp(GameObject.Find("HealImage").GetComponent<Image>().color, Color.clear, t);
            if (t < 1)
            {
                t += Time.deltaTime / flashSpeed;
            }
            GameObject.Find("HealImage").GetComponent<CanvasGroup>().alpha = 0f;
        }
        healing = false;

    }
}
