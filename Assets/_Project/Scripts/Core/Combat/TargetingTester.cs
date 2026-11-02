using UnityEngine;

namespace _Project.Scripts.Core.Combat
{
    // Exemple et test du composant Ciblage : clic droit sur le composant dans l'inspector pour lancer le test.
    // À lancer en mode Play : les PV des cibles ne sont remplis qu'au Awake, sinon toutes les cibles sont considérées comme mortes.
    public class TargetingTester : MonoBehaviour
    {
        [Header("Réglages")]
        [Tooltip("Composant Ciblage à tester")]
        [SerializeField] private TargetingComponent _targeting;
        [Tooltip("Durée d'affichage du trait vers la cible trouvée, en secondes")]
        [SerializeField, Min(0.1f)] private float _lineDuration = 3f;

        [ContextMenu("Chercher la cible la plus proche")]
        private void FindTarget()
        {
            var target = _targeting.FindClosestTarget();
            if (target == null)
            {
                Debug.Log("FindClosestTarget : aucune cible à portée");
                return;
            }

            Debug.Log($"FindClosestTarget : {target.transform.name}");
            // Trait visible dans la vue Scene uniquement
            Debug.DrawLine(_targeting.transform.position, target.transform.position, Color.green, _lineDuration);
        }
    }
}
