using FSM;
using UnityEngine.Playables;

public class JumpState : PlayerState
{
	private Playable1DBlend m_blend;
	private float m_jumpSpeed;
	public override void Enter()
	{
		base.Enter();
		m_jumpSpeed = playerCtl.jumpSpeed;
		playerCtl.SetUpSpeed(m_jumpSpeed);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void SetPlayable()
	{
		base.SetPlayable();
		m_blend = ((ScriptPlayable<Playable1DBlend>)playable).GetBehaviour();
	}

	public override void UpdateState()
	{
		base.UpdateState();
		m_blend.SetParameter(playerCtl.upSpeed.y / m_jumpSpeed);
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(LowerBodyMachine));
	}
}

public class JumpToWalk_Transition : PlayerTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(JumpState), typeof(WalkState), 0.15f, Condition);
	}
	private bool Condition() => playerCtl.isGround;
}