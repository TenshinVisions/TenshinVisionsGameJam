using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiroOnMap : MonoBehaviour
{
	[SerializeField] private CharecterState State;
	[SerializeField] private Vector2 mapSize;
	[SerializeField] private float speed;

	[SerializeField] private Transform[] Canceles;

	[SerializeField] private DungeonOmMap curentDange;

	[SerializeField] private float maxSleepTime;

	private Coroutine coroutineMove;

	[SerializeField] private float MaxDistanceOnDange;

	private void Awake()
	{
		State = CharecterState.InWay;
	}

	private void OnEnable()
	{
		Stop();
	}

	private void Update()
	{
		if (State == CharecterState.InWay && coroutineMove == null)
		{
			Vector3 newRandomPoint  =new Vector3
				(
				Random.Range(-mapSize.x, mapSize.x),
				Random.Range(-mapSize.y, mapSize.y),
				0
				);

			coroutineMove = StartCoroutine(Movement(newRandomPoint));
		}
	}

	private IEnumerator Movement(Vector3 target)
	{
		yield return new WaitForSeconds(Random.Range(0, maxSleepTime));

		Vector3 direction = (target - transform.position).normalized;

		float distance;

		do
		{
			ChackDunge();

			if (State != CharecterState.InWay)
			{
				yield break;
			}

			transform.position += direction * speed * Time.deltaTime;
			yield return new WaitForEndOfFrame();

			distance = (transform.position - target).magnitude;
		}
		while (distance > speed * Time.deltaTime || distance > 0.5f);

		transform.position = target;

		Stop();
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

	private void Stop()
	{
		if (coroutineMove == null)
			return;

		StopAllCoroutines();
		coroutineMove = null;
	}
}



public enum CharecterState
{
	InCastle,
	InWay,
	InDungeon,
}