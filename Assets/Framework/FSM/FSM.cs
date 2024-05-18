using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;

namespace FSM
{
	public abstract class State : IState
	{
		public UnityEngine.Object entity {  get;  set; }
		public Playable playable { get; set; }
		private List<ITransition> transitions = new List<ITransition>();

		public bool canTransite { get; protected set; } = true;

		public Type machineType { get; protected set; }

		public State() { }

		public void AddTransition(ITransition _transition)
		{
			transitions.Add(_transition);
		}

		public IState CheckTransitions(out TransitionInfo info)
		{
			foreach (ITransition _transition in transitions)
			{
				if (_transition.CheckCondition())
				{
					info = _transition.GetInfo();
					return _transition.endState;
				}
			}
			TransitionInfo info_temp;
			info_temp.fadeLength = 0;
			info_temp.targetPlayable = Playable.Null;
			info = info_temp;
			return null;
		}

		public virtual void Enter()
		{
			foreach(ITransition _transition in transitions)
			{
				_transition.EnterTransition();
			}
		}

		public abstract void UpdateState();

		public virtual void Exit()
		{
			foreach (ITransition _transition in transitions)
			{
				_transition.ExitTransition();
			}
		}

		public abstract void InitState();

		public abstract void SetPlayable();

		public void SetStateInitValue(Type _machineType)
		{
			machineType = _machineType;
		}
	}

	public abstract class Transition : ITransition
	{
		public UnityEngine.Object entity { get; set; }
		private Func<bool> condition;
		private float fadeLength;
		public Type startType { get; protected set; }
		public Type endType { get; protected set; }
		public IState endState { get; private set; }

		public Transition() { }
		//����װʱ����
		public void SetEndState(IState _endState)
		{
			endState = _endState;
		}
		//Ӧ����д��Init�е���SetCondition��SetType�Խ��г�ʼ������
		public abstract void InitTransition();

		public virtual void EnterTransition() { }
		public virtual void ExitTransition() { }
		protected void SetInitValue(Type _startType, Type _endType, float _fadeLength, Func<bool> _condition)
		{
			startType = _startType;
			endType = _endType;
			fadeLength = _fadeLength;
			condition = _condition;
		}

		public TransitionInfo GetInfo()
		{
			TransitionInfo info;
			info.targetPlayable = endState.playable;
			info.fadeLength = fadeLength;
			return info;
		}

		public bool CheckCondition()
		{
			return condition != null ? condition.Invoke() : false;
		}
	}

	public abstract class Machine : State, IMachine
	{
		public PlayableTransition playableTransition { get; set; }
		public bool isRootMachine { get; protected set; }
		public int layer { get; protected set; }
		public Type originalType { get; protected set; }
		public IState currentState { get; private set; }

		private IState originalState;

		private List<ITransition> anyTransitions = new List<ITransition>();

		public Machine()
		{
			InitMachine();
		}

		protected void SetMachineInitValue(bool _isRootMachine, Type _originalType, int _layer = 0)
		{
			isRootMachine = _isRootMachine;
			originalType = _originalType;
			layer = _layer;
		}
		//Ӧ��Init�е���SetMachine
		protected abstract void InitMachine();
		//����װʱ����
		public void SetStartState(IState startState)
		{
			originalState = startState;
			currentState = startState;
		}

		public void AddAnyTransition(ITransition _transition)
		{
			anyTransitions.Add(_transition);
		}

		private void ChangeState()
		{
			if (!currentState.canTransite)
				return;
			IState newState = null;
			TransitionInfo info;
			info.fadeLength = 0;
			info.targetPlayable = Playable.Null;
			foreach (var _transition in anyTransitions)
			{
				if (_transition.CheckCondition() && _transition.endState != currentState)
				{
					newState = _transition.endState;
					info = _transition.GetInfo();
				}
			}
			if (newState == null)
				newState = currentState.CheckTransitions(out info);


			if (newState != null)
			{
				currentState.Exit();
				currentState = newState;
				currentState.Enter();
				CrossFade(info);
			}

		}

		private void CrossFade(TransitionInfo info)
		{
			playableTransition.Crossfade(info.targetPlayable, info.fadeLength);
		}
		public void UpdateMachine()
		{
			ChangeState();
			currentState.UpdateState();
		}

		public abstract float GetRootMachineWeight();

		//Enter��UpdateState��Exit��Ϊ��״̬��ʱ�ᱻ����
		public override void Enter()
		{
			//������״̬�����ִ�г�ʼ״̬
			currentState = originalState;
			currentState.Enter();
		}

		public override void UpdateState()
		{
			UpdateMachine();
		}

		public override void Exit()
		{
			currentState.Exit();
		}
	}

	//ʹ�ù��������һ����״̬����state��transition���б�ʶ
	public class MachineConstructor<T_state, T_machine, T_transition> : IMachineConstructor
		where T_state : IState, new() where T_machine : IMachine, new() where T_transition : ITransition, new()

	{
		public Dictionary<String, IState> states { get; private set; } = new Dictionary<String, IState>();
		private Dictionary<Type, IMachine> machines = new Dictionary<Type, IMachine>();
		private Dictionary<Type, ITransition> transitions = new Dictionary<Type, ITransition>();

		public IMachine[] Create(UnityEngine.Object entity)
		{
			Init(entity);
			Construct();
			List<IMachine> rootMachines = new List<IMachine>();
			foreach (IMachine _machine in machines.Values)
			{
				if (_machine.isRootMachine)
					rootMachines.Add(_machine);
			}

			return rootMachines.ToArray();
		}
		private void Init(UnityEngine.Object entity)
		{
			Type[] types = Assembly.GetCallingAssembly().GetTypes();
			foreach (var type in types)
			{
				if (type.IsSubclassOf(typeof(T_state)))
				{
					IState state = Activator.CreateInstance(type) as IState;
					state.entity = entity;
					state.InitState();
					states.Add(type.Name, state);
				}
				else if (type.IsSubclassOf(typeof(T_machine)))
				{
					object stateAndMachine = Activator.CreateInstance(type);
					states.Add(type.Name, stateAndMachine as IState);
					machines.Add(type, stateAndMachine as IMachine);
				}
				else if (type.IsSubclassOf(typeof(T_transition)))
				{
					ITransition transition = Activator.CreateInstance(type) as ITransition;
					transition.entity = entity;
					transition.InitTransition();
					transitions.Add(type, transition);
				}

			}
		}

		private void Construct()
		{
			//װ��transitions
			foreach (var _transition in transitions.Values)
			{
				_transition.SetEndState(states[_transition.endType.Name]);
			}

			//����machine�ĳ�ʼ״̬
			foreach (var _machine in machines.Values)
			{
				_machine.SetStartState(states[_machine.originalType.Name]);
			}

			//��transitionsװ�䵽state��machine��
			foreach (var transition in transitions.Values)
			{
				if (transition.startType == null)
				{
					var machine = machines[transition.endState.machineType];
					machine.AddAnyTransition(transition);
				}
				else
				{
					var state = states[transition.startType.Name];
					state.AddTransition(transition);
				}
			}
		}
	}
}
