using System;

public class PlayerDeathState : PlayerState
{
	public override void Enter()
	{
		base.Enter();
		playerCtl.rigBuilder.enabled = false;
	}

	public override void Exit()
	{
		playerCtl.rigBuilder.enabled = true;
		base.Exit();
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(LowerBodyMachine));
	}

	public override void UpdateState()
	{
		base.UpdateState();
	}
}

public class AllToPlayerDeath_Transition : PlayerTransition
{
	private bool died = false;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(null, typeof(PlayerDeathState), 0.3f, () => died);
		playerCtl.playerModel.health.RegisterActionWithExcute(Condition);
	}

	private void Condition(float hp)
	{
		if(hp == 0)
			died = true;
	}
}