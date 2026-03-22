using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform thrusterFuelFill;
    [SerializeField] private RectTransform healthFill;
    [SerializeField] private Text ammoText; 
    [SerializeField] private GameObject pauseMenu;

    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Player player;
    [SerializeField] private WeaponManager weaponManager;


    private void Start()
    {
        PauseMenu.isOn = false;
    }


    void Update()
    {
        SetAmmoAmount(20);
        if (controller != null)
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());
            
        }
        if (player != null)
        {
            float healthAmount = player.GetHealthPct();
            
            SetHealthAmount(healthAmount);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (weaponManager != null) {
            SetAmmoAmount(weaponManager.currentMagazineSize);
        }

    }
    public void TogglePauseMenu()
    {
        // Inverse l'activation du menu
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;

        if (PauseMenu.isOn)
        {
            // On met le jeu en pause
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // On reprend le jeu
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale=new Vector3(1f,_amount,1f);
    }
    void SetHealthAmount(float _amount)
    {
        healthFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetAmmoAmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }

}
