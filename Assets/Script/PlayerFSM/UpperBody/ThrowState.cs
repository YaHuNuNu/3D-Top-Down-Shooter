using Framework;

public class Event_ExitThrowEvent : CustomEvent<bool> { }
public class ThrowState : PlayerState
{
	private Event_ExitThrowEvent enterThrowEvent;
	private Inventory inventory;
	public override void Enter()
	{
		base.Enter();
		playerCtl.rigBuilder.enabled = false;
		enterThrowEvent.Trigger(true);
		inventory.grenadeSum.Value--;
	}

	public override void Exit()
	{
		playerCtl.rigBuilder.enabled = true;
		enterThrowEvent.Trigger(false);
		base.Exit();
	}

	public override void SetPlayable()
	{
		base.SetPlayable();
	}

	public override void UpdateState()
	{
		base.UpdateState();
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(UpperBodyMachine));
		enterThrowEvent = CustomEventSystem.Instance.RegisterEvent<Event_ExitThrowEvent>();
		inventory = CustomModelSystem.Instance.GetOrAddModel<Inventory>();
	}
}

public class ThrowToAimIdle_Transition : PlayerTransition
{
	private bool animEnd = false;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(ThrowState), typeof(AimIdleState), 0.15f, Condition);
		CustomEventSystem.Instance.GetEvent<AnimationEvent.ThrowEndAnimEvent>().Register(() => animEnd = true);
	}
	private bool Condition()
	{
		if (animEnd)
		{
			animEnd = false;
			return true;
		}
		else
			return false;
	}
}