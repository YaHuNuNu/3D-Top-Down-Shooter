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
		//作为根状态机时使用，表示其在graph上的层顺序（端口顺序）,从零开始
		int layer {  get; }
		Type originalType { get; }
		IState currentState { get; }
		void UpdateMachine();
		void SetStartState(IState startState);
		void AddAnyTransition(ITransition _transition);
		//用于决定根状态机是否启用,以及对应的权重（0代表不启用）
		float GetRootMachineWeight();
	}

	public interface IMachineConstructor
	{
		public Dictionary<String, IState> states {  get; }
		IMachine[] Create(UnityEngine.Object entity);
	}
}
