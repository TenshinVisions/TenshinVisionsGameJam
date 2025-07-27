using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DungeonOmMap : MonoBehaviour
{
	public bool IsFree =true, Instal;

	private Vector3 savePosOnMap;
	private Transform saveParentHere, SeveHero;

	[SerializeField] private Transform buttonsObj;
	[SerializeField] private GameObject HiroInDungeon;

	public static Action OpenDangeHandler;


	// Setting 

	public bool IsUse = false;

	public List<SettingLauer> Setting = null;

	public static DungeonOmMap CurentOppen;

	public void EnterTheHero(GameObject hero)
	{
		saveParentHere = hero.transform.parent;
		savePosOnMap = hero.transform.localPosition;
		SeveHero = hero.transform;
		IsFree = false;
		hero.SetActive(false);
		buttonsObj.gameObject.SetActive(true);
	}

	public void OpenDange()
	{
		buttonsObj.gameObject.SetActive(false);
		CurentOppen = this;
		OpenDangeHandler?.Invoke();
	}

	public void LiaveDungeonHire()
	{
		IsFree = true;
		SeveHero.SetParent(saveParentHere);
		SeveHero.transform.position = savePosOnMap;
		saveParentHere.gameObject.SetActive(true);
	}
}
