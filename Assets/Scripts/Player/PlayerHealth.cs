using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float heartSpacing;
    public RectTransform heartsContainer;
    public GameObject heartPrefab;
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
        UpdateHearts();
    }

    public int TakeDamage(int damage)
    {
        currentHealth -= damage;
        isTakingDamage = true;
        UpdateHearts();
        return currentHealth;
    }

    private void UpdateHearts()
    {
        foreach (RectTransform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < currentHealth; i++)
        {
            GameObject heartInstanceGO = Instantiate(heartPrefab, heartsContainer);
            RectTransform heartInstance = heartInstanceGO.GetComponent<RectTransform>();
    
            float heartWidth = heartInstance.rect.width;
            heartInstance.anchoredPosition = new Vector2(i * heartSpacing + heartWidth / 2f, 0f);
        }
    }
}
