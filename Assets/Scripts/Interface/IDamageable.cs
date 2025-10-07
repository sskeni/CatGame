
using UnityEngine;

public interface IDamageable
{
    public bool canTakeDamage { get; set; }
    public void Damage(float damageAmount, bool wasCrit);
}
