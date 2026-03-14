using System;
using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Action node: executes a void delegate when invoked
    /// </summary>
    public class ActionNode : LeafBehaviorNode
    {
        public Action OnExecute;

        public override BehaviorNode.State Execute()
        {
            OnEnter();
            OnExecute?.Invoke();
            OnExit();
            return BehaviorNode.State.Success;
        }

        public override int Priority => 1;
    }

    /// <summary>
    /// Condition node: evaluates a predicate delegate
    /// </summary>
    public class ConditionNode : LeafBehaviorNode
    {
        public System.Predicate<object> CheckCondition;
        private object _targetData;

        public void SetTargetData(object data) => _targetData = data;

        public override BehaviorNode.State Execute()
        {
            OnEnter();
            var result = CheckCondition?.Invoke(_targetData) ?? false;
            OnExit();
            return result ? BehaviorNode.State.Success : BehaviorNode.State.Failure;
        }
    }

    /// <summary>
    /// AlwaysTrue node: always returns success
    /// </summary>
    public class AlwaysTrueNode : LeafBehaviorNode
    {
        public override BehaviorNode.State Execute()
        {
            OnEnter();
            OnExit();
            return BehaviorNode.State.Success;
        }

        public override int Priority => 100; // Always execute last
    }

    /// <summary>
    /// AlwaysFalse node: always returns failure
    /// </summary>
    public class AlwaysFalseNode : LeafBehaviorNode
    {
        public override BehaviorNode.State Execute()
        {
            OnEnter();
            OnExit();
            return BehaviorNode.State.Failure;
        }

        public override int Priority => 0; // Never execute first
    }
}
