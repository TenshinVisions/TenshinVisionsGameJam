using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public static class RotationAlongAxis
{

	public static void RotateOnTarget(Transform tr, Vector3 directionTarget, float rotSpeed)
	{
		Vector3 currentUp = tr.up;

		// Вычисляем угол между текущим up и целевым up
		float angle = Vector3.Angle(currentUp, directionTarget);

		if (angle > 0.5f)
		{
			rotSpeed *= angle / 180f;
			// Ось вращения — это ось, перпендикулярная обоим векторам
			Vector3 axis = Vector3.Cross(currentUp, directionTarget).normalized;
			if (axis.sqrMagnitude > 0.5f)
			{
				float step = rotSpeed;
				float t = Mathf.Min(1f, step / angle); // интерполяция по углу

				Quaternion rot = Quaternion.AngleAxis(step, axis);
				tr.rotation = rot * tr.rotation;
			}
			else
			{
				// Векторы почти совпадают или противоположны
				if (Vector3.Dot(currentUp, directionTarget) < 0.5f)
				{
					// Векторы противоположны — повернуть на 180° вокруг любой перпендикулярной оси
					Vector3 perpAxis = Vector3.Cross(currentUp, Vector3.right);

					if (perpAxis.sqrMagnitude < 0.5f)
						perpAxis = Vector3.Cross(currentUp, Vector3.forward);

					Quaternion rot = Quaternion.AngleAxis(180f * rotSpeed / angle, perpAxis.normalized);
					tr.rotation = rot * tr.rotation;
				}
			}
		}
	}
}

	//private void FixedUpdate()
	//{
	//	#region спасибо GPT

	//	Vector3 gravityDir =  -gravity.Direction.normalized;
	//	// Текущий up объекта
	//	Vector3 currentUp = entity.up;

	//	// Вычисляем угол между текущим up и целевым up
	//	float angle = Vector3.Angle(currentUp, upNorm.position);
	//	if (angle > 0.01f)
	//	{
	//		// Ось вращения — это ось, перпендикулярная обоим векторам
	//		Vector3 axis = Vector3.Cross(currentUp, gravityDir).normalized;
	//		if (axis.sqrMagnitude > 0.001f)
	//		{
	//			// Плавное вращение на небольшой угол за кадр
	//			float step = rotationSpeed /* Time.deltaTime*/;
	//			float t = Mathf.Min(1f, step / angle); // интерполяция по углу
	//			Quaternion rot = Quaternion.AngleAxis(step, axis);
	//			entity.rotation = rot * entity.rotation;
	//		}
	//		else
	//		{
	//			// Векторы почти совпадают или противоположны
	//			if (Vector3.Dot(currentUp, upNorm.position) < 0)
	//			{
	//				// Векторы противоположны — повернуть на 180° вокруг любой перпендикулярной оси
	//				Vector3 perpAxis = Vector3.Cross(currentUp, Vector3.right);
	//				if (perpAxis.sqrMagnitude < 0.001f)
	//					perpAxis = Vector3.Cross(currentUp, Vector3.forward);
	//				Quaternion rot = Quaternion.AngleAxis(180f * /*Time.deltaTime */ rotationSpeed / angle, perpAxis.normalized);
	//				entity.rotation = rot * entity.rotation;
	//			}
	//		}
	//	}

	//	#endregion
	//}