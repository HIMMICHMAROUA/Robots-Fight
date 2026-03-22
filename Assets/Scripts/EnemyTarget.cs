using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " a pris des dégâts. Vie restante : " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();

        }
    }

    void Die()
    {
        Debug.Log(transform.name + " est éliminé !");
        FindObjectOfType<Player>().AddKill();
        Destroy(gameObject); 
    }
}
