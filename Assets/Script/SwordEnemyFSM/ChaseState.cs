using UnityEngine;

public class ChaseState : SwordEnemyState
{
	public override void Enter()
	{
		base.Enter();
		swordEnemyCtl.agent.speed = 6.5f;
	}

	public override void Exit()
	{
		base.Exit();
		swordEnemyCtl.agent.destination = swordEnemyCtl.agent.nextPosition;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		swordEnemyCtl.chaseTime -= Time.deltaTime;
		swordEnemyCtl.agent.destination = playerCtl.transform.position;
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(SwordBodyMachine));
	}
}

public class ChaseToRelax_Transition : SwordEnemyTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(ChaseState), typeof(RelaxState), 0.2f, () => swordEnemyCtl.chaseTime < 0);
	}
}

public class ChaseToPatrol_Transition : SwordEnemyTransition
{
	private float chackRadius = 14;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(ChaseState), typeof(PatrolState), 0.3f, Condition);
	}

	private bool Condition()
	{
		base.InitTransition();
		if (swordEnemyCtl.distanceWithPlayer > chackRadius)
			return true;
		else
			return false;
	}
}

public class ChaseToAttack_Transition : SwordEnemyTransition
{
	private float chackRadius = 1.9f;
	private int lastAttackType = 0;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(ChaseState), typeof(AttackState), 0.2f, Condition);
	}

	private bool Condition()
	{
		base.InitTransition();
		if (swordEnemyCtl.distanceWithPlayer < chackRadius)
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