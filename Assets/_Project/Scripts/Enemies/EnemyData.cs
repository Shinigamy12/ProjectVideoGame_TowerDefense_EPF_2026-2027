using UnityEngine;

namespace _Project.Scripts.Enemies
{
    // Données d'un type d'ennemi (SCRUM-137), lues par le PV des ennemis (SCRUM-128), le déplacement (SCRUM-41 / SCRUM-43)
    // et les dégâts à la tour (SCRUM-129). Un asset par type dans ScriptableObjects/Enemies.
    [CreateAssetMenu(fileName = "Enemy_", menuName = "Jeu/Ennemis/Type d'ennemi", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [Header("Identité")]
        [Tooltip("Nom affiché en jeu")]
        [SerializeField] private string _displayName = "Ennemi";

        [Header("Stats")]
        [Tooltip("PV au moment de l'apparition")]
        [SerializeField, Min(1f)] private float _maxHealth = 10f;
        [Tooltip("Vitesse de déplacement, en cases par seconde (1 case = 1 unité Unity)")]
        [SerializeField, Min(0.1f)] private float _moveSpeed = 2f;
        [Tooltip("Dégâts infligés à la tour quand l'ennemi l'atteint")]
        [SerializeField, Min(0f)] private float _towerDamage = 1f;

        public string DisplayName => _displayName;
        public float MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float TowerDamage => _towerDamage;
    }
}
