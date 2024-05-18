using System;

public class DeathState : SwordEnemyState
{
	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(SwordBodyMachine));
	}
}

public class AllToDeath_Transition : SwordEnemyTransition
{
	private bool died = false;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(null, typeof(DeathState), 0.3f, () => died);

		swordEnemyCtl.model.health.RegisterActionWithExcute(Condition);
	}

	private void Condition(float hp)
	{
		if(hp == 0)
			died = true;
	}
}