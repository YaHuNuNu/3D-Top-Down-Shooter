using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager instance { get; private set; }
	public InputCtrls InputCtrls {  get; private set; }
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(instance.gameObject);

		InputCtrls = new InputCtrls();
	}

	private void OnEnable()
	{
		InputCtrls.Enable();
	}

	private void OnDisable()
	{
		InputCtrls.Disable();
	}
}
