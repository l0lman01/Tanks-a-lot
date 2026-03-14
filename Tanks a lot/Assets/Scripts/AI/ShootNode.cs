using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Leaf node that commands the tank to shoot at detected targets
    /// </summary>
    public class ShootNode : LeafBehaviorNode
    {
        [SerializeField] protected float shootCooldown = 0.5f;

        private TankController _tankController;
        private float _shootTimer = 0f;

        /// <summary>
        /// Initialize this node with a tank controller reference
        /// </summary>
        public void Initialize(TankController tankController)
        {
            _tankController = tankController;
            _shootTimer = shootCooldown;
        }

        public override BehaviorNode.State Execute()
        {
            OnEnter();

            if (_tankController == null)
            {
                Debug.LogWarning("[AI] ShootNode: Tank controller not initialized!");
                return BehaviorNode.State.Failure;
            }

            // Update cooldown timer
            _shootTimer -= Time.deltaTime;

            // Shoot if cooldown has expired
            if (_shootTimer <= 0)
            {
                _tankController.HandleShoot();
                _shootTimer = shootCooldown;
                Debug.Log("[AI] Tank fired!");
            }

            OnExit();
            return BehaviorNode.State.Success; // Shooting is always considered a success
        }

        /// <summary>
        /// Reset the shoot cooldown (for testing or external control)
        /// </summary>
        public void ResetCooldown()
        {
            _shootTimer = 0;
        }
    }
}
