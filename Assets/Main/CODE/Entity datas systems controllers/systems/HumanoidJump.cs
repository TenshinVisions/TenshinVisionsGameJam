using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidJump : MonoBehaviour, ISystem
{
	[SerializeField, Range(0,1)] private float proportionNormalInImpulse;

	private StrategyByPriority priority;
	private CoyoteTime coyoteTime;
	private Surface surface;
	private Jump jump;
	private Squat squat;
	private Rigidbody2D rb;
	private Transform tr;

	private bool isJumping = false;

	void ISystem.Instal(Entity entity)
	{
		entity.Datas.TryGetComponent(out coyoteTime);
		entity.Datas.TryGetComponent(out surface);
		entity.Datas.TryGetComponent(out jump);
		entity.Datas.TryGetComponent(out squat);
		entity.TryGetComponent(out rb);
		entity.TryGetComponent(out tr);


		priority = new StrategyByPriority(Priority.Normal, false);
		jump.HandlerPriority.Add(priority);
	}

	private void FixedUpdate()
	{
		if (!priority.IsUse && !priority.IsActive && (coyoteTime.IsUse || surface.IsContact))
		{
			priority.TurnOn();
		}
		else if (priority.IsUse && !coyoteTime.IsUse && !surface.IsContact)
		{
			priority.TurnOff();
		}

		if (!priority.IsUse)
			return;

		if (jump.InputJump && !isJumping)
		{
			isJumping = true;

			Vector2 inpulse;

			if (!surface.IsContact)
				inpulse = tr.up;
			else
				inpulse = (surface.Normal * proportionNormalInImpulse) + ((Vector2)tr.up * (1f-proportionNormalInImpulse));

			inpulse *= jump.Inpulse;
			StartCoroutine(Jumping(inpulse));
		}
	}

	private IEnumerator Jumping(Vector2 inpulse)
	{
		for (int i = 0; i < jump.UseFrame; i++)
		{
			rb.AddForce(inpulse, ForceMode2D.Impulse);
			inpulse *= jump.Slowdown;
			coyoteTime.IsActive = false;

			squat.State = SquatState.Up;
			squat.Input = true;

			if (jump.InputJump)
				yield return new WaitForFixedUpdate();
			else
				break;
		}
		squat.Input = false;
		isJumping = false;
	}
}
