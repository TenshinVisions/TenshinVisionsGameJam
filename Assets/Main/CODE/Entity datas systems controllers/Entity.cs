using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	public GameObject Datas, Systems;

	private void Awake()
	{
		ISystem[] systems = Systems.GetComponents<ISystem>();


		foreach (var system in systems)
		{
			Debug.Log($"{name}: system: {system}");
			system.Instal(this);
		}
	}
}
