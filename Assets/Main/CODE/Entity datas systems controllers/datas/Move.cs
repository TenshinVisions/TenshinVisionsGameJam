using UnityEngine;

public class Move : Data
{
	public float Speed, Acceleration, MaxAngle, Stop;
	[Range(0f,1f)] 
	public float StopMulty;
	public Vector2 Input;
}
