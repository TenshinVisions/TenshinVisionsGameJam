using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidSquat : MonoBehaviour, ISystem
{
	[SerializeField] private Transform trCollider;
	[SerializeField] private float height;
	[SerializeField] private Vector3 compressProportion;

	private StrategyByPriority priority;
	private Squat squat;
	private Surface surface;

	private Vector3 startScale, startLocalPos;
	private SquatState cashstate;

	void ISystem.Instal(Entity entity)
	{
		entity.Datas.TryGetComponent(out squat);
		entity.Datas.TryGetComponent(out surface);

		priority = new StrategyByPriority(Priority.Normal, true);
		squat.HandlerPriority.Add(priority);

		startScale = trCollider.localScale;
		startLocalPos = trCollider.localPosition;
	}

	private void FixedUpdate()
	{
		if (!priority.IsUse)
			return;

		if(squat.Input && squat.State == SquatState.None)
		{
			if (surface.IsContact)
				squat.State = SquatState.Down;
			else
				squat.State = SquatState.Up;
		}
		else if (!squat.Input && squat.State != SquatState.None)
		{
			squat.State = SquatState.None;
		}

		if (squat.State == cashstate)
			return;

		cashstate = squat.State;

		if (cashstate == SquatState.Up)
		{
			Compression();
			trCollider.localPosition = startLocalPos + trCollider.up * (height * 0.5f - squat.Compression * 0.5f);
			return;
		}

		if (cashstate == SquatState.Down)
		{
			Compression();
			trCollider.localPosition = startLocalPos + -trCollider.up * (height * 0.5f - squat.Compression * 0.5f);
			return;
		}

		trCollider.localPosition = startLocalPos;
		trCollider.localScale = startScale;
	}

	private void Compression()
	{
		Vector3 newScale = new Vector3
			(
				(startScale.x * squat.Compression * compressProportion.x) + startScale.x * (1 - compressProportion.x),
				(startScale.y * squat.Compression * compressProportion.y) + startScale.y * (1 - compressProportion.y),
				1
			);

		trCollider.localScale = newScale;
	}
}
