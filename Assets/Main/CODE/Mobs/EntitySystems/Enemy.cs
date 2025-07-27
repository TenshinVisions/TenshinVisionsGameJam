using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EntitySO entitySO;
    List<Vector2> PathToHero = new List<Vector2>();
    EnemyPathFinder pathFinder;
    public bool isMoving;

    public Hero hero;
    public bool isAlive;

    public int currentHealth;
    int damage;
    bool startMoving = false;

    [SerializeField] float distanceToChasing = 4;
    [SerializeField] float distanceToAttack = 1f;
    [SerializeField] public float moveSpeed = 1.4f;
    float nextAttackTime;
    [SerializeField] public float attackRate = 2f;

    PolygonCollider2D polygonCollider2D;

    public event EventHandler OnEnemyAttack;
    public event EventHandler OnEnemyTakeHit;

    private void Start()
    {
        pathFinder = GetComponent<EnemyPathFinder>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        currentHealth = entitySO.entityHealth;
        damage = entitySO.entityDamage;
        if (hero != null)
        {
            PathToHero = pathFinder.GetPath(hero.transform.position);
            isMoving = true;
        }
        isAlive = true;
    }

    private void Update()
    {
        if (hero == null)
        {
            if (FindFirstObjectByType<Hero>() != null)
            {
                hero = FindFirstObjectByType<Hero>();
                pathFinder.Target = hero;
            }
            else
            {
                return;
            }
        }
        if (Vector2.Distance(transform.position, hero.transform.position) <= distanceToAttack)
        {
            isMoving = false;
            if (Time.time > nextAttackTime)
            {
                OnEnemyAttack?.Invoke(this, EventArgs.Empty);
                nextAttackTime = Time.time + attackRate;
            }
            
        }
        else if (Vector2.Distance(transform.position, hero.transform.position) <= distanceToChasing && Vector2.Distance(transform.position, hero.transform.position) > distanceToAttack)
        {
            polygonCollider2D.enabled = false;
            if (!startMoving)
            {
                isMoving = true;
                startMoving = true;
            }
            if (Vector2.Distance(transform.position, hero.transform.position) > distanceToAttack && PathToHero.Count == 0)
            {
                PathToHero = pathFinder.GetPath(hero.transform.position);
                isMoving = true;
            }
            if (PathToHero.Count == 0)
            {
                return;
            }

            if (isMoving)
            {
                if (Vector2.Distance(transform.position, PathToHero[PathToHero.Count - 1]) > 0.3f)
                {
                    isMoving = true;
                    transform.position = Vector2.MoveTowards(transform.position, PathToHero[PathToHero.Count - 1], moveSpeed * Time.deltaTime);
                    ChangeFacingDirection(new Vector2(transform.position.x, transform.position.y), PathToHero[PathToHero.Count - 1]);
                }

                if (Vector2.Distance(transform.position, PathToHero[PathToHero.Count - 1]) <= 0.3f)
                {
                    isMoving = false;
                }

            }
            else
            {
                PathToHero = pathFinder.GetPath(hero.transform.position);
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
        polygonCollider2D.enabled = false;
        OnEnemyTakeHit?.Invoke(this, EventArgs.Empty);
        currentHealth -= amount;
        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        polygonCollider2D.enabled = false;
        
    }

    public void PolygonColliderTurnOn()
    {
        polygonCollider2D.enabled = true;
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Hero")
            return;
        if (collision.transform.TryGetComponent(out Hero targeted_hero))
            {
                Debug.Log("Skele Attack");
                targeted_hero.TakeDamage(transform, damage);
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