using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityController : MonoBehaviour
{
	private Move move;
	private Jump jump;
	private Entity entity;

	private void Awake()
	{
		TryGetComponent(out entity);
		// entity.Datas.TryGetComponent(out move);
		// entity.Datas.TryGetComponent(out jump);
	}

	private void Update()
	{
		Vector2 input = Vector2.zero;

		if (Input.GetKey(PlayerConfig.right)) input.x++;
		if (Input.GetKey(PlayerConfig.left)) input.x--;

		move.Input = input;

		jump.InputJump = Input.GetKey(KeyCode.Space);
	}
}
