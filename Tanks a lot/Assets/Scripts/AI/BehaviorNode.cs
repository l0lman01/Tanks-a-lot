using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Base class for all behavior tree nodes
    /// </summary>
    public abstract class BehaviorNode
    {
        protected BehaviorNode leftChild;
        protected BehaviorNode rightChild;

        // Node state enum for blackboard communication
        public enum State
        {
            Running,
            Success,
            Failure
        }

        /// <summary>
        /// Execute the node logic. Returns current execution state.
        /// </summary>
        public abstract State Execute();

    /// <summary>
    /// Called before Execute() to prepare the node
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// Called after Execute() to clean up
    /// </summary>
    public virtual void OnExit() { }
        public virtual bool IsLeaf() => leftChild == null && rightChild == null;

        /// <summary>
        /// Get the priority of this node for sequencing
        /// Higher priority nodes execute first in Sequence
        /// </summary>
        public virtual int Priority => 0;
    }
}
