using UnityEngine;

namespace _Project.Scripts.Cards
{
    public abstract class CardData : ScriptableObject
    {
        [Header("Affichage")]
        [Tooltip("Nom affiché sur la carte")]
        [SerializeField] private string _displayName;

        [Tooltip("Texte affiché sous le nom, qui explique l'effet de la carte")]
        [SerializeField, TextArea(2, 4)] private string _description;

        [SerializeField] private Sprite _icon;

        [Header("Tirage")]
        [Tooltip("Chance relative d'être tirée : une carte de poids 2 sort deux fois plus souvent qu'une carte de poids 1")]
        [SerializeField, Range(0.1f, 10f)] private float _weight = 1f;

        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float Weight => _weight;

        public abstract void Apply(CardContext context);
    }
}
