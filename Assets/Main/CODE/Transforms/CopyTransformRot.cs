using System.Collections.Generic;
using UnityEngine;

public class CopyTransformRot : MonoBehaviour
{
	public Transform Target, Obj;

	public float Interpolate;

	public UpdateMode ModeUpdate;

	public Vector3 Offset;


	private void Update()
	{
		if ((ModeUpdate & UpdateMode.Update) != UpdateMode.None) InterpolateRotateTransform();
	}

	private void LateUpdate()
	{
		if ((ModeUpdate & UpdateMode.LateUpdate) != UpdateMode.None) InterpolateRotateTransform();
	}

	private void FixedUpdate()
	{
		if ((ModeUpdate & UpdateMode.FixedUpdate) != UpdateMode.None) InterpolateRotateTransform();
	}

	public void InterpolateRotateTransform()
	{
		Obj.rotation = Quaternion.Lerp(Obj.rotation, Target.rotation, Interpolate);
		Obj.Rotate(Offset);
	}
}
