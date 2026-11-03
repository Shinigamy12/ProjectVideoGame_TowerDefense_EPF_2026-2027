using _Project.Scripts.Core.Health;
using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    // Exemple et test de la classe Projectile (SCRUM-174) : sous-classe minimale, guidée vers une cible fixée dans l'inspector.
    // À placer sur un objet de la scène (une sphère) : le tir part au Start, une seule fois par lancement du mode Play.
    // Cas à tester : impact (PV retirés puis destruction), cible hors d'atteinte (destruction en fin de durée de vie),
    // Attack vide (destruction à l'impact sans dégâts ni erreur), cible tuée en vol (destruction sans dégâts).
    public class ProjectileTester : Projectile
    {
        [Header("Test")]
        [Tooltip("Composant Attaque qui fournit les dégâts. Laisser vide pour tester le garde-fou")]
        [SerializeField] private AttackComponent _attack;
        [Tooltip("Cible visée par le projectile")]
        [SerializeField] private HealthComponent _target;
        [Tooltip("Distance à la cible en dessous de laquelle le projectile touche, en unités")]
        [SerializeField, Range(0.05f, 1f)] private float _hitDistance = 0.2f;

        private float _launchTime;

        private void Start()
        {
            // Start passe avant le premier Update : sans Initialize, la durée de vie vaudrait 0
            Initialize(_attack);
            _launchTime = Time.time;
            Debug.Log(_attack == null
                ? "Projectile lancé sans Attack : il doit disparaître à l'impact sans dégâts"
                : $"Projectile lancé : {_attack.Damage} dégâts attendus sur {_target.name}");
        }

        protected override void Move()
        {
            if (_target == null || !_target.IsAlive)
            {
                Debug.Log("Cible absente ou morte en vol : disparition sans dégâts");
                Despawn();
                return;
            }

            var targetPosition = _target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) > _hitDistance) return;

            HitTarget(_target);
            Debug.Log($"Impact sur {_target.name}, PV restants : {_target.CurrentHealth}");
        }

        [ContextMenu("Tuer la cible (test de la cible morte en vol)")]
        private void KillTarget()
        {
            if (_target == null) return;

            _target.TakeDamage(_target.CurrentHealth);
        }

        private void OnDestroy()
        {
            Debug.Log($"Projectile détruit après {Time.time - _launchTime:0.00} s");
        }
    }
}
