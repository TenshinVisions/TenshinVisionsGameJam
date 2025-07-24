using System;

[Flags]
public enum UpdateMode
{
	None			= 0,
	Awake			= 1 << 0,
	Start			= 1 << 1,
	Update			= 1 << 2,
	LateUpdate		= 1 << 3,
	FixedUpdate		= 1 << 4,
}