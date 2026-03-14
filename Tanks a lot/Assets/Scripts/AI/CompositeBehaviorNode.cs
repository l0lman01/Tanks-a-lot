using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Selector node: executes children until one succeeds (FirstMatch behavior)
    /// </summary>
    public class SelectorNode : CompositeBehaviorNode
    {
        protected override BehaviorNode.State ExecuteChildren()
        {
            foreach (var child in children)
            {
                child.OnEnter();
                var result = child.Execute();

                if (result == BehaviorNode.State.Success)
                    return BehaviorNode.State.Success;

                child.OnExit();
            }

            return BehaviorNode.State.Failure;
        }

        public override string ToString() => "Selector";
    }

    /// <summary>
    /// Sequence node: executes all children in order, fails if any fails (Sequence behavior)
    /// </summary>
    public class SequenceNode : CompositeBehaviorNode
    {
        protected override BehaviorNode.State ExecuteChildren()
        {
            foreach (var child in children)
            {
                child.OnEnter();
                var result = child.Execute();

                if (result == BehaviorNode.State.Failure)
                    return BehaviorNode.State.Failure;

                child.OnExit();
            }

            return BehaviorNode.State.Success;
        }

        public override string ToString() => "Sequence";
    }

    /// <summary>
    /// Base class for composite nodes that can have children
    /// </summary>
    public abstract class CompositeBehaviorNode : BehaviorNode
    {
        public override void OnEnter() { }
        public override void OnExit() { }

        public override int Priority => 99; // Execute after leaf nodes

        protected System.Collections.Generic.List<BehaviorNode> children = new System.Collections.Generic.List<BehaviorNode>();

        public override State Execute()
        {
            OnEnter();
            var result = ExecuteChildren();
            OnExit();
            return result;
        }

        public virtual void AddChild(BehaviorNode node)
        {
            if (node != null && !children.Contains(node))
                children.Add(node);
        }

        public void RemoveChild(BehaviorNode node)
        {
            children.Remove(node);
        }

        protected virtual BehaviorNode.State ExecuteChildren() => BehaviorNode.State.Failure;
    }
}
