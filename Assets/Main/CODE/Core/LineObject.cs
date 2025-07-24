using UnityEngine;

public class LineObject : MonoBehaviour
{
	public Transform StartPoint, MiddlePoint, EndPoint;

	public Vector3 RotationWorldUp, RotationOffset, MultiScale;

	public UpdateMode ModeUpdatePosition, ModeUpdateRotation, ModeUpdateScale;

	private Vector3 _startScale;
	private float _startDistance;

	private void Awake()
	{
		_startScale = MiddlePoint.localScale;
		_startDistance = (EndPoint.position - StartPoint.position).magnitude;
	}

	private void Update()
	{
		if ((ModeUpdatePosition & UpdateMode.Update) != UpdateMode.None) PositionUpdate();
		if ((ModeUpdateScale & UpdateMode.Update) != UpdateMode.None) ScaleUpdate();
		if ((ModeUpdateRotation & UpdateMode.Update) != UpdateMode.None) RotateUpdate();
	}

	private void LateUpdate()
	{
		if ((ModeUpdatePosition & UpdateMode.LateUpdate) != UpdateMode.None) PositionUpdate();
		if ((ModeUpdateScale & UpdateMode.LateUpdate) != UpdateMode.None) ScaleUpdate();
		if ((ModeUpdateRotation & UpdateMode.LateUpdate) != UpdateMode.None) RotateUpdate();
	}

	private void FixedUpdate()
	{
		if ((ModeUpdatePosition & UpdateMode.FixedUpdate) != UpdateMode.None) PositionUpdate();
		if ((ModeUpdateScale & UpdateMode.FixedUpdate) != UpdateMode.None) ScaleUpdate();
		if ((ModeUpdateRotation & UpdateMode.FixedUpdate) != UpdateMode.None) RotateUpdate();
	}

	public void PositionUpdate()
	{
		MiddlePoint.position = ((EndPoint.position - StartPoint.position) / 2) + StartPoint.position;
	}

	public void ScaleUpdate()
	{
		float multi = (EndPoint.position - StartPoint.position).magnitude / _startDistance - 1;
		Vector3 newScale = _startScale;
		
		newScale.x += _startScale.x * multi * MultiScale.x;
		newScale.y += _startScale.y * multi * MultiScale.y;
		newScale.z += _startScale.z * multi * MultiScale.z;

		MiddlePoint.localScale = newScale;
	}

	public void RotateUpdate()
	{
		MiddlePoint.LookAt(StartPoint, RotationWorldUp);
		MiddlePoint.Rotate(RotationOffset);
	}
}
