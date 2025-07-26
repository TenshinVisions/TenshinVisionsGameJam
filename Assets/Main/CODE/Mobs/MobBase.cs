using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MobBase : MonoBehaviour
{
    [Header("Base Mob Stats")]
    public float damage;
    public float speed;
    public float health;

    public virtual void Initialize(float damage, float speed, float health)
    {
        this.damage = damage;
        this.speed = speed;
        this.health = health;
    }

    public abstract void Move();
    public abstract void Attack();
}