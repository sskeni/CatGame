
using UnityEngine;

public interface IDamageable
{
    public bool hasTakenDamage { get; set; }
    public void Damage(float damageAmount, bool wasCrit);
}
