using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DungeonOmMap : MonoBehaviour
{
	public bool IsFree =true;

	private Vector3 savePosOnMap;
	private Transform saveParentHere, SeveHero;

	[SerializeField] private Transform dungeonObj, buttonsObj;
	[SerializeField] private GameObject HiroInDungeon;

	public static Action OpenDangeHandler;

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
		dungeonObj.gameObject.SetActive(true);
		dungeonObj.SetParent(null);
		dungeonObj.localScale = Vector3.one;

		OpenDangeHandler?.Invoke();
	}

	public void LiaveDungeonHire()
	{
		IsFree = true;
		SeveHero.SetParent(saveParentHere);
		SeveHero.transform.position = savePosOnMap;
		saveParentHere.gameObject.SetActive(true);
		dungeonObj.SetParent(transform);
	}
}
