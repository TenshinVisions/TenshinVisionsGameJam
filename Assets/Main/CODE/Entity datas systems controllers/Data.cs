using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Data : MonoBehaviour
{
	public HandlerStrategyByPriority HandlerPriority = new();
}