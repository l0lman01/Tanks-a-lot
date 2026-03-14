using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Decorator node: wraps another node to modify its behavior
    /// </summary>
    public class BehaviorNodeDecorator : BehaviorNode
    {
        private BehaviorNode _nodeToDecorate;

        public override State Execute()
        {
            if (_nodeToDecorate == null)
                return State.Failure;

            _nodeToDecorate.OnEnter();
            var result = _nodeToDecorate.Execute();
            _nodeToDecorate.OnExit();
            return result;
        }

        public void SetNodeToDecorate(BehaviorNode node)
        {
            _nodeToDecorate = node;
        }

        public BehaviorNode GetDecoratedNode() => _nodeToDecorate;
    }
}
