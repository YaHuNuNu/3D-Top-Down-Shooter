using FSM;

public class PlayerState : State
{
	protected PlayerController playerCtl;
	public override void Enter()
	{
		
	}

	public override void UpdateState()
	{
		
	}

	public override void Exit()
	{
		
	}

	public override void InitState()
	{
		playerCtl = EntityManager.Instance.playerCtl;
	}

	public override void SetPlayable()
	{
		
	}
}