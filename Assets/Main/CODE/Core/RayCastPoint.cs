using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastPoint : MonoBehaviour
{
	public Transform StartPoint, DirectionPoint, AimPoint, DefaundPosPoint;

	public UpdateMode ModeUpdate;

	public float MaxDistance = 0;

	public RaycastHit2D CurentAim, OldAim;

	public LayerMask LayerMask;

	public bool IsUseTargetDistance, NoUseMaxDistance;


	private void Update()
	{
		if ((ModeUpdate & UpdateMode.Update) != UpdateMode.None) AimPositionUpdate();
	}

	private void LateUpdate()
	{
		if ((ModeUpdate & UpdateMode.LateUpdate) != UpdateMode.None) AimPositionUpdate();
	}

	private void FixedUpdate()
	{
		if ((ModeUpdate & UpdateMode.FixedUpdate) != UpdateMode.None) AimPositionUpdate();
	}

	public void AimPositionUpdate()
	{
		Vector2 startPos = StartPoint.position;
		Vector2 direction = (DirectionPoint.position - StartPoint.position).normalized;

		float maxDistance, curentDistance = (DirectionPoint.position - StartPoint.position).magnitude;
		
		if (IsUseTargetDistance)
		{
			maxDistance = (DirectionPoint.position - StartPoint.position).magnitude;

			if(NoUseMaxDistance == false && maxDistance > MaxDistance)
			{
				maxDistance = MaxDistance;
			}
		}
		else maxDistance = MaxDistance;

		RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDistance, LayerMask);


		if (hit.collider != null)
		{
			CurentAim = hit;
			AimPoint.position = hit.point;
		}
		else
		{
			if (DefaundPosPoint != null && curentDistance > MaxDistance && NoUseMaxDistance == false)
			{
				AimPoint.position = StartPoint.position + (DirectionPoint.position - StartPoint.position) * (MaxDistance / curentDistance);
			}
			else if (DefaundPosPoint != null)
			{
				AimPoint.position = DirectionPoint.position;
			}

			OldAim = CurentAim;
			CurentAim = default;
		}
	}
}
