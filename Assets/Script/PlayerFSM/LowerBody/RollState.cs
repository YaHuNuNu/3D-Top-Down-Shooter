using Framework;
using UnityEngine;
using UnityEngine.Playables;

public class RollState : PlayerState
{
	private float speed = 5f;
	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		playerCtl.moveSpeed = Vector3.zero;
		base.Exit();
	}

	public override void SetPlayable()
	{
		base.SetPlayable();
		playable.SetSpeed(2);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		playerCtl.moveSpeed = playerCtl.transform.forward * speed;
	}

	public override void InitState()
	{
		base.InitState();
		SetStateInitValue(typeof(LowerBodyMachine));
	}
}

public class RollToWalk_Transition : PlayerTransition
{
	private bool animEnd = false;
	public override void InitTransition()
	{
		base.InitTransition();
		SetInitValue(typeof(RollState), typeof(WalkState), 0.2f, Condition);
		CustomEventSystem.Instance.GetEvent<AnimationEvent.RollAnimEvent>().Register(() => animEnd = true);
	}
	private bool Condition()
	{
		if(animEnd)
		{
			animEnd = false;
			return true;
		}
		else
			return false;
	}
}