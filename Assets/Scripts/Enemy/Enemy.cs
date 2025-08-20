using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] public float maxHealth;
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField] private float experienceAmount;
    [SerializeField] private GameObject experienceOrbParticleSystem;

    public float currentHealth { get; private set; }
    public bool hasTakenDamage { get; set; }
    private Tuple<int, int> roomID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damageAmount, bool wasCrit)
    {
        hasTakenDamage = true;
        currentHealth -= damageAmount;
        SpawnDamageNumber(damageAmount, wasCrit);

        if (currentHealth <= 0) Die();
    }

    private void SpawnDamageNumber(float damageAmount, bool wasCrit)
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab);
        //damageNumber.transform.position = transform.position;
        damageNumber.transform.position = new Vector3(transform.position.x + Random.Range(0f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z);
        damageNumber.DamageWasCrit(wasCrit);
        damageNumber.SetDamageAmount(damageAmount);
    }

    public void SetRoomID(Tuple<int, int> ID)
    {
        roomID = ID;
    }

    private void Die()
    {
        Destroy(gameObject);
        SpawnExperienceOrbs();
        RoomHandler.Instance.LowerEnemyCount(roomID.Item1, roomID.Item2);
    }

    private void SpawnExperienceOrbs()
    {
        if (experienceOrbParticleSystem != null && PlayerController.Instance != null)
        {
            GameObject p = Instantiate(experienceOrbParticleSystem);
            p.transform.position = this.transform.position;
            p.GetComponent<ExperienceOrbParticleCollision>().experienceAmount = experienceAmount;
            p.GetComponent<ParticleSystem>().trigger.AddCollider(PlayerController.Instance.gameObject.GetComponent<BoxCollider2D>());
            p.gameObject.SetActive(true);
        }
    }
}
