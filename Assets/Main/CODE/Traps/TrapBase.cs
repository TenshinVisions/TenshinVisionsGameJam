using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    [Header("Base Trap Stats")]
    public float damage;

    public virtual void Initialize(float damage)
    {
        this.damage = damage;
    }

    // Абстрактные методы для поведения ловушки
    public abstract void ActivateTrap();
    public abstract void DeactivateTrap();
}
