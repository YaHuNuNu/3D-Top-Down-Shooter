using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class RunState : PlayerState
{
	private Playable2DBlend m_blend;
	private InputCtrls m_inputCtrls;
	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void SetPlayable()
	{
		base.SetPlayable();
		m_blend = ((ScriptPlayable<Playable2DBlend>)playable).GetBehaviour();
		playable.SetSpeed(1.5f);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		Vector2 input_Move = m_inputCtrls.Base.Move.ReadValue<Vector2>();
		Vector3 localInput = playerCtl.transform.InverseTransformDirection(new Vector3(input_Move.x, 0, input_Move.y));
		m_blend.SetParameterGrad(localInput.x, localInput.z, 0.06f);
		playerCtl.SetMoveSpeed(input_Move * playerCtl.runSpeed);
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(LowerBodyMachine));
		m_inputCtrls = InputManager.instance.InputCtrls;
	}
}

public class RunToWalk_Transition : PlayerTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(RunState), typeof(WalkState), 0.15f, Condition);
	}
	private bool Condition() => !inputCtrls.Base.Dash.IsPressed();
}

public class RunToJump_Transition : PlayerTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(RunState), typeof(JumpState), 0.15f, Condition);
	}
	private bool Condition() => inputCtrls.Base.Jump.triggered && playerCtl.isGround;
}

public class RunToRoll_Transition : PlayerTransition
{
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(RunState), typeof(RollState), 0.15f, Condition);
	}
	private bool Condition() => inputCtrls.Base.Roll.triggered;
}