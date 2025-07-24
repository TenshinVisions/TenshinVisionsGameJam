public class Squat : Data
{
	public SquatState State;
	public float Compression;
	public bool Input;
}


public enum SquatState
{
	None,
	Up,
	Down
}