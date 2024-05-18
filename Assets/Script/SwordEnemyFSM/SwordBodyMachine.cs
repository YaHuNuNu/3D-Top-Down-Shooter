public class SwordBodyMachine : SwordEnemyMachine
{
	protected override void InitMachine()
	{
		base.InitMachine();
		SetMachineInitValue(true, typeof(SwordEnemyStartState), 0);
	}
}

public class SwordEnemyStartState : SwordEnemyState { }

public class SwordEnemyStart_Transition : SwordEnemyTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(SwordEnemyStartState), typeof(PatrolState), 0, () => true);
	}
}
