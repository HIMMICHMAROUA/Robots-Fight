using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    public static AmmoDisplay singleton = null;

    public int currentAmmo;           // balles dans le chargeur courant  
    public int reserveAmmo = 300;     // réserve de munitions hors chargeur  
    public bool hasWeapon = false;    // vrai si arme équipée  
    public TextMeshProUGUI ammoDisplay;

    void Start()
    {
        singleton = this;
        currentAmmo = 0;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (!hasWeapon)
        {
            ammoDisplay.text = "";
            return;
        }
    }

    public void SetCurrentAmmos(int ammoCurrent)
    {
        currentAmmo = ammoCurrent;
        UpdateAmmoUI();
    }

    public void SetReserveAmmos(int ammoReserve)
    {
        reserveAmmo = ammoReserve;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        ammoDisplay.text = currentAmmo.ToString() + " / " + reserveAmmo.ToString();
    }

    public void SetWeaponEquipped(bool equipped)
    {
        hasWeapon = equipped;
        UpdateAmmoUI();
    }
}
