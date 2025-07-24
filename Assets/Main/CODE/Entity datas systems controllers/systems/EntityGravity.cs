using UnityEngine;

public class EntityGravity : MonoBehaviour, ISystem
{
	private Rigidbody2D rb;
	private Gravity gravity;
	private StrategyByPriority priority;

	void ISystem.Instal(Entity entity)
	{
		entity.Datas.TryGetComponent(out gravity);
		entity.TryGetComponent(out rb);
		priority = new StrategyByPriority(Priority.Normal, true);
		gravity.HandlerPriority.Add(priority);
	}

	private void FixedUpdate()
	{
		if (!priority.IsUse || !gravity.IsUse)
			return;

		rb.AddForce(gravity.Direction * gravity.Acceleration, ForceMode2D.Impulse);
	}
}
