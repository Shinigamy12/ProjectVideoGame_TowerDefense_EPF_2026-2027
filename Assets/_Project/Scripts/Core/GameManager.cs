using System;
using UnityEngine;

namespace _Project.Scripts.Core
{
    // Exécuté avant les autres scripts : GameManager.Instance existe déjà quand ils s'abonnent à ses events dans OnEnable
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Réglages")]
        [Tooltip("Nombre de manches à tenir pour gagner")]
        [SerializeField, Range(1, 20)] private int _maxRounds = 10;

        private GamePhase _currentPhase;
        private int _currentRound;
        private bool _isPaused;

        public GamePhase CurrentPhase => _currentPhase;
        public int CurrentRound => _currentRound;
        public int MaxRounds => _maxRounds;
        public bool IsPaused => _isPaused;
        public bool IsGameOver => _currentPhase == GamePhase.Victory || _currentPhase == GamePhase.Defeat;

        public event Action<GamePhase> PhaseChanged;
        public event Action<bool> PauseChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _currentPhase = GamePhase.DrawCard;
            _currentRound = 1;
            _isPaused = false;
        }

        private void Start()
        {
            // Lancé dans Start et pas dans Awake : à ce moment, tous les scripts de la scène sont abonnés à PhaseChanged
            ChangePhase(GamePhase.DrawCard);
        }

        private void OnDestroy()
        {
            if (Instance != this)
            {
                return;
            }

            Instance = null;

            // Time.timeScale est global : sans ça, quitter la scène pendant une pause figerait aussi le menu
            Time.timeScale = 1f;
        }

        [ContextMenu("Tirages terminés")]
        public void EndDrawing()
        {
            if (_currentPhase != GamePhase.DrawCard)
            {
                return;
            }

            ChangePhase(GamePhase.PlacementDefense);
        }

        [ContextMenu("Lancer la vague")]
        public void StartWave()
        {
            if (_currentPhase != GamePhase.PlacementDefense)
            {
                return;
            }

            ChangePhase(GamePhase.WaveDefense);
        }

        [ContextMenu("Vague terminée")]
        public void EndWave()
        {
            if (_currentPhase != GamePhase.WaveDefense)
            {
                return;
            }

            if (_currentRound >= _maxRounds)
            {
                ChangePhase(GamePhase.Victory);
                return;
            }

            _currentRound++;
            ChangePhase(GamePhase.DrawCard);
        }

        // TODO SCRUM-171 : appeler cette méthode depuis l'event de mort du composant PV de la tour
        [ContextMenu("Tour détruite")]
        public void TriggerDefeat()
        {
            if (IsGameOver)
            {
                return;
            }

            ChangePhase(GamePhase.Defeat);
        }

        [ContextMenu("Pause / reprise")]
        public void TogglePause()
        {
            SetPaused(!_isPaused);
        }

        public void SetPaused(bool isPaused)
        {
            if (IsGameOver || _isPaused == isPaused)
            {
                return;
            }

            _isPaused = isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            PauseChanged?.Invoke(_isPaused);
        }

        private void ChangePhase(GamePhase newPhase)
        {
            _currentPhase = newPhase;
            Debug.Log($"[GameManager] Manche {_currentRound}/{_maxRounds} : phase {newPhase}");
            PhaseChanged?.Invoke(newPhase);
        }
    }
}
