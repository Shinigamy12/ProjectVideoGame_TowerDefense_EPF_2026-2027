using _Project.Scripts.Core.Health;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    public abstract class Projectile : MonoBehaviour
    {
        [Header("Réglage")]
        [Tooltip("Vitesse du tir en unités par seconde")]
        [SerializeField, Min(0.1f)] private float _speed = 2f;
        [Tooltip("Durée de vie du tir en secondes")]
        [SerializeField, Min(0.1f)] private float _maxLifeTime = 3f;
        
        private AttackComponent _attack;
        private float _currentLifeTime = 3f;
        
        protected float Speed => _speed;

        protected void Initialize(AttackComponent attackComponent)
        {
            _currentLifeTime = _maxLifeTime;
            _attack = attackComponent;
        }
        
        private void Update()
        {
            _currentLifeTime -= Time.deltaTime;
            if (_currentLifeTime <= 0f)
            {
                Despawn();
                return;
            }
            Move();
        }

        protected abstract void Move();

        protected void HitTarget(IDamageable damageable)
        {
            if(_attack != null) _attack.DealDamage(damageable);
            Despawn();
        }
        
        protected void Despawn()
        {
            Destroy(gameObject);
        }

    }
}