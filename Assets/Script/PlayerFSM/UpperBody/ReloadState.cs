using Framework;

public class ReloadState : PlayerState
{
	private PlayerModel playerModel;
	private Inventory inventory;
	public override void Enter()
	{
		base.Enter();
		playerCtl.rigBuilder.enabled = false;
		int currentWeapon = (int)playerModel.weaponType.Value;
		int BulletNum_Delta = playerModel.defaultBulletNum[currentWeapon] -  playerModel.currentBulletNum[currentWeapon].Value;

		if(BulletNum_Delta > inventory.bulletSum[currentWeapon].Value)
		{
			BulletNum_Delta = inventory.bulletSum[currentWeapon].Value;
		}

		inventory.bulletSum[currentWeapon].Value -= BulletNum_Delta;
		playerModel.currentBulletNum[currentWeapon].Value += BulletNum_Delta;
	}

	public override void Exit()
	{
		playerCtl.rigBuilder.enabled = true;
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
		playerModel = CustomModelSystem.Instance.GetOrAddModel<PlayerModel>();
		inventory = CustomModelSystem.Instance.GetOrAddModel<Inventory>();
	}
}

public class ReloadToAimIdle_Transition : PlayerTransition
{
	private bool animEnd = false;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(ReloadState), typeof(AimIdleState), 0.2f, Condition);
		CustomEventSystem.Instance.GetEvent<AnimationEvent.ReloadAnimEvent>().Register(() => animEnd = true);
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
