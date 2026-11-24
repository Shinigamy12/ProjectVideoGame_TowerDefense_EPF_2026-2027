using _Project.Scripts.Core.Placement;
using UnityEngine;

namespace _Project.Scripts.Cards
{
    [CreateAssetMenu(fileName = "Card_Building_", menuName = "Cards/Building Card")]
    public class BuildingCardData : CardData
    {
        [Header("Bâtiment")]
        [Tooltip("Prefab posé sur la grille. Sa racine doit avoir un composant qui implémente IGridOccupant")]
        [SerializeField] private GameObject _buildingPrefab;

        public GameObject BuildingPrefab => _buildingPrefab;

        public override void Apply(CardContext context)
        {
            // TODO SCRUM-124 : ajouter la carte à la liste des défenses (SCRUM-30)
            Debug.LogWarning($"Effet de la carte {DisplayName} pas encore branché (SCRUM-124)", this);
        }

        // Unity ne sait pas afficher un champ typé interface dans l'inspector : on garde un GameObject
        // et on vérifie ici qu'il implémente bien le contrat de la grille.
        private void OnValidate()
        {
            if (_buildingPrefab != null && _buildingPrefab.GetComponent<IGridOccupant>() == null)
            {
                Debug.LogWarning($"{name} : le prefab {_buildingPrefab.name} n'a pas de composant IGridOccupant", this);
            }
        }
    }
}
