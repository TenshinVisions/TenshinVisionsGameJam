using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingLauer : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown typeDoss;

	public RoomSetings Room0, Room1, Room2, Room3;

	private void Start()
	{
		typeDoss.options.Clear();

		string[] buffer = Enum.GetNames(typeof(RoomType));

		foreach (var nameTypeRoom in buffer)
		{
			typeDoss.options.Add(new TMP_Dropdown.OptionData(nameTypeRoom));
		}
	}
}

public enum BossType
{

}
