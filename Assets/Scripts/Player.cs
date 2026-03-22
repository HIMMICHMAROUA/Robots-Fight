using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Santé & Dégâts")]
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private float currentHealth ;

    [Header("Interface (UI)")]
    [SerializeField] 
    private GameObject gameOverPanel;
    [SerializeField] 
    private GameObject victoryPanel;
    
    [Header("Sons")]
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip destroySound;

    [Header("Condition de Victoire")]
    [SerializeField] 
    private int enemiesToKillForVictory = 10; // Le nombre d'ennemis à tuer
    private int enemiesKilled = 0; // Le compteur actuel

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(34f);
        }
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " a pris des dégâts. Vie restante : " + currentHealth);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(hitSound);

        if (currentHealth <= 0f)
        {
            audioSource.PlayOneShot(destroySound);
            GameOver();
        }
    }
    public void AddKill()
    {
        enemiesKilled++;
        Debug.Log("Ennemi tué ! Total : " + enemiesKilled + "/" + enemiesToKillForVictory);

        // On vérifie si on a atteint l'objectif
        if (enemiesKilled >= enemiesToKillForVictory)
        {
            Victory();
        }
    }

    void GameOver()
    {
        PauseMenu.isOn = true;
        Debug.Log(transform.name + " est mort.");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
            

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
    void Victory()
    {
        PauseMenu.isOn = true;
        Debug.Log("Victoire ! Vous avez tué " + enemiesToKillForVictory + " ennemis.");

        // Affiche le panneau de victoire
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // Affiche la souris pour cliquer sur "Rejouer" ou "Menu"
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Met le jeu en pause
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); 
    }
    public float GetHealthPct()
    {
        return currentHealth / maxHealth;
    }

}
