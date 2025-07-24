using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	[SerializeField] private GameObject[] munus;
	[SerializeField] private GameObject curentMenu;

	public void OpenMenu (int i)
	{
		curentMenu.SetActive(false);
		curentMenu = munus[i];
		curentMenu.SetActive(true);
	}
}