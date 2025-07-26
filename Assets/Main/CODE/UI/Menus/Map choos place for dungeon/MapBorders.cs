
using UnityEngine;
using UnityEngine.EventSystems;

public class MapBorders : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		MapWithDungeons.CusorOnMap = false;
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		MapWithDungeons.CusorOnMap = true;
	}
}

