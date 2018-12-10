using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject player;
    public int startingHealth = 100;
    public int currentHealth;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public bool damaged;
    public bool isDead = false;
    public Slider healthSlider;

    public AudioClip falling;
    public AudioClip player_hurt;



    void Awake()
    {
        currentHealth = startingHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        if (damaged)
        {
            damageImage.color = flashColour;
            GameObject.Find("DamageImage").GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            GameObject.Find("DamageImage").GetComponent<CanvasGroup>().alpha = 0f;
        }
        damaged = false;

        if (isDead == true)
        {
            Destroy(gameObject, 2f);
            SceneManager.LoadScene("LoseScene");
        }
        if (player.transform.position.y < 0)
        {
            AudioSource fall = GetComponent<AudioSource>();
            fall.PlayOneShot(falling, 0.1F);
        }

        if (player.transform.position.y < -20)
        {
            isDead = true;
        }
        
    }

    public void TakeDamage(int amount)
    {
        AudioSource hurt = GetComponent<AudioSource>();
        hurt.PlayOneShot(player_hurt, 0.5F);

        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;


        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void heal(int amount)
    {
        currentHealth += amount;

        healthSlider.value = currentHealth;

    }

    void Death()
    {
        isDead = true;
    }





}