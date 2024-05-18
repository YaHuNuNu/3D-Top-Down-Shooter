using FSM;

public class SwordEnemyMachine : Machine
{
	public override float GetRootMachineWeight()
	{
		return 1;
	}

	public override void SetPlayable()
	{

	}

	protected override void InitMachine()
	{

	}

	public override void InitState()
	{

	}
}

public class SwordEnemyState : State
{
	protected SwordEnemyController swordEnemyCtl;
	protected PlayerController playerCtl;
	public override void Enter()
	{
		base.Enter();
	}

	public override void UpdateState()
	{

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void InitState()
	{
		swordEnemyCtl = entity as SwordEnemyController;
		playerCtl = EntityManager.Instance.playerCtl;
	}

	public override void SetPlayable()
	{

	}
}

public class SwordEnemyTransition : Transition
{
	protected SwordEnemyController swordEnemyCtl;
	protected PlayerController playerCtl;
	public override void InitTransition()
	{
		swordEnemyCtl = entity as SwordEnemyController;
		playerCtl = EntityManager.Instance.playerCtl;
	}
}