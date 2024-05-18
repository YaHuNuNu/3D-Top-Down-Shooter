using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneInit : MonoBehaviour
{
	private UpdateAfterChangeSence UACS;
	private void Awake()
	{
		UACS = GameObject.FindAnyObjectByType<UpdateAfterChangeSence>();
		UACS.SetHideObjectActive(false);
	}
}
