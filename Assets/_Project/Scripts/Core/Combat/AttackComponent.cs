using System;
using _Project.Scripts.Core.Health;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    // Gère la cadence et les dégâts d'une attaque, quelle que soit l'arme.
    // Le choix des cibles reste à l'utilisateur : vérifier IsReady, trouver la ou les cibles,
    // puis appeler TryAttack() et, s'il réussit, DealDamage() sur chaque cible.
    public class AttackComponent : MonoBehaviour
    {
        [Header("Attaque")]
        [Tooltip("Dégâts infligés à chaque cible touchée")]
        [SerializeField, Min(0f)] private float _damage = 1f;
        [Tooltip("Délai entre deux attaques, en secondes")]
        [SerializeField, Min(0.05f)] private float _cooldown = 1f;

        // Time.time est figé quand Time.timeScale vaut 0 : la pause bloque aussi les attaques
        private float _nextAttackTime;

        public float Damage => _damage;
        public float Cooldown => _cooldown;
        public bool IsReady => Time.time >= _nextAttackTime;

        public event Action Attacked;

        public bool TryAttack()
        {
            if (!IsReady) return false;

            _nextAttackTime = Time.time + _cooldown;
            Attacked?.Invoke();
            return true;
        }

        public void DealDamage(IDamageable target)
        {
            if (target == null || !target.IsAlive) return;

            target.TakeDamage(_damage);
        }

        // Remplace les dégâts, par exemple après une amélioration d'arme.
        // L'appelant garde les dégâts de base : les bonus s'additionnent sur la base, pas sur la valeur actuelle.
        public void SetDamage(float damage)
        {
            if (damage < 0f) return;
            _damage = damage;
        }
    }
}
