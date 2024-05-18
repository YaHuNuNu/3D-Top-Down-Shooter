using Framework;
using FSM;
using UnityEngine;
using UnityEngine.AI;

public class SwordEnemyController : MonoBehaviour, IController, EntityController
{
	public int ID;
	[HideInInspector]
	public SwordEnemyModel model;

	[SerializeField]
	private Animator m_Animator;
	public AnimationClipData[] m_clipDatas;
	public ReplaceAnimationClipData[] m_replaceClipDatas;
	private IMachineConstructor m_machineConstructor;
	public CustomPlayableAnimator m_customAnimator;

	public NavMeshAgent agent;
	public Transform[] patrolPoints;
	[HideInInspector]
	public int m_Index = 0;

	[HideInInspector]
	public float idleTime = 0;
	[HideInInspector]
	public float chaseDuration = 7f;
	[HideInInspector]
	public float chaseTime = 0f;
	[HideInInspector]
	public float distanceWithPlayer = 0;

	public enum AttackType
	{
		Slash_1 = 0,
		Slash_2,
		Kick
	}

	private void Awake()
	{
		model = this.GetOrAddModel<SwordEnemyModelBase>().GetByID(ID);

		m_machineConstructor = new MachineConstructor<SwordEnemyState, SwordEnemyMachine, SwordEnemyTransition>();
		m_customAnimator = new CustomPlayableAnimator(this, m_Animator, m_clipDatas, m_replaceClipDatas, m_machineConstructor);
		;
	}

	private void OnEnable()
	{
		m_customAnimator.GraphPlay();
	}

	private void OnDisable()
	{
		m_customAnimator.GraphStop();
	}

	private void OnDestroy()
	{
		m_customAnimator.GraphDestroy();
	}
	private void Update()
	{
		distanceWithPlayer = Vector3.Distance(transform.position, EntityManager.Instance.playerCtl.transform.position);
	}

	public void BeDamaged(float damageValue)
	{
		float hp = model.health.Value;
		hp -= damageValue / 100f;
		model.health.Value = Mathf.Clamp(hp, 0, 1);
	}
}
