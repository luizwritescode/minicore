using UnityEngine;
using System.Collections;

public class ShootableObject : MonoBehaviour {

    public float maxHealth;
    //The box's current health point total
    public float currentHealth;

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damageAmount)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0) 
        {
            //if health has fallen below zero, deactivate it 
            gameObject.SetActive (false);
        }
    }
}   