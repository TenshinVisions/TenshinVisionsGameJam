using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapWithDungeons : MonoBehaviour
{
	public static bool PlaceIsFree = false, CusorOnMap = true;
	public static List<DangeonOmMap> Dangeons = new();

	[SerializeField] private DangeonOmMap dangeRef;
	[SerializeField] private Transform content;

	[Space]
	[SerializeField] private Image cursorImage;
	[SerializeField] private Transform cursorTr;
	[SerializeField] private Color canPut, noPut;

	[Space]
	[SerializeField] private GameObject buttonsAssert;

	public static StateDungeonSettings State = StateDungeonSettings.TryOn;

	private Vector3 saveInstalPos;

	public static MapWithDungeons Init;

	private void Awake()
	{
		Init = this;
	}

	private void Update()
	{
		if (!CusorOnMap || State != StateDungeonSettings.TryOn)
		{
			cursorTr.gameObject.SetActive(false);
			return;
		}

		cursorTr.gameObject.SetActive(true);

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		cursorTr.transform.position = mousePos;
		saveInstalPos = cursorTr.transform.position;

		if (PlaceIsFree)
			cursorImage.color = canPut;
		else cursorImage.color = noPut;
	}

	public void TryAssert()
	{
		if (!CusorOnMap || !PlaceIsFree)
			return;

		State = StateDungeonSettings.Assert;
		buttonsAssert.SetActive(true);
		buttonsAssert.transform.position = saveInstalPos;
	}

	public void AssertConfirm(bool state)
	{
		buttonsAssert.SetActive(false);

		if (state)
			State = StateDungeonSettings.Config;
		else
			State = StateDungeonSettings.TryOn;
	}

	public void CloseAddDangeonMenu(bool addDange)
	{
		State = StateDungeonSettings.TryOn;

		if (addDange)
			Instantiate(dangeRef, saveInstalPos, Quaternion.identity, content);
	}

	public enum StateDungeonSettings
	{
		TryOn,
		Assert,
		Config,
		AddDangeon
	}
}
