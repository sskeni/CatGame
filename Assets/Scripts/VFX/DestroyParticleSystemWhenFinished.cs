using UnityEngine;

public class DestroyParticleSystemWhenFinished : MonoBehaviour
{
    private ParticleSystem p;

    private void Start()
    {
        p = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!p.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
