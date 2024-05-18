using Framework;
using UnityEngine;
using UnityEngine.UI;

public class Status_UI_Controller : MonoBehaviour, IController
{
	private SwordEnemyController swordEnemyCtl;
	private Slider slider;
	private SwordEnemyModel model;

	private void Awake()
	{
		swordEnemyCtl = GetComponentInParent<SwordEnemyController>();
		slider = GetComponentInChildren<Slider>();
	}
	void Start()
	{
		model = this.GetOrAddModel<SwordEnemyModelBase>().GetByID(swordEnemyCtl.ID);
		model.health.RegisterActionWithExcute((float hp) => slider.value = hp);
		model.health.RegisterActionWithExcute(DeathAction);
	}

	private void DeathAction(float hp)
	{
		if (hp == 0)
			this.gameObject.SetActive(false);
	}
}
