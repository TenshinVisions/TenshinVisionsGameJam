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
	}

	public void NewCarecter()
	{
		curentSelect = (curentSelect + 1) % mags.Length;
		trContentPoints.GetChild(trContentPoints.childCount - 1).SetAsFirstSibling();
		trContentPoints.GetChild(trContentPoints.childCount - 1).GetComponent<CopyTransformPos>().Obj.position -= Vector3.up * 9999f;
		UpdateText();
	}

	public void OldCarecter()
	{
		curentSelect = (curentSelect - 1) % mags.Length;
		trContentPoints.GetChild(0).SetSiblingIndex(trContentPoints.childCount - 1);
		trContentPoints.GetChild(0).GetComponent<CopyTransformPos>().Obj.position -= Vector3.up * 9999f;

		UpdateText();
	}

	private void UpdateText()
	{
		int i = Math.Abs(curentSelect);

		nameMag.text = mags[i].Name;
		discriptionMag.text = mags[i].Discription;
	}
}
