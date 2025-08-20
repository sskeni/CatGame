using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ExperienceOrbParticleCollision : MonoBehaviour
{
    private ParticleSystem ps;
    private List<Particle> particleList = new List<Particle>();
    [HideInInspector] public float experienceAmount = 1f;


    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(DestroyCoroutine()); // Destroy the particle system after for cleanup
    }

    private void OnParticleTrigger()
    {
        // Get list of all particles currently being triggered
        ParticlePhysicsExtensions.GetTriggerParticles(ps, ParticleSystemTriggerEventType.Enter, particleList);
        for (int i = 0; i < particleList.Count; i++)
        {
            // Set particle life to 0 to destory particle
            Particle p = particleList[i];
            p.remainingLifetime = 0f;
            particleList[i] = p;

            // Give player experience
            PlayerController.Instance.playerLevel.GiveExperience(experienceAmount / ps.main.maxParticles);
        }
        // Replace triggered particles in particle system
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
    }

    // Destroys the particle system shorly after all particles lifetimes have expired
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(ps.main.startLifetime.constant + (ps.main.maxParticles * 0.1f)); // 0.1f because thats the double the interval we burst the particles at
        Destroy(this.gameObject);
    }
}
