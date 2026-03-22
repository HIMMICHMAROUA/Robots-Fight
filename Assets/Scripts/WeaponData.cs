using UnityEngine;

[CreateAssetMenu(fileName="WeaponData",menuName="My Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    
    public string name;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0f;
    public int magazineSize;
    public float reloadTime=1.5f;

    public GameObject graphics;
    public AudioClip shootSound;
    public AudioClip reloadSound;
}
