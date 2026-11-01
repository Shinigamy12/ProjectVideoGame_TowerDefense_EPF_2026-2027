using UnityEngine;
using _Project.Scripts.Core;
using TMPro;

namespace _Project.Scripts.UI
{
    public class RoundPhaseDisplay : MonoBehaviour
    {
        [Header("Références")] [SerializeField]
        private GameManager _gameManager;

        [SerializeField] private TextMeshProUGUI _roundPhaseText;

        private void OnEnable()
        {
            _gameManager.PhaseChanged += OnPhaseChanged;
            _gameManager.RoundStarted += OnRoundStarted;
        }

        private void OnDisable()
        {
            _gameManager.PhaseChanged -= OnPhaseChanged;
            _gameManager.RoundStarted -= OnRoundStarted;
        }

        private void OnPhaseChanged(GamePhase phase)
        {
            RefreshText();
        }

        private void OnRoundStarted(int round)
        {
            RefreshText();
        }

        private void RefreshText()
        {
            _roundPhaseText.text =
                $"Manche {_gameManager.CurrentRound}/{_gameManager.MaxRounds} : phase {_gameManager.CurrentPhase}";
        }
    }
}