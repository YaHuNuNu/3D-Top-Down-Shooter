using UnityEngine;

public class AimController : MonoBehaviour
{
	private Vector2 cursorPos;
	private InputCtrls playerCtrls;
	private Transform playerTransform;
	[SerializeField]
	private LayerMask layerMask;
	public float maxDistanceDelta;

	private void Start()
	{
		playerCtrls = InputManager.instance.InputCtrls;
		playerTransform = EntityManager.Instance.playerCtl.gameObject.transform;
	}

	private void OnEnable()
	{
		transform.position = new Vector3(1.5f, 0, 1.5f);
	}

	private void Update()
	{
		cursorPos = playerCtrls.Base.CursorPos.ReadValue<Vector2>();
		Ray ray = Camera.main.ScreenPointToRay(cursorPos);
		//∑¢≥ˆ…‰œﬂ
		if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
		{
			transform.position = Vector3.MoveTowards(transform.position, hitInfo.point, maxDistanceDelta * Time.deltaTime);

			Vector3 playerForward = transform.position - playerTransform.position;
			playerForward.y = 0f;
			playerTransform.forward = playerForward;
		}
	}
}
