using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField]
    private WeaponData theWeapon;
    private GameObject pickUpGraphics;
   
    void Start()
    {
        ResetWeapon();
    }
    void ResetWeapon()
    {
        pickUpGraphics = Instantiate(theWeapon.graphics, transform.position, transform.rotation);

        pickUpGraphics.transform.SetParent(transform);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            EquipNewWeapon(weaponManager);
        }
    }

    void EquipNewWeapon(WeaponManager weaponManager)
    {
        Destroy(weaponManager.GetCurrentGraphics().gameObject);
        weaponManager.EquipWeapon(theWeapon);


    }

}
