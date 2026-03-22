using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Ennemies : MonoBehaviour
{
    [Header("Déplacement - References")]
    // J'ai renommé cette variable "playerTransform" car "Player" avec une majuscule 
    // entrait en conflit avec le nom de votre script Player.
    [SerializeField] private Transform playerTransform;
    [SerializeField] private NavMeshAgent navMeshAgent;

    [Header("Déplacement - Stats")]
    [SerializeField] private float detectionRadius = 20f;
    private bool hasDestination;

    [Header("Déplacement - Wandering")]
    [SerializeField] private float wanderingWaitTimeMin = 1f;
    [SerializeField] private float wanderingWaitTimeMax = 5f;
    [SerializeField] private float wanderingDistanceMin = 5f;
    [SerializeField] private float wanderingDistanceMax = 10f;

    [Header("Tir - Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 50f;
    [SerializeField] private float fireRate = 1f; // Un tir par seconde
    private float nextTimeToFire = 0f;

    [Header("Tir - References")]
    [SerializeField] private Transform shootPoint; // L'endroit d'où part la balle
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private AudioClip shootSound;

    void Start()
    {
        // Initialisation si besoin
    }

    void Update()
    {
        // On calcule la distance entre l'ennemi et le joueur
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        // 1. DÉTECTION ET CHASSE
        if (distanceToPlayer < detectionRadius)
        {
            // L'ennemi voit le joueur : il le poursuit
            navMeshAgent.SetDestination(playerTransform.position);

            if (distanceToPlayer <= navMeshAgent.stoppingDistance)
            {
                // On force l'ennemi à pivoter pour toujours vous viser !
                Vector3 directionToFace = (playerTransform.position - transform.position).normalized;
                directionToFace.y = 0; // Pour qu'il reste droit
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToFace), Time.deltaTime * 5f);
            }

            // On vérifie si on peut lui tirer dessus
            CheckAndShoot();
        }
        // 2. ERRANCE (Wandering)
        else
        {
            // L'ennemi est loin : il se promène au hasard
            if (navMeshAgent.remainingDistance < 0.75f && !hasDestination)
            {
                StartCoroutine(GetNewDestination());
            }
        }
    }

    // --- FONCTIONS DE DÉPLACEMENT ---

    IEnumerator GetNewDestination()
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));

        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDistanceMin, wanderingDistanceMax) * new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
        hasDestination = false;
    }

    // --- FONCTIONS DE TIR ---

    void CheckAndShoot()
    {
        RaycastHit hit;
        // On tire un rayon invisible vers l'avant depuis le canon
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, range))
        {
            // Si le rayon touche le joueur
            if (hit.collider.CompareTag("Player"))
            {
                // Si c'est le moment de tirer (cadence)
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                }
            }
        }
    }

    void Shoot()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        // Effet visuel du tir
        if (muzzleFlash != null) muzzleFlash.Play();


        // On relance un rayon pour appliquer les dégâts
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, range))
        {
            // On récupère le script Player sur l'objet touché
            Player playerComponent = hit.collider.GetComponent<Player>();

            if (playerComponent != null)
            {
                // On applique les dégâts
                playerComponent.TakeDamage(damage);
                Debug.Log("L'ennemi a touché le joueur ! Dégâts : " + damage);
            }
        }
    }

    // --- DEBUG ---

    private void OnDrawGizmos()
    {
        // Dessine la zone de détection en jaune
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Dessine la portée de tir en rouge si le shootPoint est assigné
        if (shootPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(shootPoint.position, shootPoint.forward * range);
        }
    }
}