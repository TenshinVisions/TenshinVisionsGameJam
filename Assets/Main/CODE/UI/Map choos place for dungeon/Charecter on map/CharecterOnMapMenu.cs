using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CharecterOnMapMenu : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private CharecterState State;
	[SerializeField] private Vector2 mapSize;
	[SerializeField] private float speed;

	[SerializeField] private Transform[] Canceles;

	[SerializeField] private DungeonOmMap curentDange;

	[SerializeField] private float maxSleepTime;

	private Coroutine coroutineMove;

	[SerializeField] private float MaxDistanceOnDange;

	private void OnEnable()
	{
		NewRandomAction();
	}

	private void FixedUpdate()
	{
		if (State == CharecterState.InWay && coroutineMove == null)
		{
			NewRandomAction();
		}
		else if (State != CharecterState.InWay && coroutineMove != null)
		{
			Stop();
		}
	}

	private IEnumerator Movement(Vector3 target)
	{
		yield return new WaitForSeconds(Random.Range(0, maxSleepTime));
		target.z = 0;

		Vector3 direction = (target - transform.position).normalized;

		do
		{
			ChackDunge();

			if (State != CharecterState.InWay)
				yield break;
			
			Debug.Log(direction * speed);

			transform.position += direction * speed;
			yield return new WaitForFixedUpdate();
		} 
		while ((transform.position - target).magnitude > speed);

		transform.position = target;

		Stop();
	}

	private void NewRandomAction()
	{
		Stop();

		int i = Random.Range(0, 2);
			
		if (i <= 1)
		{
			Vector3 newRandomPoint = new Vector3
				(
					Random.Range(-mapSize.x, mapSize.x),
					Random.Range(-mapSize.y, mapSize.y),
					0
				);

			coroutineMove = StartCoroutine(Movement(newRandomPoint));
		}
		else
		{
			coroutineMove = StartCoroutine(Movement(Canceles[Random.Range(0, Canceles.Length -1)].position)); ;
		}
	}

	private void Stop()
	{
		if (coroutineMove == null)
			return;
		StopCoroutine(coroutineMove);
		coroutineMove = null;
	}

	private void ChackDunge()
	{
		if (MapWithDungeons.Dangeons == null)
			return;

		for (int i = 0; i < MapWithDungeons.Dangeons.Count; i++)
		{
			if(MapWithDungeons.Dangeons[i].IsFree &&
				(MapWithDungeons.Dangeons[i].transform.position - transform.position).magnitude <= MaxDistanceOnDange)
			{
				curentDange = MapWithDungeons.Dangeons[i];
				break;
			}
		}

		if (curentDange == null)
			return;

		State = CharecterState.InDungeon;
		curentDange.EnterTheHero(gameObject);
	}
}

public enum CharecterState
{
	InCastle,
	InWay,
	InDungeon,
}