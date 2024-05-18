using Framework;
using FSM;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour, IController, EntityController
{
	#region field
	[Header("Controller")]
	[SerializeField]
	private Animator m_Animator;
	public RigBuilder rigBuilder;
	public CharacterController controller;
	public AnimationClipData[] m_clipDatas;
	public ReplaceAnimationClipData[] m_replaceClipDatass;
	private IMachineConstructor m_machineConstructor;
	public CustomPlayableAnimator m_customAnimator;
	[Header("BeDamaged")]
	public SkinnedMeshRenderer bodyRenderer;
	public SkinnedMeshRenderer headRenderer;
	public Material redMat;
	private Material bodyMat;
	private Material headMat;

	[Header("PlayerStateData")]
	public float gravity = -9.8f;
	public float walkSpeed = 4.5f;
	public float runSpeed = 8f;
	public float jumpSpeed = 4.5f;

	[HideInInspector]
	public PlayerModel playerModel { get; private set; }
	[HideInInspector]
	public Vector3 upSpeed = Vector3.down;
	[HideInInspector]
	public Vector3 moveSpeed = Vector3.zero;
	public bool isGround { get; private set; } = false;
	#endregion

	private void Awake()
	{
		playerModel = this.GetOrAddModel<PlayerModel>();
		m_machineConstructor = new MachineConstructor<PlayerState, PlayerMachine, PlayerTransition>();
		m_customAnimator = new CustomPlayableAnimator(this, m_Animator, m_clipDatas, m_replaceClipDatass, m_machineConstructor);

		bodyMat = bodyRenderer.material;
		headMat = headRenderer.material;

		playerModel.playerPos.RegisterAction(SetPos);
	}

	private void SetPos(float[] pos)
	{
		Vector3 vec;
		vec.x = pos[0];
		vec.y = pos[1];
		vec.z = pos[2];
		transform.position = vec;
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

	private void Start()
	{

	}

	private void Update()
	{
		GenerateMove();
	}

	public void SetMoveSpeed(Vector2 speed)
	{
		moveSpeed.x = speed.x;
		moveSpeed.z = speed.y;
		//moveSpeed = transform.TransformDirection(moveSpeed);
	}
	public void SetUpSpeed(float speed)
	{
		upSpeed.y = speed;
		//保证isGround的准确
		GenerateMove();
	}
	private void GenerateMove()
	{
		controller.Move(moveSpeed * Time.deltaTime);
		upSpeed.y += gravity * Time.deltaTime;
		controller.Move(upSpeed * Time.deltaTime);
		if (controller.isGrounded)
		{
			upSpeed.y = -1;
			isGround = true;
		}
		else
			isGround = false;
	}

	public void BeDamaged(float damageValue)
	{
		float hp = playerModel.health.Value;
		hp -= damageValue / 100f;
		playerModel.health.Value = Mathf.Clamp(hp, 0, 1);
		StartCoroutine(__Timer(0.1f));
	}

	private IEnumerator __Timer(float t)
	{
		bodyRenderer.material = redMat;
		headRenderer.material = redMat;
		yield return new WaitForSeconds (t);
		bodyRenderer.material = bodyMat;
		headRenderer.material = headMat;
	}
}

