using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
	[SerializeField] private MagicianInfo[] mags;

	[SerializeField] private int contentCount;

	[SerializeField] private CopyTransformPos[] contentPoints;
	[SerializeField] private CopyTransformPos pointRef;

	[SerializeField] private Image[] contentImages;
	[SerializeField] private Image imagePef;

	[SerializeField] private Transform trContentPoints, trContentImage;

	[SerializeField] private TextMeshProUGUI nameMag, discriptionMag;

	public int curentSelect;

	private void Awake()
	{
		contentPoints = new CopyTransformPos[contentCount];
		contentImages = new Image[contentCount];

		for (int i = 0; i < contentCount; i++)
		{
			contentImages[i] = Instantiate(imagePef, trContentImage);
			contentImages[i].sprite = mags[(i+1)  % mags.Length].image;

			contentPoints[i] = Instantiate(pointRef, trContentPoints);
			contentPoints[i].Obj = contentImages[i].transform;
			contentPoints[i].Target = contentPoints[i].transform;
		}

		Destroy(pointRef.gameObject);
		Destroy(imagePef.gameObject);

		foreach (MagicianInfo mag in mags)
		{
			mag.IsSelect = false;
		}

		UpdateText();
	}

	public void NewCarecter()
	{
		curentSelect = (curentSelect + 1) % mags.Length;
		trContentPoints.GetChild(trContentPoints.childCount - 1).SetAsFirstSibling();

		trContentPoints.GetChild(trContentPoints.childCount - 1).GetComponent<CopyTransformPos>().Obj.position += new Vector3(1000, 1000, 0);
		trContentPoints.GetChild(0).GetComponent<CopyTransformPos>().Obj.position -= new Vector3(1000, 1000, 0);

		UpdateText();
	}

	public void OldCarecter()
	{
		curentSelect = (curentSelect - 1) % mags.Length;
		trContentPoints.GetChild(0).SetSiblingIndex(trContentPoints.childCount - 1);

		trContentPoints.GetChild(trContentPoints.childCount - 1).GetComponent<CopyTransformPos>().Obj.position -= new Vector3(-1000, 1000, 0);
		trContentPoints.GetChild(0).GetComponent<CopyTransformPos>().Obj.position += new Vector3(-1000, 1000, 0);

		UpdateText();
	}

	private void UpdateText()
	{
		int i = -curentSelect;

		if (i < 0)
			i += mags.Length;

		nameMag.text = mags[i].Name;
		discriptionMag.text = mags[i].Discription;
	}

	public void SaveSelectMag()
	{
		int i = -curentSelect;

		if (i < 0)
			i += mags.Length;

		mags[i].IsSelect = true;
	}
}


[Flags]
public enum TypeLauer : int
{
	sewerage = 0,
	lava = 1,
	jail = 0 << 1,
	forest = 1 << 2,
}

[Flags]
public enum UsePatern : int
{
	PerlinNoise = 0,
	VoronoiDiagrams = 1,
	PerlinsWorms = 0 << 1,
}

[Flags]
public enum UseMobe : int
{
	skeleton = 0,
	slug = 1,
	orc = 0 << 1,
	golem = 1 << 2,
}

[Flags]
public enum UseTraps: int
{
	pits = 0,
	spikes = 1,
	trap = 0 << 1,
	axeFromWall = 1 << 2,
}

public enum Density
{
	VeryLow,
	Low,
	Normal,
	High,
	VetyHigh
}

public class Layer
{
	public TypeLauer TypeLauer;
	public Vector2 Size;

	public UsePatern Patern;
	public Density[] PaternDensity;

	public UseMobe Mobs;
	public Density[] MobsDensity;

	public UseMobe UseTraps;
	public Density[] UseTrapsDensity;
}