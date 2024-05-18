using Framework;

public class ShootEvent : CustomEvent<WeaponType> { }
public class FiringState : PlayerState
{
	private ShootEvent shootEvent;
	private PlayerModel playerModel;
	public override void Enter()
	{
		base.Enter();
		int num = playerModel.currentBulletNum[(int)playerModel.weaponType.Value].Value;
		num = --num >= 0 ? num : 0;
		playerModel.currentBulletNum[(int)playerModel.weaponType.Value].Value = num;
		if (num == 0)
			return;
		shootEvent.Trigger(playerModel.weaponType.Value);
	}

	public override void Exit()
	{
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
		shootEvent = CustomEventSystem.Instance.RegisterEvent<ShootEvent>();
		playerModel = CustomModelSystem.Instance.GetOrAddModel<PlayerModel>();
	}
}

public class FiringToAimIdle_Transition : PlayerTransition
{
	public bool animEnd = false;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(FiringState), typeof(AimIdleState), 0, Condition);
		CustomEventSystem.Instance.GetEvent<AnimationEvent.FireAnimEvent>().Register(() => animEnd = true);
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
