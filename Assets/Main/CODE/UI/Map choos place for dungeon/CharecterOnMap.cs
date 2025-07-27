using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharecterOnMap : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private 
	CharecterState State;
}

public enum CharecterState
{
	InCastle,
	InWay,
	InDungeon,
}