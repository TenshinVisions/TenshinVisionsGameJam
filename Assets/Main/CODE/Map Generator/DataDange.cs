using UnityEngine;
using System;

public class DataDange
{
	[Flags]
	public enum TypeLauer : int
	{
		sewerage = 0,
		lava = 1,
		jail = 0 << 1,
		forest = 1 << 2,
	}

	[Flags]
	public enum UsePatern : int
	{
		PerlinNoise = 0,
		VoronoiDiagrams = 1,
		PerlinsWorms = 0 << 1,
	}

	[Flags]
	public enum UseMobe : int
	{
		skeleton = 0,
		slug = 1,
		orc = 0 << 1,
		golem = 1 << 2,
	}

	[Flags]
	public enum UseTraps : int
	{
		pits = 0,
		spikes = 1,
		trap = 0 << 1,
		axeFromWall = 1 << 2,
	}

	public enum Density
	{
		VeryLow,
		Low,
		Normal,
		High,
		VetyHigh
	}

	public class Layer
	{
		public TypeLauer TypeLauer;
		public Vector2 Size;

		public UsePatern Patern;
		public Density[] PaternDensity;

		public UseMobe Mobs;
		public Density[] MobsDensity;

		public UseTraps UseTraps;
		public Density[] UseTrapsDensity;
	}
}

