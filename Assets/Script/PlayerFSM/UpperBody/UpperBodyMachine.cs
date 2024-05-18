public class UpperBodyMachine : PlayerMachine
{
	protected override void InitMachine()
	{
		base.InitMachine();
		SetMachineInitValue(true, typeof(AimIdleState), 1);
	}
}