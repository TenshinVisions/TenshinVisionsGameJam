using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerConfig
{
	public static Action UpdateConfig;

	public static float 
		defaultSoundsOverall  = 1f,
		defaultSoundsMusick  = 1f,
		defaultSoundsAmbient  = 1f;

	public static float
		soundsOverall  = 1f,
		soundsMusick  = 1f,
		soundsAmbient  = 1f;

	public static Vector2[] ScreenSizes = new Vector2[]
		{
			new Vector2 (1920, 1080),
			new Vector2 (1366, 768),
			new Vector2 (2560, 1440),
			new Vector2 (3840, 2160),
			new Vector2 (1280, 720),
			new Vector2 (960, 540),
			new Vector2 (380, 180),
		};

	public static int screenSizes = -1;
	public static int defaultScreenSizes = -1;

	public static KeyCode
		 defaultUp = KeyCode.W,
		 defaultDown = KeyCode.S,
		 defaultLeft= KeyCode.A,
		 defaultRight= KeyCode.D,
		 defaultJump = KeyCode.Space,
		 defaultDesh = KeyCode.LeftShift,
		 defaultSquats = KeyCode.LeftControl;

	public static KeyCode
		 up = KeyCode.W,
		 down = KeyCode.S,
		 left= KeyCode.A,
		 right= KeyCode.D,
		 jump = KeyCode.Space,
		 desh = KeyCode.LeftShift,
		 squats = KeyCode.LeftControl;

}
