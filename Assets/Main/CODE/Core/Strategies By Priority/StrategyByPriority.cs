public class StrategyByPriority
{
	public bool IsActive = false, IsUse= false;
	public Priority Priority = Priority.Null;

	private HandlerStrategyByPriority handler;

	public void Init(HandlerStrategyByPriority handler)
	{
		this.handler = handler;
	}

	public StrategyByPriority(Priority priority, bool isActive = true)
	{
		Priority = priority;
		IsActive = isActive;
	}

	public void TryUse()
	{
		handler.TryUse(this);
	}

	public void RerollUseStrategy()
	{
		handler.RerollUseStrategy();
	}

	public void SetPriority(Priority priority)
	{
		if (Priority < priority && IsActive && !IsUse)
		{
			Priority = priority;
			handler.TryUse(this);
			return;
		}

		if (Priority > priority && (!IsActive || IsUse))
		{
			Priority = priority;
			handler.RerollUseStrategy();
			return;
		}

		Priority = priority;
	}

	public void TurnOn()
	{
		IsActive = true;
		handler.TryUse(this);
	}

	public void TurnOff()
	{
		IsActive = false;
		if (IsUse)
			handler.RerollUseStrategy();
	}
}
