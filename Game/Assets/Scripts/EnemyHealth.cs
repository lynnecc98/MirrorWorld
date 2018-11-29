using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    public static int miniBossCount = 0;

    elementType element;
    ChangeType change;



    void Awake()
    {
        currentHealth = startingHealth;
        element = GetComponent<elementType>();
        change = GameObject.FindGameObjectWithTag("Player").GetComponent<ChangeType>();
    }

    void Update()
    {
        if (isDead  == true)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        isDead = true;

        if(this.CompareTag("Boss")) { 
            miniBossCount++;
            if (miniBossCount == 1)
            {
                GameObject.Find("Hint1PopUp").GetComponent<Image>().enabled = true;
            }

            if (miniBossCount >= 2)
            {
                if (GameObject.Find("Mesh").GetComponent<elementType>().element == Element.Red)
                {
                    GameObject.Find("HintRed").GetComponent<Image>().enabled = true;
                }
                if (GameObject.Find("Mesh").GetComponent<elementType>().element == Element.Blue)
                {
                    GameObject.Find("HintBlue").GetComponent<Image>().enabled = true;
                }
                if (GameObject.Find("Mesh").GetComponent<elementType>().element == Element.Green)
                {
                    GameObject.Find("HintGreen").GetComponent<Image>().enabled = true;
                }
            }
        }

        if (element.element == Element.Red)
        {
            change.red.addPoints(1);
            
        }
        if (element.element == Element.Blue)
        {
            change.blue.addPoints(1);
            
        }
        if (element.element == Element.Green)
        {
            change.green.addPoints(1);
            
        }
    }



}