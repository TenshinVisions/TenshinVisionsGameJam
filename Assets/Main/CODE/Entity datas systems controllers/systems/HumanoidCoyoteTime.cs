using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class HumanoidCoyoteTime : MonoBehaviour, ISystem
{
	private Rigidbody2D rb;
	private Surface surface;
	private Gravity gravity;
	private CoyoteTime coyoteTime;
	private StrategyByPriority priority;

	private bool isFly;

	void ISystem.Instal(Entity entity)
	{
		// entity.Datas.TryGetComponent(out surface);
		// entity.Datas.TryGetComponent(out gravity);
		// entity.Datas.TryGetComponent(out coyoteTime);

		entity.TryGetComponent(out rb);

		priority = new StrategyByPriority(Priority.Normal, true);
		coyoteTime.HandlerPriority.Add(priority);
	}

	private void FixedUpdate()
	{
		if (!priority.IsUse)
			return;

		if (surface.IsContact)
		{
			isFly = false;
			coyoteTime.IsActive = true;
		}

		if(!surface.IsContact && !isFly && coyoteTime.IsActive)
		{
			isFly = true;
			gravity.IsUse = false;
			coyoteTime.IsUse = true;
			StartCoroutine(Flying());
		}
	}

	private IEnumerator Flying()
	{

		for (int i = 0; i < coyoteTime.UseFrame; i++)
		{
			if (!priority.IsUse || !coyoteTime.IsActive)
				break;

			StopFail();
			yield return new WaitForFixedUpdate();
		}

		gravity.IsUse = true;
		coyoteTime.IsUse = false;
	}

	private void StopFail()
	{
		Vector2 newVelosity = rb.velocity - gravity.Direction * Vector2.Dot(rb.velocity, gravity.Direction);
		rb.velocity = newVelosity;
	}
}
