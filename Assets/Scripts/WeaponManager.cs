using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponData primaryWeapon;
    private WeaponData currentWeapon;
    private WeaponGraphics currentGraphics; 
    [SerializeField]
    private Transform weaponHolder;
    [SerializeField]
    private string weaponLayerName = "Weapon";
    public int currentMagazineSize;
    public bool isReloading=false; 

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    public void EquipWeapon(WeaponData _weapon)
    {
        currentWeapon = _weapon;
        currentMagazineSize = _weapon.magazineSize;

        GameObject weaponIns = Instantiate(_weapon.graphics,weaponHolder.position,weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);
        currentGraphics= weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.LogError("Pas de script WeaponGraphics sur l'arme:" + weaponIns.name);
        } 
        SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public IEnumerator Reload()
    {
        if (isReloading)
        {
            yield break;
        }
        Debug.Log("Reloading ...");
        isReloading=true;
        Animator animator = currentGraphics.GetComponent<Animator>();
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(currentWeapon.reloadSound);
        if (animator != null)
        {
            animator.SetTrigger("Reload");
        }
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentMagazineSize=currentWeapon.magazineSize;


        isReloading=false;
        Debug.Log("Reloading finished");

    }

}
