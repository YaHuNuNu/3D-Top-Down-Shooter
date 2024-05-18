using FSM;

public class PlayerTransition : Transition
{
	protected PlayerController playerCtl;
	protected InputCtrls inputCtrls;
	public override void InitTransition()
	{
		playerCtl = EntityManager.Instance.playerCtl;
		inputCtrls = InputManager.instance.InputCtrls;
	}
}