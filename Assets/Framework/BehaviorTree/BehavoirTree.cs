using System.Collections.Generic;

namespace BT
{
	#region Node
	public enum NodeState
	{
		Success = 0,
		Running,
		Failed
	}
	public abstract class Node
	{
		protected NodeState m_State;
		public Node parent;
		protected List<Node> m_Children;
		protected Blackboard m_Blackboard;
		protected Blackboard m_ExternalBd;

		public Node(Blackboard blackboard, Node _parent = null, List<Node> _children = null)
		{
			if (_children == null)
				m_Children = new List<Node>();
			else
			{
				m_Children = _children;
				foreach (Node child in _children)
					child.parent = this;
			}

			parent = _parent;
			m_Blackboard = blackboard;
			m_ExternalBd = ExternalBlackboard.Blackboard;
			Init();
		}

		public void AttachChild(Node _child)
		{
			_child.parent = this;
			m_Children.Add(_child);
		}

		public abstract NodeState Execute();
		protected abstract void Init();
	}
	#endregion

	#region Tree
	public abstract class BehaviorTree : UnityEngine.MonoBehaviour
	{
		private Node m_Root;
		private Node RunningNode;
		protected Blackboard m_blackboard = new Blackboard();
		protected Blackboard m_ExternalBd = ExternalBlackboard.Blackboard;

		protected virtual void Start()
		{
			m_Root = SetRoot();
		}

		protected virtual void Update()
		{
			if (RunningNode != null)
			{
				NodeState state = RunningNode.Execute();
				if (state != NodeState.Running)
					RunningNode = null;
			}
			else
				m_Root?.Execute();
		}

		protected abstract Node SetRoot();
		public void SetRunningNode(Node node) => RunningNode = node;
	}
	#endregion

	#region Sequence
	public class Sequence : Node
	{
		public Sequence(Blackboard blackboard = null, Node _parent = null, List<Node> _children = null) : base(blackboard, _parent, _children)
		{
		}

		protected override void Init() { }

		public override NodeState Execute()
		{
			foreach (var child in m_Children)
			{
				NodeState childState = child.Execute();
				switch (childState)
				{
					case NodeState.Success: continue;
					case NodeState.Running: m_State = NodeState.Running; return NodeState.Running;
					case NodeState.Failed: m_State = NodeState.Failed; return NodeState.Failed;
				}
			}
			m_State = NodeState.Success;
			return m_State;
		}
	}
	#endregion

	#region Selector
	public class Selector : Node
	{
		public Selector(Blackboard blackboard = null, Node _parent = null, List<Node> _children = null) : base(blackboard, _parent, _children)
		{
		}

		protected override void Init() { }

		public override NodeState Execute()
		{
			foreach (var child in m_Children)
			{
				NodeState childState = child.Execute();
				switch (childState)
				{
					case NodeState.Success: m_State = NodeState.Success; return NodeState.Success;
					case NodeState.Running: m_State = NodeState.Running; return NodeState.Running;
					case NodeState.Failed: continue;
				}
			}

			m_State = NodeState.Failed;
			return m_State;
		}
	}
	#endregion

	#region Parallel
	public class Parallel : Node
	{
		public Parallel(Blackboard blackboard = null, Node _parent = null, List<Node> _children = null) : base(blackboard, _parent, _children)
		{
		}

		protected override void Init() { }

		public override NodeState Execute()
		{
			bool anyNodeIsRunning = false;
			bool anyNodeIsFailed = false;
			foreach (var child in m_Children)
			{
				NodeState childState = child.Execute();
				switch (childState)
				{
					case NodeState.Success: continue;
					case NodeState.Running: anyNodeIsRunning = true; continue;
					case NodeState.Failed: anyNodeIsFailed = true; continue;
				}
			}

			if (anyNodeIsFailed)
			{
				m_State = NodeState.Failed;
				return m_State;
			}
			else if (anyNodeIsRunning)
			{
				m_State = NodeState.Running;
				return m_State;
			}
			else
			{
				m_State = NodeState.Success;
				return m_State;
			}

		}
	}
	#endregion
}