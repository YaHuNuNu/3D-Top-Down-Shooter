using Framework;
using UnityEngine;

public class AnimationEvent : MonoBehaviour, IController
{
	public class FireAnimEvent : CustomEvent { }
	private FireAnimEvent fireAnimEvent;
	public class ReloadAnimEvent : CustomEvent { }
	private ReloadAnimEvent reloadAnimEvent;
	public class RollAnimEvent : CustomEvent { }
	private RollAnimEvent rollAnimEvent;
	public class ThrowAnimEvent : CustomEvent { }
	public class ThrowEndAnimEvent : CustomEvent { }
	private ThrowAnimEvent throwAnimEvent;
	private ThrowEndAnimEvent throwEndAnimEvent;

	private void Start()
	{
		fireAnimEvent = this.RegisterEvent<FireAnimEvent>();
		reloadAnimEvent = this.RegisterEvent<ReloadAnimEvent>();
		rollAnimEvent = this.RegisterEvent<RollAnimEvent>();
		throwAnimEvent = this.RegisterEvent<ThrowAnimEvent>();
		throwEndAnimEvent = this.RegisterEvent<ThrowEndAnimEvent>();
	}

	public void FireEvent() => fireAnimEvent.Trigger();
	public void ReloadEvent() => reloadAnimEvent.Trigger();
	public void RollEvent() => rollAnimEvent.Trigger();
	public void ThrowEvent() => throwAnimEvent.Trigger();
	public void ThrowEndEvent() => throwEndAnimEvent.Trigger();
}
