using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharecterOnMapMenu : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private CharecterState State;
	[SerializeField] private Vector2 mapSize;
	[SerializeField] private float speed;

	[SerializeField] private Transform[] Canceles;

	[SerializeField] private DangeonOmMap curentDange;

	[SerializeField] private float maxSleepTime;

	private void Start()
	{
		NewRandomAction();
	}

	private IEnumerator Movement(Vector3 target)
	{
		yield return new WaitForSeconds(Random.Range(0, maxSleepTime));

		Vector3 
			direction = (transform.position - target).normalized,
			step = direction * speed;
		do
		{
			transform.position += step;
			yield return new WaitForFixedUpdate();
		} 
		while ((transform.position - target).magnitude > speed);

		transform.position = target;
		NewRandomAction();
	}

	private void NewRandomAction()
	{
		int i = Random.Range(0, 2);

		if(i <= 1)
		{

		}
		else
		{

		}
	}
}

public enum CharecterState
{
	InCastle,
	InWay,
	InDungeon,
}