using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraToTexture : MonoBehaviour
{
	public Camera sourceCamera; 
	public RawImage targetRawImage;

	public int desiredWidth = 512;
	public int desiredHeight = 512;

	private RenderTexture renderTexture;

	private void Awake()
	{
		renderTexture = new RenderTexture(desiredWidth, desiredHeight, 24);
		renderTexture.Create();
		sourceCamera.targetTexture = renderTexture;
		targetRawImage.texture = renderTexture;
		RectTransform rt = targetRawImage.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(desiredWidth, desiredHeight);
	}
}
