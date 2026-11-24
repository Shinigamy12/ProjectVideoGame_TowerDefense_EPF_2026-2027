using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Cards
{
    // Affichage d'une carte à l'écran. Un seul prefab pour toutes les cartes : il se remplit avec la CardData reçue.
    public class CardView : MonoBehaviour
    {
        [Header("Références")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _iconImage;

        private CardData _card;

        public CardData Card => _card;

        public void Setup(CardData card)
        {
            _card = card;
            _nameText.text = card.DisplayName;
            _descriptionText.text = card.Description;
            _iconImage.sprite = card.Icon;
            // Une Image sans sprite affiche un carré blanc : on la cache tant que la carte n'a pas d'icône
            _iconImage.enabled = card.Icon != null;
        }
    }
}
