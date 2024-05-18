using UnityEngine;
public class PatrolState : SwordEnemyState
{

	public override void Enter()
	{
		base.Enter();
		swordEnemyCtl.agent.destination = swordEnemyCtl.patrolPoints[swordEnemyCtl.m_Index].position;
		swordEnemyCtl.agent.speed = 2.5f;
	}

	public override void Exit()
	{
		base.Exit();
		swordEnemyCtl.agent.destination = swordEnemyCtl.agent.nextPosition;
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(SwordBodyMachine));
	}
}

public class PatrolToIdle_Transition : SwordEnemyTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(PatrolState), typeof(IdleState), 0.2f, Condition);
	}

	private bool Condition()
	{
		if (!swordEnemyCtl.agent.pathPending && swordEnemyCtl.agent.remainingDistance < 0.5f)
			return true;
		else
			return false;
	}
}

public class PatrolToChase_Transition : SwordEnemyTransition
{
	private float chackRadius = 8f;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(PatrolState), typeof(ChaseState), 0.2f, Condition);
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