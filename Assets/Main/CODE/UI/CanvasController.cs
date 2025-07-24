using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
	private CanvasScaler canvasScaler;
	private int cashConfig = -1;

	private void Awake()
	{
		TryGetComponent(out canvasScaler);
	}

	private void OnEnable()
	{
		PlayerConfig.UpdateConfig += UpdateScreanSize;
	}

	private void OnDisable()
	{
		
	}

	private void UpdateScreanSize()
	{
		if (cashConfig == PlayerConfig.screenSizes)
			return;

		cashConfig = PlayerConfig.screenSizes;

		if (cashConfig == -1)
		{
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
			return;
		}

		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = PlayerConfig.ScreenSizes[cashConfig];
	}
}
