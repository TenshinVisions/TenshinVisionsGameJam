using System.Collections;
using UnityEngine;

public class CopyTransformPos : MonoBehaviour
{
	public Transform Target, Obj;

	public float Interpolate;

	public UpdateMode ModeUpdate;

	private void Update()
	{
		if ((ModeUpdate & UpdateMode.Update) != UpdateMode.None) InterpolateMoveTransform();
	}

	private void LateUpdate()
	{
		if ((ModeUpdate & UpdateMode.LateUpdate) != UpdateMode.None) InterpolateMoveTransform();
	}

	private void FixedUpdate()
	{
		if ((ModeUpdate & UpdateMode.FixedUpdate) != UpdateMode.None) InterpolateMoveTransform();
	}

	public void InterpolateMoveTransform()
	{
		Vector3 offset = Target.position - Obj.position;
		Obj.position += offset * Interpolate;
	}

}
