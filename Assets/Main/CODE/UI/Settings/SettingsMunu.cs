using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public Action Close, Apply;

	[SerializeField] private Slider soundsOverall, soundsMusick, soundsAmbient;

	[SerializeField] private TMP_Dropdown dropdown;

	[SerializeField] private Color defaultColor, expectationColor;
	[SerializeField] private TextMeshProUGUI
		up,
		down,
		left,
		right,
		jump,
		desh,
		squats;

	[SerializeField] private Image[] imagesKeycode;
	[SerializeField] private Button[] buttonsKeycode;

	private void OnEnable()
	{
		Apply += HandlerApplay;
	}

	private void OnDisable()
	{
		Apply -= HandlerApplay;
	}

	private void Awake()
	{
		// sounds
		soundsOverall.value = PlayerConfig.soundsOverall;
		soundsMusick.value = PlayerConfig.soundsMusick;
		soundsAmbient.value = PlayerConfig.soundsAmbient;

		//screen size
		dropdown.value = PlayerConfig.screenSizes + 1;

		InstalTextKeycode();
	}


	private void HandlerApplay()
	{
		// sounds
		PlayerConfig.soundsOverall = soundsOverall.value;
		PlayerConfig.soundsMusick = soundsMusick.value;
		PlayerConfig.soundsAmbient = soundsAmbient.value;

		//screen size
		PlayerConfig.screenSizes = dropdown.value - 1;

		PlayerConfig.UpdateConfig?.Invoke();
	}

	private void HandlerReset()
	{
		// sounds
		PlayerConfig.soundsOverall = PlayerConfig.defaultSoundsOverall;
		PlayerConfig.soundsMusick = PlayerConfig.defaultSoundsMusick;
		PlayerConfig.soundsAmbient = PlayerConfig.defaultSoundsAmbient;

		soundsOverall.value = PlayerConfig.soundsOverall;
		soundsMusick.value = PlayerConfig.soundsMusick;
		soundsAmbient.value = PlayerConfig.soundsAmbient;

		//screen size
		PlayerConfig.screenSizes = PlayerConfig.defaultScreenSizes;
		dropdown.value = PlayerConfig.screenSizes + 1;

		// key code

		PlayerConfig.up = PlayerConfig.defaultUp;
		PlayerConfig.down = PlayerConfig.defaultDown;
		PlayerConfig.left = PlayerConfig.defaultLeft;
		PlayerConfig.right = PlayerConfig.defaultRight;
		PlayerConfig.jump = PlayerConfig.defaultJump;
		PlayerConfig.desh = PlayerConfig.defaultDesh;
		PlayerConfig.squats = PlayerConfig.defaultSquats;

		InstalTextKeycode();
	}

	public void ButtonClose()
	{
		Close?.Invoke();
		StopAllCoroutines();
		gameObject.SetActive(false);
	}

	public void ButtonApply()
	{
		Apply?.Invoke();
	}

	public void ButtonReset()
	{
		HandlerReset();
	}

	public void ButtonSetKeycode(int i)
	{
		/*
		 * 0 - up
		 * 1 - down
		 * 2 - left
		 * 3 - right
		 * 4 - jump
		 * 5 - desh
		 * 6 - squats
		 */
		imagesKeycode[i].color = expectationColor;
		ButonsKeycodeSetInteractable(false);

		StartCoroutine(ListeningInput(i));
	}

	private IEnumerator ListeningInput(int i)
	{
		while (true)
		{
			if (Input.anyKeyDown)
			{
				foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
				{
					if (Input.GetKeyDown(key))
					{
						SetKeycode(i, key);
						yield break;
					}
				}
			}

			yield return null;
		}
	}

	private void SetKeycode(int i, KeyCode key)
	{
		imagesKeycode[i].color = defaultColor;
		ButonsKeycodeSetInteractable(true);

		switch (i)
		{
			case 0:
				PlayerConfig.up = key;
				break;

			case 1:
				PlayerConfig.down = key;
				break;

			case 2:
				PlayerConfig.left = key;
				break;

			case 3:
				PlayerConfig.right = key;
				break;

			case 4:
				PlayerConfig.jump = key;
				break;

			case 5:
				PlayerConfig.desh = key;
				break;

			case 6:
				PlayerConfig.squats = key;
				break;
		}

		InstalTextKeycode();
	}

	private void InstalTextKeycode()
	{
		up.text = PlayerConfig.up.ToString();
		down.text = PlayerConfig.down.ToString();
		left.text = PlayerConfig.left.ToString();
		right.text = PlayerConfig.right.ToString();
		jump.text = PlayerConfig.jump.ToString();
		desh.text = PlayerConfig.desh.ToString();
		squats.text = PlayerConfig.squats.ToString();
	}

	private void ButonsKeycodeSetInteractable(bool state)
	{
		foreach (Button button in buttonsKeycode)
		{
			button.interactable = state;
		}
	}
}
