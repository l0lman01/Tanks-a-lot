using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// A leaf behavior node that performs an action or evaluation
    /// </summary>
    public abstract class LeafBehaviorNode : BehaviorNode
    {
        public override bool IsLeaf() => true;
    }
}
