using Framework;
using System;

public class AimIdleState : PlayerState
{
	public override void Enter()
	{
		base.Enter();
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
	}
}

public class AimIdleToFiring_Transition : PlayerTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(AimIdleState), typeof(FiringState), 0, Condition);
	}
	private bool Condition() => inputCtrls.Base.Fire.IsPressed();
}

public class Oneself_Transition : PlayerTransition
{
	private int weaponTypeLength = 0;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(AimIdleState), typeof(AimIdleState), 0.2f, Condition);
		weaponTypeLength = Enum.GetNames(typeof(WeaponType)).Length;
	}

	private bool Condition()
	{
		if (inputCtrls.Base.Toggle.triggered)
		{
			playerCtl.playerModel.weaponType.Value = (WeaponType)((int)(playerCtl.playerModel.weaponType.Value + 1) % weaponTypeLength);
			//ÇÐ»»¶¯×÷
			playerCtl.m_customAnimator.ChangeOverrideAniamtion(playerCtl.playerModel.weaponType.Value.ToString());
			return true;
		}
		else
			return false;
	}
}

public class AimIdleToReload_Transition : PlayerTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(AimIdleState), typeof(ReloadState), 0.2f, Condition);
	}
	private bool Condition() => inputCtrls.Base.Reload.triggered;
}

public class AimIdleToThrow_Transition : PlayerTransition
{
	private Inventory inventory;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(AimIdleState), typeof(ThrowState), 0.15f, Condition);
		inventory = CustomModelSystem.Instance.GetOrAddModel<Inventory>();
	}
	private bool Condition()
	{
		return inputCtrls.Base.Throw.triggered && inventory.grenadeSum.Value > 0;
	}
	}