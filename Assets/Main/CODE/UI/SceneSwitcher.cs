using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	public void SwitchByName(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void SwitchByID(int id)
	{
		SceneManager.LoadScene(id);
	}
}
