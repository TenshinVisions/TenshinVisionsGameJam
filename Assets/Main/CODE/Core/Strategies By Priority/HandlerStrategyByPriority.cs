using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandlerStrategyByPriority
{
	private List<StrategyByPriority> strategies = new();
	private StrategyByPriority curentStrategy;

	public void RerollUseStrategy()
	{
		curentStrategy.IsUse = false;
		curentStrategy = null;

		Priority buffer = Priority.Null;

		for (int i = 0; i < strategies.Count; i++)
		{
			if (strategies[i].IsActive &&
				strategies[i].Priority > buffer)
			{
				curentStrategy = strategies[i];
				buffer = strategies[i].Priority;
			}
		}

		if(curentStrategy != null)
			curentStrategy.IsUse = true;
	}

	public void TryUse(StrategyByPriority strategy)
	{
		if ((curentStrategy == null || curentStrategy.Priority < strategy.Priority) && strategy.IsActive)
		{
			if(curentStrategy != null)
				curentStrategy.IsUse = false;

			curentStrategy = strategy;
			curentStrategy.IsUse = true;
		}
	}

	public void Add(StrategyByPriority strategy)
	{
		strategy.Init(this);
		strategies.Add(strategy);
		TryUse(strategy);
	}

	public void Remove(StrategyByPriority strategy)
	{
		strategies.Remove(strategy);
		strategy.IsUse = false;

		if (curentStrategy == strategy)
			RerollUseStrategy();
	}
}
