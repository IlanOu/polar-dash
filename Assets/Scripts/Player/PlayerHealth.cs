using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
     public int currentHealth;
    [HideInInspector] public bool isTakingDamage;

    public static PlayerHealth instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il existe déjà une instance de PlayerHealth dans cette scène");
            return;
        }
        instance = this;
    }
    void Start()
    {
        SetMaxHealth();
    }

    void Update()
    {
        isTakingDamage = false;
    }

    void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }

    public int TakeDamage(int damage)
    {
        currentHealth -= damage;
        isTakingDamage = true;
        return currentHealth;
    }
}
