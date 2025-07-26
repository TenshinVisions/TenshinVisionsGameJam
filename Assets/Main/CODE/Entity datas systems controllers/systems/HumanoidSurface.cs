using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidSurface : MonoBehaviour, ISystem
{
	[SerializeField] private Vector3 pointCast;
	[SerializeField] private float radiusCast, distanceCast;
	[SerializeField] private LayerMask layerCast;

	private Transform tr;
	private Rigidbody2D rb;

	private Surface surface;
	private StrategyByPriority priority;
	private RaycastHit2D hit;

	void ISystem.Instal(Entity entity)
	{
		entity.Datas.TryGetComponent(out surface);
		entity.TryGetComponent(out tr);
		entity.TryGetComponent(out rb);

		priority = new StrategyByPriority(Priority.Normal, true);
		surface.HandlerPriority.Add(priority);
	}

	private void FixedUpdate()
	{
		if (!priority.IsUse || (rb.velocity.sqrMagnitude <= 0.001f && surface.IsContact))
			return;

		Vector2 direction = -tr.up;

		hit = Physics2D.CircleCast(tr.position + pointCast, radiusCast, direction, distanceCast, layerCast);

		if (hit.collider != null)
		{
			surface.IsContact = true;
			surface.Normal = hit.normal;
			surface.Point = hit.point;
			surface.Angle = Vector3.Angle(-direction, hit.normal);
			surface.ObjectContact = hit.collider.gameObject;
		}
		else
		{
			surface.IsContact = false;
		}
	}

	private void OnDrawGizmos()
	{
		if (surface == null)
			return;

		if (!surface.IsContact)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(tr.position + pointCast, radiusCast);
			Gizmos.DrawSphere(tr.position + pointCast + (-tr.up * distanceCast), radiusCast);
		}
		else
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(tr.position + pointCast, radiusCast);
			Gizmos.DrawSphere(hit.point + (hit.normal * radiusCast), radiusCast);
			Gizmos.color = Color.gray;
			Gizmos.DrawLine(hit.point, hit.point + hit.normal);
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(hit.point, tr.position);
		}
	}
}