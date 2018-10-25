using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public GameObject player;
    elementType element;
    ChangeType change;


    void Awake()
    {
        currentHealth = startingHealth;
        element = GetComponent<elementType>();
        change = player.GetComponent<ChangeType>();
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

    void Death()
    {
        isDead = true;
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