using System.Collections;
using UnityEngine;

public class CopyTransformPos : MonoBehaviour
{
	public Transform Target, Obj;

	public float Interpolate;

	public UpdateMode ModeUpdate;

	private void Update()
	{
		if ((ModeUpdate & UpdateMode.Update) != UpdateMode.None) InterpolateMoveTransform(true);
	}

	private void LateUpdate()
	{
		if ((ModeUpdate & UpdateMode.LateUpdate) != UpdateMode.None) InterpolateMoveTransform(true);
	}

	private void FixedUpdate()
	{
		if ((ModeUpdate & UpdateMode.FixedUpdate) != UpdateMode.None) InterpolateMoveTransform();
	}

	public void InterpolateMoveTransform(bool useDeltaTime = false)
	{
		Vector3 offset = Target.position - Obj.position;

		if(useDeltaTime)
			offset *= Time.deltaTime;

		Obj.position += offset * Interpolate;
	}

}
