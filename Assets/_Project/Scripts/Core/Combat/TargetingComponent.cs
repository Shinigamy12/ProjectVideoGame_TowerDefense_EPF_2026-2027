using _Project.Scripts.Core.Health;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Project.Scripts.Core.Combat
{
    public class TargetingComponent : MonoBehaviour
    {
        [Header("Ciblage")] 
        [Tooltip("Layer ciblé (Enemy, Wall, Tower)")] 
        [SerializeField] private LayerMask _targetLayers;

        [Tooltip("Portée max de ciblage")] 
        [SerializeField, Min(0.1f)] private float _targetRange = 5f;

        public float TargetRange => _targetRange;

        public IDamageable FindClosestTarget()
        {
            var colliders = Physics.OverlapSphere(transform.position, _targetRange, _targetLayers);
            IDamageable closestTarget = null;
            var closestSqrDistance = float.MaxValue;

            foreach (var hit in colliders)
            {
                var target = hit.GetComponentInParent<IDamageable>();
                if (target is { IsAlive: true })
                {
                    var offset = target.transform.position - transform.position;
                    offset.y = 0f;
                    var sqrDistance = offset.sqrMagnitude;
                    if (sqrDistance < closestSqrDistance)
                    {
                        closestSqrDistance = sqrDistance;
                        closestTarget = target;
                    }
                }
            }
            return closestTarget;
        }
        
        // Sert à afficher la portée uniquement dans l'éditeur
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, _targetRange);
        }
        #endif
    }
}