    using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Cards
{
    // Exemple et test : affiche une carte par CardData de la liste au lancement du mode Play
    public class CardViewTester : MonoBehaviour
    {
        [Header("Références")]
        [SerializeField] private CardView _cardViewPrefab;

        [Tooltip("Parent des cartes affichées : un objet d'un Canvas, avec un Horizontal Layout Group pour les aligner")]
        [SerializeField] private Transform _container;

        [Header("Cartes à afficher")]
        [SerializeField] private List<CardData> _cards = new();

        private readonly List<CardView> _spawnedViews = new();

        private void Start()
        {
            ShowCards();
        }

        [ContextMenu("Test : réafficher les cartes")]
        private void ShowCards()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("À lancer en mode Play", this);
                return;
            }

            ClearCards();
            foreach (CardData card in _cards)
            {
                if (card == null) continue;

                CardView view = Instantiate(_cardViewPrefab, _container);
                view.Setup(card);
                _spawnedViews.Add(view);
            }
        }

        // Montre le polymorphisme : le même appel lance l'effet propre à chaque type de carte
        [ContextMenu("Test : appliquer les effets")]
        private void ApplyEffects()
        {
            var context = new CardContext(null);
            foreach (CardData card in _cards)
            {
                if (card != null) card.Apply(context);
            }
        }

        private void ClearCards()
        {
            foreach (CardView view in _spawnedViews)
            {
                Destroy(view.gameObject);
            }
            _spawnedViews.Clear();
        }
    }
}
