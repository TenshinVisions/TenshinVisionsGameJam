using UnityEngine;

public class HeroVisual : MonoBehaviour
{
    [SerializeField] Hero hero;
    Animator animator;

    const string IsWalking = "IsWalking";
    const string Attack = "Attack";
    const string TakeHit = "TakeHit";
    const string IsDie = "IsDie";

    void Start()
    {
        animator = GetComponent<Animator>();
        hero.OnHeroAttack += hero_OnHeroAttack;
        hero.OnHeroTakeHit += hero_OnHeroTakeHit;
    }

    void Update()
    {
        animator.SetBool(IsWalking, hero.isMoving);
    }

    public void TriggerAttackAnimationTurnOff()
    {
        hero.PolygonColliderTurnOff();
        if (hero != null && hero.enemy != null)
            hero.ChangeFacingDirection(new Vector2(hero.transform.position.x, hero.transform.position.y), new Vector2(hero.enemy.transform.position.x, hero.enemy.transform.position.y));
    }

    public void TriggerAttackAnimationTurnOn()
    {
        hero.PolygonColliderTurnOn();
    }

    void OnDestroy()
    {
        hero.OnHeroAttack -= hero_OnHeroAttack;
        hero.OnHeroTakeHit -= hero_OnHeroTakeHit;
    }

    void hero_OnHeroAttack(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Attack);
    }

    void hero_OnHeroTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TakeHit);
    }
}
