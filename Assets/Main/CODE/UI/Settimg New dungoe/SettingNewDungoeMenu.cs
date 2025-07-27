using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingNewDungoeMenu : MonoBehaviour
{
	[SerializeField] private GameObject prefab;
	[SerializeField] private List<GameObject> layerSetting;
	[SerializeField] private Transform content;
	[SerializeField] private GameObject hiros;

	private void OnEnable()
	{
		hiros.SetActive(false);
	}

	private void OnDisable()
	{
		hiros.SetActive(true);
	}

	public void Add()
	{
		layerSetting.Add(Instantiate(prefab, content));
	}

	public void Remove()
	{
		Destroy(layerSetting[layerSetting.Count - 1]);
		layerSetting.RemoveAt(layerSetting.Count - 1);
	}
}
