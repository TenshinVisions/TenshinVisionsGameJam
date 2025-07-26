using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapWithDungeons : MonoBehaviour
{
	public static bool PlaceIsFree = false, CusorOnMap = true;
	public static List<DangeonOmMap> Dangeons = new();

	[SerializeField] private DangeonOmMap dangeRef;
	[SerializeField] private Transform content;

	[SerializeField] private Image Cursor;
	[SerializeField] private Color canPut, noPut;

	public static DangeonOmMap DangeRef;
	public static Vector3 CursorPos;
	public static Transform Content;

	private void Awake()
	{
		DangeRef = dangeRef;
		Content = content;
	}

	private void Update()
	{
		if (!CusorOnMap)
		{
			Cursor.gameObject.SetActive(false);
			return;
		}

		Cursor.gameObject.SetActive(true);

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;

		Cursor.transform.position = mousePos;
		CursorPos = mousePos;

		if (PlaceIsFree)
			Cursor.color = canPut;
		else Cursor.color = noPut;
	}

	private enum StateDungeonSettings
	{
		TryOn,
		Assert,
		Config,
		AddDangeon
	}
}
