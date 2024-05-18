using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace FSM
{
	public struct TransitionInfo
	{
		public Playable targetPlayable;
		public float fadeLength;
	}

	public interface IState
	{
		UnityEngine.Object entity { get; set; }
		Playable playable { get; set; }
		bool canTransite { get; }
		Type machineType { get; }
		IState CheckTransitions(out TransitionInfo info);
		void AddTransition(ITransition _transition);
		void InitState();
		void SetPlayable();
		void Enter();
		void UpdateState();
		void Exit();
	}

	public interface ITransition
	{
		UnityEngine.Object entity { get; set; }
		Type startType { get; }
		Type endType { get; }
		IState endState { get; }
		void SetEndState(IState _endState);
		void InitTransition();
		bool CheckCondition();
		TransitionInfo GetInfo();

		void EnterTransition();
		void ExitTransition();
	}

	public interface IMachine
	{
		PlayableTransition playableTransition {  get; set; }
		bool isRootMachine { get; }
		//��Ϊ��״̬��ʱʹ�ã���ʾ����graph�ϵĲ�˳�򣨶˿�˳��,���㿪ʼ
		int layer {  get; }
		Type originalType { get; }
		IState currentState { get; }
		void UpdateMachine();
		void SetStartState(IState startState);
		void AddAnyTransition(ITransition _transition);
		//���ھ�����״̬���Ƿ�����,�Լ���Ӧ��Ȩ�أ�0�������ã�
		float GetRootMachineWeight();
	}

	public interface IMachineConstructor
	{
		public Dictionary<String, IState> states {  get; }
		IMachine[] Create(UnityEngine.Object entity);
	}
}
