using UnityEngine;


public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private WeaponManager weaponManager;
    private WeaponData currentWeapon;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Pas de camera sur le player shoot");

        }

        weaponManager = GetComponent<WeaponManager>();
    }
    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.R) && weaponManager.currentMagazineSize<currentWeapon.magazineSize)
        {
            StartCoroutine(weaponManager.Reload());
            return;
            
        }

            if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot",0f,1f / currentWeapon.fireRate);
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }

        }
    }



    void ShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(currentWeapon.shootSound);
    }

    void HitEffect(Vector3 pos,Vector3 normal)
    {
        GameObject hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab,pos,Quaternion.LookRotation(normal));
        Destroy(hitEffect,2f);
    }


    private void Shoot()
    {
        if (weaponManager.isReloading)
        {
            return;
        }
        if (weaponManager.currentMagazineSize <= 0)
        {
            StartCoroutine(weaponManager.Reload());
            return;
        }
        ShootEffect();
        RaycastHit hit;
        weaponManager.currentMagazineSize--;
        if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hit,currentWeapon.range,mask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Ennemi touché : " + hit.collider.name);

                
                EnemyTarget target = hit.collider.GetComponent<EnemyTarget>();

                if (target != null)
                {
                    target.TakeDamage(currentWeapon.damage);
                }
            }
            else
            {
                Debug.Log("Objet touché, mais ce n'est pas un ennemi : " + hit.collider.name);
            }
            HitEffect(hit.point,hit.normal);
        }
        
    }
    
}
