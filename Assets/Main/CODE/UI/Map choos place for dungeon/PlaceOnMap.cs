using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceOnMap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		MapWithDungeons.PlaceIsFree = true;
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		MapWithDungeons.PlaceIsFree = false;
	}
	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		MapWithDungeons.Init.TryAssert();
	}
}
