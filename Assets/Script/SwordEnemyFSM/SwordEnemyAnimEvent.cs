using Framework;
using UnityEngine;

public class SwordEnemyAnimEvent : MonoBehaviour, IController
{
	public LayerMask playerLayer;
	public Transform centerPos;
	public float radius;
	private SwordEnemyController swordEnemyCtl;
	public class RelaxAnimEvent : CustomEvent<SwordEnemyController> { }
	private RelaxAnimEvent relaxAnimEvent;
	public class AttackAnimEvent:CustomEvent<SwordEnemyController> { }
	private AttackAnimEvent attackAnimEvent;

	private void Start()
	{
		relaxAnimEvent = this.RegisterEvent<RelaxAnimEvent>();
		attackAnimEvent = this.RegisterEvent<AttackAnimEvent>();
		swordEnemyCtl = GetComponent<SwordEnemyController>();
	}

	public void RelaxEvent() => relaxAnimEvent.Trigger(swordEnemyCtl);
	public void AttackEvent() => attackAnimEvent.Trigger(swordEnemyCtl);

	public void Attack()
	{
		Collider[] collider = Physics.OverlapSphere(centerPos.position, radius, playerLayer);
		if(collider != null && collider.Length > 0)
		{
			collider[0].GetComponent<PlayerController>()?.BeDamaged(swordEnemyCtl.model.ATK.Value);
		}
	}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.DrawWireSphere(centerPos.position, radius);
	//}
}