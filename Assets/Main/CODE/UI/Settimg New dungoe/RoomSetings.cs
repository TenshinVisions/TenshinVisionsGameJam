using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class RoomSetings : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown types;
	[SerializeField] private Slider sliderCount;
	[SerializeField] private TMP_InputField input;

	private int max = 8, curent = 0;

	public int MobCount => curent;

	public RoomType RoomType => (RoomType)types.value;


	private void Start()
	{
		types.options.Clear();

		string[] buffer = Enum.GetNames(typeof(RoomType));

		foreach (var nameTypeRoom in buffer)
		{
			types.options.Add(new TMP_Dropdown.OptionData(nameTypeRoom));
		}
	}

	private void Update()
	{
		if((int)(max * sliderCount.value) != curent)
		{
			curent = (int)(max * sliderCount.value);
			input.text = curent.ToString();
		}
	}

	public void NewValue()
	{
		if(int.TryParse(input.text, out int curent))
		{
			sliderCount.value = curent / (float)max;
		}
	}
}


public enum RoomType
{

}