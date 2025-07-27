using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] EntitySO entitySO;
    List<Vector2> PathToEnemy = new List<Vector2>();
    PathFinder pathFinder;
    public bool isMoving;

    public int currentHealth;
    int damage;

    public Enemy enemy;
    public float moveSpeed = 1.4f;
    float nextAttackTime;
    [SerializeField] float distanceToAttack = 1.2f;
    [SerializeField] public float attackRate = 1.5f;

    PolygonCollider2D polygonCollider2D;

    public event EventHandler OnHeroAttack;
    public event EventHandler OnHeroTakeHit;

    private void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        currentHealth = entitySO.entityHealth;
        damage = entitySO.entityDamage;
        if (enemy != null)
        {
            pathFinder = GetComponent<PathFinder>();
            PathToEnemy = pathFinder.GetPath(enemy.transform.position);
            isMoving = true;
        }
    }

    private void Update()
    {
        if (enemy == null)
        {
            if (FindFirstObjectByType<Enemy>() != null)
            {
                enemy = FindFirstObjectByType<Enemy>();
                pathFinder.Target = enemy;
            }
            else
            {
                return;
            }
        }

        if (Vector2.Distance(transform.position, enemy.transform.position) <= distanceToAttack)
        {
            isMoving = false;
            if (Time.time > nextAttackTime)
            {
                OnHeroAttack?.Invoke(this, EventArgs.Empty);
                nextAttackTime = Time.time + attackRate;
            }

        }
        else if (Vector2.Distance(transform.position, enemy.transform.position) > distanceToAttack)
        {
            polygonCollider2D.enabled = false;
            if (Vector2.Distance(transform.position, enemy.transform.position) > distanceToAttack && PathToEnemy.Count == 0)
            {
                PathToEnemy = pathFinder.GetPath(enemy.transform.position);
                isMoving = true;
            }
            if (PathToEnemy.Count == 0)
            {
                return;
            }

            if (isMoving)
            {
                if (Vector2.Distance(transform.position, PathToEnemy[PathToEnemy.Count - 1]) > 0.3f)
                {
                    isMoving = true;
                    transform.position = Vector2.MoveTowards(transform.position, PathToEnemy[PathToEnemy.Count - 1], moveSpeed * Time.deltaTime);
                    ChangeFacingDirection(new Vector2(transform.position.x, transform.position.y), PathToEnemy[PathToEnemy.Count - 1]);
                }

                if (Vector2.Distance(transform.position, PathToEnemy[PathToEnemy.Count - 1]) <= 0.3f)
                {
                    isMoving = false;
                }

            }
            else
            {
                PathToEnemy = pathFinder.GetPath(enemy.transform.position);
                isMoving = true;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    public void TakeDamage(Transform source, int amount)
    {
        OnHeroTakeHit?.Invoke(this, EventArgs.Empty);
        currentHealth -= amount;
        DetectDeath();
    }
     public void PolygonColliderTurnOff()
    {
        polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        if (!isMoving)
            polygonCollider2D.enabled = true;
        else
        {
            ChangeFacingDirection(new Vector2(transform.position.x, transform.position.y), PathToEnemy[PathToEnemy.Count - 1]);
        }
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Enemy targeted_enemy))
        {
            targeted_enemy.TakeDamage(transform, damage);
        }
    }

    public void ChangeFacingDirection(Vector2 sourcePosition, Vector2 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }

}