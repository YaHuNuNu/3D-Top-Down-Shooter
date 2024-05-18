using Framework;
using UnityEngine;

public class RelaxState : SwordEnemyState
{
	public override void Exit()
	{
		base.Exit();
		swordEnemyCtl.chaseTime = swordEnemyCtl.chaseDuration;
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(SwordBodyMachine));
	}
}

public class RelaxToChase_Transition : SwordEnemyTransition
{
	private bool animEnd = false;
	private float chackRadius = 1.6f;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(RelaxState), typeof(ChaseState), 0.2f, Condition);
		CustomEventSystem.Instance.GetEvent<SwordEnemyAnimEvent.RelaxAnimEvent>().Register(AnimEvent);
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
		if (animEnd && swordEnemyCtl.distanceWithPlayer > chackRadius)
		{
			animEnd = false;
			return true;
		}
		else
			return false;
	}
}

public class RelaxToAttack_Transition : SwordEnemyTransition
{
	private bool animEnd = false;
	private float chackRadius = 1.9f;
	private int lastAttackType = 0;

	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(RelaxState), typeof(AttackState), 0.2f, Condition);
		CustomEventSystem.Instance.GetEvent<SwordEnemyAnimEvent.RelaxAnimEvent>().Register(AnimEvent);
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
		base.InitTransition();
		if (animEnd && swordEnemyCtl.distanceWithPlayer < chackRadius)
		{
			int i = Random.Range(0, System.Enum.GetNames(typeof(SwordEnemyController.AttackType)).Length);
			SwordEnemyController.AttackType attackType;
			if (i == lastAttackType)
			{
				i = (i + 1) % System.Enum.GetNames(typeof(SwordEnemyController.AttackType)).Length;
			}
			lastAttackType = i;
			attackType = (SwordEnemyController.AttackType)i;
			swordEnemyCtl.m_customAnimator.ChangeOverrideAniamtion(attackType.ToString());
			return true;
		}
		else
			return false;
	}
}