using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ExperienceOrbParticleCollision : MonoBehaviour
{
    // Particle References
    private ParticleSystem ps;
    private List<Particle> particleList = new List<Particle>();
    [HideInInspector] public float experienceAmount = 1f;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!ps.isPlaying) // Destroy the particle system once it is no longer in use
        {
            Destroy(this.gameObject);
        }
    }

    // Gets all the particles that touch the player, give the player experience and destroy them.
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
            PlayerLevel.Instance.GiveExperience(experienceAmount / ps.main.maxParticles);
        }
        // Replace triggered particles in particle system
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
    }
}
