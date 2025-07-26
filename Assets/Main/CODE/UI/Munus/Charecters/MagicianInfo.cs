using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magician Info", menuName = "Magician Info", order = 51)]
public class MagicianInfo : ScriptableObject
{
	public bool IsSelect = false;

	public Sprite image;

	public string Name, Discription;
}
