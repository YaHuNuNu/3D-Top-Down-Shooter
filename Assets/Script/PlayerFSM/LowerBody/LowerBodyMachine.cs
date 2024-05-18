public class LowerBodyMachine : PlayerMachine
{
	protected override void InitMachine()
	{
		base.InitMachine();
		SetMachineInitValue(true, typeof(WalkState), 0);
	}
}