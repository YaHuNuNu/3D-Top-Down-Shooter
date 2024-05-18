using Framework;
using UnityEngine;

public class AttackState : SwordEnemyState
{
	public override void UpdateState()
	{
		base.UpdateState();
		swordEnemyCtl.chaseTime -= Time.deltaTime / 2.0f;
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(SwordBodyMachine));
	}
}

public class AttackToChase_Transition : SwordEnemyTransition
{
	private bool animEnd = false;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(AttackState), typeof(ChaseState), 0.2f, Condition);
		CustomEventSystem.Instance.GetEvent<SwordEnemyAnimEvent.AttackAnimEvent>().Register(AnimEvent);
	}

	private void AnimEvent(SwordEnemyController controller)
	{
		if (controller == swordEnemyCtl)
		{
			animEnd = true;
		}
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