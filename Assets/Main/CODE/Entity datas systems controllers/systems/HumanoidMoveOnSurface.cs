using System;
using UnityEngine;
using UnityEngine.Windows;

public class HumanoidMoveOnSurface : MonoBehaviour, ISystem
{
	[SerializeField, Range(0,1)] float saveSpeed;

	private Move move;
	private Surface surface;
	private StrategyByPriority priority;
	private Transform tr;
	private Rigidbody2D rb;

	private Vector2
		cashDirect = Vector2.zero,
		cashNormal= Vector2.zero,
		cashInput = Vector2.zero;

	private Quaternion cashRot = Quaternion.identity;

	void ISystem.Instal(Entity entity)
	{
		entity.Datas.TryGetComponent(out move);
		entity.Datas.TryGetComponent(out surface);
		entity.TryGetComponent(out tr);
		entity.TryGetComponent(out rb);

		priority = new StrategyByPriority(Priority.Normal, false);
		move.HandlerPriority.Add(priority);
	}

	private void FixedUpdate()
	{
		if (priority.IsUse && (!surface.IsContact || surface.Angle > move.MaxAngle))
		{
			priority.TurnOff();
		}
		else if (
			!priority.IsUse && !priority.IsActive && surface.IsContact && surface.Angle <= move.MaxAngle)
		{
			priority.TurnOn();
		}

		if (!priority.IsUse || !priority.IsActive)
			return;

		Vector2 normal = surface.Normal;
		Vector2 currentVelocity = rb.velocity;
		Vector2 surfaceVelocity = currentVelocity - Vector3.Dot(currentVelocity, normal) * normal;

		if (move.Input.sqrMagnitude > 0.1f)
		{
			Vector2 input;
			Vector2 direct;

			if (true && 
				(normal - cashNormal).sqrMagnitude <= 0.1f &&
				(move.Input - cashInput).sqrMagnitude <= 0.1f)
			{
				direct = cashDirect;
			}
			else
			{
				input = move.Input;
				direct = (input - Vector2.Dot(input, normal) * normal).normalized;

				cashNormal = normal;
				cashDirect = direct;
				cashInput = move.Input;
			}

			Vector2 targetVelocity = direct * move.Speed;

			if (targetVelocity.sqrMagnitude > 0.1f)
			{
				Vector2 velocityDelta = targetVelocity - surfaceVelocity, force;
				force = velocityDelta * rb.mass;
				force = Vector3.ClampMagnitude(force, move.Acceleration);

				rb.AddForce(
					((surfaceVelocity.magnitude * direct - surfaceVelocity) * saveSpeed) // save curent speed
					+ force, ForceMode2D.Impulse);

				return;
			}
		}
		
		rb.AddForce(Vector2.ClampMagnitude(-surfaceVelocity * move.StopMulty, move.Stop), ForceMode2D.Impulse);
	}

	private bool CompareRot()
	{
		if ((Mathf.Abs(tr.rotation.w - cashRot.w)) > 0.01f ||
			(Mathf.Abs(tr.rotation.x - cashRot.x)) > 0.01f ||
			(Mathf.Abs(tr.rotation.y - cashRot.y)) > 0.01f ||
			(Mathf.Abs(tr.rotation.z - cashRot.z)) > 0.01f)
		{
			cashRot = tr.rotation;
			return false;
		}
		return true;
	}
}
