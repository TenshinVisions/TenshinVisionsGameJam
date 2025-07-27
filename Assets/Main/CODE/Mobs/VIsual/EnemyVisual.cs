using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    Animator animator;

    const string IsWalking = "IsWalking";
    const string Attack = "Attack";
    const string TakeHit = "TakeHit";
    const string IsDie = "IsDie";

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy.OnEnemyAttack += enemy_OnEnemyAttack;
        enemy.OnEnemyTakeHit += enemy_OnEnemyTakeHit;
    }

    void Update()
    {
        animator.SetBool(IsWalking, enemy.isMoving);
    }

    public void TriggerAttackAnimationTurnOff()
    {
        enemy.PolygonColliderTurnOff();
        if (enemy != null && enemy.hero != null)
            enemy.ChangeFacingDirection(new Vector2(enemy.transform.position.x, enemy.transform.position.y), new Vector2(enemy.hero.transform.position.x, enemy.hero.transform.position.y));
    }

    public void TriggerAttackAnimationTurnOn()
    {
        enemy.PolygonColliderTurnOn();
    }

    void OnDestroy()
    {
        enemy.OnEnemyAttack -= enemy_OnEnemyAttack;
        enemy.OnEnemyTakeHit -= enemy_OnEnemyTakeHit;
    }

    void enemy_OnEnemyAttack(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Attack);
    }

    void enemy_OnEnemyTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TakeHit);
    }
}
