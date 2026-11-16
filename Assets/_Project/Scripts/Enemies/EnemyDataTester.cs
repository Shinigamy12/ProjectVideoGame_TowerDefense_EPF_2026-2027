using _Project.Scripts.Core.Health;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    // Exemple et test des types d'ennemis (SCRUM-137) : clic droit sur le composant dans l'inspector pour lancer les tests.
    // « Appliquer » est à lancer en mode Play : les PV de la cible ne sont remplis qu'au Awake.
    public class EnemyDataTester : MonoBehaviour
    {
        [Header("Réglages")]
        [Tooltip("Types d'ennemis à tester")]
        [SerializeField] private EnemyData[] _enemyTypes;
        [Tooltip("Composant PV qui reçoit les PV max du type choisi")]
        [SerializeField] private HealthComponent _target;
        [Tooltip("Index du type appliqué à la cible")]
        [SerializeField, Min(0)] private int _selectedIndex;

        [ContextMenu("Afficher les types d'ennemis")]
        private void LogEnemyTypes()
        {
            if (_enemyTypes == null || _enemyTypes.Length == 0)
            {
                Debug.LogWarning("EnemyDataTester : aucun type d'ennemi renseigné");
                return;
            }

            foreach (var data in _enemyTypes)
            {
                if (data == null)
                {
                    Debug.LogWarning("EnemyDataTester : case vide dans la liste");
                    continue;
                }

                Debug.Log($"{data.DisplayName} : {data.MaxHealth} PV, {data.MoveSpeed} cases/s, {data.TowerDamage} dégâts à la tour");
            }
        }

        [ContextMenu("Appliquer le type choisi à la cible")]
        private void ApplySelected()
        {
            if (_target == null || _enemyTypes == null || _selectedIndex >= _enemyTypes.Length || _enemyTypes[_selectedIndex] == null)
            {
                Debug.LogWarning("EnemyDataTester : cible ou type choisi manquant");
                return;
            }

            var data = _enemyTypes[_selectedIndex];
            _target.SetMaxHealth(data.MaxHealth);
            Debug.Log($"{_target.name} devient {data.DisplayName} : {_target.CurrentHealth}/{_target.MaxHealth} PV");
    }
}
