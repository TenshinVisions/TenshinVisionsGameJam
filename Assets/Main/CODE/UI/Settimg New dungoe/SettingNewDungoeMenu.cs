using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingNewDungoeMenu : MonoBehaviour
{
	[SerializeField] private SettingLauer prefab;
	[SerializeField] private List<SettingLauer> layerSetting = new();
	[SerializeField] private Transform content;
	[SerializeField] private GameObject hiros;

	// settings
	public List<SettingLauer> LayersSetting => layerSetting;


	private void OnEnable()
	{
		hiros.SetActive(false);

		for (int i = 0; i < layerSetting.Count; i++)
		{
			Destroy(layerSetting[i].gameObject);
		}

		layerSetting.Clear();
	}

	private void OnDisable()
	{
		hiros.SetActive(true);

		DungeonOmMap buffer = null;

		for(int i = 0; i< MapWithDungeons.Dangeons.Count; i++)
		{
			if (!MapWithDungeons.Dangeons[i].Instal)
			{
				buffer = MapWithDungeons.Dangeons[i];
				MapWithDungeons.Dangeons[i].Instal = true;
				break;
			}
		}

		buffer.Setting = new();

		foreach (SettingLauer layerSetting in LayersSetting)
		{
			buffer.Setting.Add(layerSetting);
		}
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
