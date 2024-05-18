using UnityEngine;

public class IdleState : SwordEnemyState
{
	float duration = 2f;
	public override void Enter()
	{
		base.Enter();
		swordEnemyCtl.idleTime = duration;

	}

	public override void UpdateState()
	{
		base.UpdateState();
		swordEnemyCtl.idleTime -= Time.deltaTime;
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(SwordBodyMachine));
	}
}

public class IdleToPatrol_Transition : SwordEnemyTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(IdleState), typeof(PatrolState), 0.2f, Condition);
	}

	private bool Condition()
	{
		
		if(swordEnemyCtl.idleTime < 0)
		{
			swordEnemyCtl.m_Index = (swordEnemyCtl.m_Index + 1) % swordEnemyCtl.patrolPoints.Length;
			return true;
		}
		return false;
	}
}

public class IdleToChase_Transition : SwordEnemyTransition
{
	private float chackRadius = 8f;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(IdleState), typeof(ChaseState), 0.2f, Condition);
	}

	private bool Condition()
	{
		base.InitTransition();
		if (swordEnemyCtl.distanceWithPlayer < chackRadius)
			return true;
		else
			return false;
	}
}
